using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour, Tornable, Avisable
{
    [SerializeField] PlayerSO playerBase;
    [SerializeField] HealthBar vidaPantalla;
    [SerializeField] ManaBar manaPantalla;
    [SerializeField] List<AtacSO> atacs;
    public event Action<AtacSO> onAttack;
    Animator animator;
    [SerializeField] AnimationClip atacClip;
    [SerializeField] AnimationClip hurtClip;
    [SerializeField] TextMeshProUGUI textoTarget;

    [SerializeField] AtacSO ataqueBasico;
    [SerializeField] HpMaxJugador HpMax;

    enum CombatStates { WAITING, SELECT_ACTION, SELECT_MAGIC, ACTION_MAGIC, SELECT_OBJECT, ACTION_RUN, SELECCIONAR_TARGET }
    [SerializeField] CombatStates combatState;
    enum PlayerAnimations { IDLE, HURT, ATTACK }
    [SerializeField] PlayerAnimations actualState;
    [SerializeField] float stateTime;
    int hp;
    public int lvl;
    int mana;
    int def;
    int damageAtk;
    int spd;
    int xp;
    public int Xp { get => xp; set => xp = value; }
    public bool entroSeleccionado { get; private set; }
    EstadosAlterados estado = null;
    [SerializeField] List<AtacSO> atacsBase;
    AtacSO atacSeleccionat;
    GameObject target;
    public event Action onMuerto;
    //Accions GUI
    public event Action OnMostrarAccions;
    public event Action OnOcultarAccions;
    public event Action<List<AtacSO>> OnMostrarMagia;
    public event Action OnOcultarMagia;
    public event Action OnDeshabilitarAccions;
    public event Action OnEntrarSeleccionarTarget;
    public event Action OnFugir;
    public event Action<string> OnIniciarTornUI;
    public event Action<string, string> OnRebreEstadoAlteradoUI;
    public event Action<string, int> OnRebreMalUI;
    public event Action<string> OnEmpezarVentajaUI;
    public event Action OnSeleccionarTargetUI;
    public event Action OnSeleccionatTargetUI;
    public event Action OnFallarHuirUI;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        IniState(PlayerAnimations.IDLE);
        Iniciar(playerBase);
    }

    public void Iniciar(PlayerSO player)
    {
        this.hp = playerBase.Hp;
        this.xp = playerBase.Xp;
        this.lvl = playerBase.Lvl;
        this.mana = playerBase.Mana;
        this.def = playerBase.Def;
        this.damageAtk = playerBase.DamageAtk;
        this.spd = playerBase.Spd;
        this.atacs = playerBase.listaAtaques;
        vidaPantalla.IniciarBarra(HpMax.hpMax);
        vidaPantalla.UpdateHealth(HpMax.hpMax - this.hp);
        manaPantalla.IniciarBarra(HpMax.manaMax);
        if (HpMax.manaMax > 0)
            manaPantalla.UpdateHealth(HpMax.manaMax - this.mana);
        else
            manaPantalla.BuidaBarra();
        if (player.estadosAlterados != null)
        {
            //this.estado=estado.IniciarEstadoAlterado(player.estadosAlterados);
            this.estado = new EstadosAlterados(player.estadosAlterados.nom, player.estadosAlterados.incapacitat, player.estadosAlterados.torns, player.estadosAlterados.hp, player.estadosAlterados.modAtk, player.estadosAlterados.modDef, player.estadosAlterados.modSpd);
            Debug.Log($"{gameObject}/{this}: INICIO ESTADO ALTERADO: {player.estadosAlterados.nom}");
        }
    }

    public void RebreMal(AtacSO atac)
    {
        Debug.Log($"{gameObject}/{this} VIDA ABANS REBRE MAL: {this.hp}");

        ChangeState(PlayerAnimations.HURT);
        OnRebreMalUI?.Invoke("player", atac.mal);
       
        this.hp -= (atac.mal-def);
        if (this.hp <= 0)
        {
            Debug.Log($"{gameObject}/{this} He mort!");
            onMuerto?.Invoke();
        }
        else
        {
            Debug.Log($"VIDA DESPRÉS REBRE MAL: {this.hp}");
            vidaPantalla.UpdateHealth((atac.mal-def));

            if (atac.estat != null && estado == null)
            {
                this.estado = new EstadosAlterados(atac.estat.nom, atac.estat.incapacitat, atac.estat.torns, atac.estat.hp, atac.estat.modAtk, atac.estat.modDef, atac.estat.modSpd);
            }
        }
    }

    public void lvlUP()
    {
        lvl++;
        hp += 10;
        mana += 10;
        HpMax.hpMax += 10;
        HpMax.manaMax += 10;
        def += 1;

        foreach (AtacSO a in atacs)
        {
            a.mal += 8;
        }
        ataqueBasico.mal += 5;

        if (lvl == 2)
        {
            atacs.Add(atacsBase[0]);
        }
        else if (lvl == 5)
        {
            atacs.Add(atacsBase[1]);
            damageAtk += 1;
        }
        else if (lvl == 10)
        {
            atacs.Add(atacsBase[2]);
            damageAtk += 1;
        }
        else if (lvl == 15)
        {
            atacs.Add(atacsBase[3]);
            damageAtk += 1;
        }
        
    }

    public void SavePlayer()
    {
        playerBase.Mana = this.mana;
        playerBase.Hp = this.hp;
        playerBase.Spd = this.spd;
        playerBase.Def = this.def;
        playerBase.DamageAtk = this.damageAtk;
        playerBase.Lvl = this.lvl;
        playerBase.listaAtaques = this.atacs;
        playerBase.Xp = this.xp;
    }


    public void IniciarTorn()
    {
        Assert.AreEqual(combatState, CombatStates.WAITING, $"{gameObject}: Iniciant torn quan no s'est� esperant.");
        ChangeState(CombatStates.SELECT_ACTION);
    }

    public void AcabarTorn()
    {
        ProcessarEstat();
        Debug.Log($"{gameObject}/{this}: He acabat el torn");
        GameManagerArena.Instance.BucleJoc();
    }

    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        if (tempsDespera > 0)
            yield return new WaitForSeconds(tempsDespera);
        else
            yield return null;
        accio();
    }

    //FSM COMBAT
    #region FSM
    private void ChangeState(CombatStates newstate)
    {
        Debug.Log($"---------------------- Sortint de {combatState} a {newstate} ------------------------");
        ExitState(combatState);

        Debug.Log($"---------------------- Entrant a {newstate} ------------------------");
        InitState(newstate);
    }

    private void InitState(CombatStates newstate)
    {
        combatState = newstate;
        switch (combatState)
        {
            case CombatStates.WAITING:
                OnOcultarAccions?.Invoke();
                break;
            case CombatStates.SELECT_ACTION:
                //Si el enemigo empieza con ventaja. Incapacitat sempre ser� true en aquest cas.
                if (estado != null && estado.Incapacitat && estado.Torns > 0)
                {
                    OnEmpezarVentajaUI?.Invoke("L'enemic");
                    Debug.Log($"{gameObject}/{this}: INICIO ESTADO ALTERADO: {estado.Nom}");
                    estado.Torns--;
                    playerBase.estadosAlterados = null;
                    //TODO: Mirar qu� passa
                    StartCoroutine(EsperarIActuar(3, () => AcabarTorn()));
                    ChangeState(CombatStates.WAITING);
                    break;
                }
                else
                {
                    OnMostrarAccions?.Invoke();
                    OnIniciarTornUI?.Invoke("jugador");
                }
                break;

            case CombatStates.SELECT_MAGIC:
                OnMostrarMagia?.Invoke(atacs);
                OnDeshabilitarAccions?.Invoke();
                break;
            case CombatStates.ACTION_MAGIC:
                ChangeState(PlayerAnimations.ATTACK);
                this.mana -= atacSeleccionat.mana;
                manaPantalla.UpdateHealth(atacSeleccionat.mana);
                target.GetComponent<EnemyArena>().RebreMal(atacSeleccionat, this.damageAtk);
                break;
            case CombatStates.ACTION_RUN:
                StartCoroutine(EsperarIActuar(0, () => OnFugir?.Invoke()));
                ChangeState(CombatStates.WAITING);
                break;
            case CombatStates.SELECCIONAR_TARGET:
                OnSeleccionarTargetUI?.Invoke();
                entroSeleccionado = true;
                GameManagerArena.Instance.OnSeleccionarTarget += TargetSeleccionat;
                OnDeshabilitarAccions.Invoke();
                break;
        }
    }

    private void ExitState(CombatStates currentState)
    {
        Assert.AreEqual(combatState, currentState, $"{gameObject}: Est�s cridant un sortir d'estat quan no est�s a aquest estat");
        switch (currentState)
        {
            case CombatStates.WAITING:
                break;
            case CombatStates.SELECT_ACTION:
                //OnOcultarAccions?.Invoke();
                break;
            case CombatStates.SELECT_MAGIC:
                OnOcultarMagia?.Invoke();
                break;
            case CombatStates.ACTION_MAGIC:
                StartCoroutine(EsperarIActuar(1, () => AcabarTorn()));
                break;
            case CombatStates.SELECCIONAR_TARGET:
                OnSeleccionatTargetUI?.Invoke();
                entroSeleccionado = false;
                break;
        }
    }

    #endregion
    private void ProcessarEstat()
    {
        //DARLE A UNA VUELTA POR SI HACEMOS QUE LA VIDA SEA NEGATIVA EN CASO DE QUE QUITE VIDA.
        if (estado != null)
        {
            OnRebreEstadoAlteradoUI?.Invoke("player", estado.Nom);
            ChangeState(PlayerAnimations.HURT);
            this.hp -= estado.Hp;
            this.def += estado.ModDef;
            this.damageAtk += estado.ModAtk;
            this.spd += estado.ModSpd;
            estado.Torns--;
            if (estado.Torns <= 0)
            {
                Debug.Log($"L'estat {estado.Nom} ha finalitzat");
                estado = null;
            }

        }
    }

    public void AtacAcabat()
    {
        Debug.Log("Canviat a waiting després d'atacar");
        ChangeState(CombatStates.WAITING);
    }


    //FSM ANIMACIONS
    private void ChangeState(PlayerAnimations newstate)
    {
        ExitState(actualState);
        IniState(newstate);
    }
    private void IniState(PlayerAnimations initState)
    {
        actualState = initState;
        stateTime = 0f;

        switch (actualState)
        {
            case PlayerAnimations.IDLE:
                animator.Play("Idle");
                break;
            case PlayerAnimations.ATTACK:
                animator.Play("atac2");
                StartCoroutine(EsperarIActuar(atacClip.length, () => ChangeState(PlayerAnimations.IDLE)));
                break;
            case PlayerAnimations.HURT:
                animator.Play("Hurt");
                StartCoroutine(EsperarIActuar(hurtClip.length, () => ChangeState(PlayerAnimations.IDLE)));
                break;
            default:
                break;
        }
    }

    private void ExitState(PlayerAnimations exitState)
    {
        switch (exitState)
        {
            case PlayerAnimations.IDLE:
                break;
            case PlayerAnimations.ATTACK:
                break;
            case PlayerAnimations.HURT:
                break;
            default:
                break;
        }
    }


    //Accions Menu
    internal void AccioAtacar()
    {

        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� atack quan no s'est� esperant una selecci�.");
        this.atacSeleccionat = ataqueBasico;
        ChangeState(CombatStates.SELECCIONAR_TARGET);

    }

    internal void AccioSeleccionarMagia()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� magia quan no s'est� esperant una selecci�.");
        ChangeState(CombatStates.SELECT_MAGIC);
    }

    internal void AccioFugir()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� fugir quan no s'est� esperant una selecci�.");
        int random = UnityEngine.Random.Range(1, 101);
        if (random <= 50)
        {
            ChangeState(CombatStates.ACTION_RUN);
        }
        else
        {
            OnFallarHuirUI?.Invoke();
            ChangeState(CombatStates.WAITING);
            StartCoroutine(EsperarIActuar(5, () => AcabarTorn()));
        }
    }

    public void AccioCancelar()
    {
        switch (combatState)
        {
            case CombatStates.SELECT_MAGIC:
            case CombatStates.SELECT_OBJECT:
                ChangeState(CombatStates.SELECT_ACTION);
                break;
        }
    }

    //FUNCIO QUE ES CRIDA QUAN SE SELCCIONA UN BOTO D'ATAC.
    public void AccioHabilitat(AtacSO atac)
    {

        //comprovar mana, estat, tot i canviar
        if (this.mana >= atac.mana)
        {
            this.atacSeleccionat = atac;

            ChangeState(CombatStates.SELECCIONAR_TARGET);

        }
        else
        {
            ChangeState(CombatStates.SELECT_ACTION);
        }
    }

    //FUNCIO QUE ES CRIDA QUAN EL GAMEMANAGER SELECCIONA UN TARGET
    private void TargetSeleccionat(GameObject target)
    {
        GameManagerArena.Instance.OnSeleccionarTarget -= TargetSeleccionat;
        this.target = target;
        //guardar target   
        ChangeState(CombatStates.ACTION_MAGIC);
    }

}
