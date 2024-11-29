using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour, Tornable
{
    [SerializeField] PlayerSO playerBase;

    [SerializeField]List<AtacSO> atacs;
    public event Action<AtacSO> onAttack;
    Animator animator;
    [SerializeField] AnimationClip atacClip;
    [SerializeField] AnimationClip hurtClip;
    [SerializeField] TextMeshProUGUI textoTarget;

    [SerializeField] AtacSO ataqueBasico;

    enum CombatStates { WAITING, SELECT_ACTION, SELECT_MAGIC, ACTION_MAGIC, SELECT_OBJECT, ACTION_OBJECTS, ACTION_RUN , SELECCIONAR_TARGET}
    [SerializeField] CombatStates combatState;
    enum PlayerAnimations { IDLE, HURT, ATTACK }
    [SerializeField] PlayerAnimations actualState;
    [SerializeField] float stateTime;
    int hp;
    int lvl;
    int mana;
    int def;
    int damageAtk;
    int spd;
    public bool entroSeleccionado { get; private set; }
    EstadosAlterados estado;
    [SerializeField] List<AtacSO> atacsBase;
    AtacSO atacSeleccionat;
    GameObject target;
    public event Action<string> onMuerto;
    //Accions GUI
    public event Action OnMostrarAccions;
    public event Action OnOcultarAccions;
    public event Action<List<AtacSO>> OnMostrarMagia;
    public event Action OnOcultarMagia;
    public event Action OnDeshabilitarAccions;
    public event Action OnEntrarSeleccionarTarget;
    public event Action OnFugir;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        IniState(PlayerAnimations.IDLE);
        Iniciar(playerBase);
    }

    public void Iniciar(PlayerSO player)
    {
        this.hp = playerBase.Hp;
        this.lvl = playerBase.Lvl;
        this.mana = playerBase.Mana;
        this.def = playerBase.Def;
        this.damageAtk = playerBase.DamageAtk;
        this.spd = playerBase.Spd;
    }

    public void RebreMal(AtacSO atac)
    {
        Debug.Log($"VIDA ABANS ATAC{this.hp}");

        ChangeState(PlayerAnimations.HURT);

        if (atac.mal > def)
        {
            int hprestat = atac.mal - def;
            hp -= hprestat;
            Debug.Log($"VIDA DESPRÉS ATAC{this.hp}");
        }

        if (atac.estat != null && estado==null)
        {
            estado.IniciarEstadoAlterado(atac.estat);
        }
        if (this.hp <= 0)
        {
            //INVOKE GAME MANAGER CAMBIAR DE ESCENA
            onMuerto?.Invoke("Overworld");
        }
    }

    public void lvlUP()
    {
        lvl++;
        hp += 10;
        mana += 10;
        def += 2;
        damageAtk += 1;
        if (lvl == 2)
        {
            atacs.Add(atacsBase[0]);
        }
        else if (lvl == 5)
        {
            atacs.Add(atacsBase[1]);
        }
        else if (lvl == 10)
        {
            atacs.Add(atacsBase[2]);
        }
        else if (lvl == 15)
        {
            atacs.Add(atacsBase[3]);
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
    }


    public void IniciarTorn()
    {
        Assert.AreEqual(combatState, CombatStates.WAITING, $"{gameObject}: Iniciant torn quan no s'est� esperant.");
        ChangeState(CombatStates.SELECT_ACTION);
    }

    public void AcabarTorn()
    {
        ProcessarEstat();
        GameManagerArena.Instance.BucleJoc();
        Debug.Log("He acabat el torn");
    }

    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        yield return new WaitForSeconds(tempsDespera);
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
                    estado.Torns--;
                    //TODO: Mirar qu� passa
                    ChangeState(CombatStates.WAITING);
                    break;
                }
                else
                {
                    OnMostrarAccions?.Invoke();
                }
                break;
      
            case CombatStates.SELECT_MAGIC:
                OnMostrarMagia?.Invoke(atacsBase);
                OnDeshabilitarAccions?.Invoke();
                break;
            case CombatStates.ACTION_MAGIC:
                ChangeState(PlayerAnimations.ATTACK);
                this.mana -= atacSeleccionat.mana;
                target.GetComponent<EnemyArena>().RebreMal(atacSeleccionat);
                break;
            case CombatStates.ACTION_OBJECTS:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
            case CombatStates.ACTION_RUN:
                //AVISAR AL GAMEMANAGER PARA CANVIAR DE ESCENA.
                StartCoroutine(EsperarIActuar(0, () => OnFugir?.Invoke()));
                ChangeState(CombatStates.WAITING);
                break;
            case CombatStates.SELECCIONAR_TARGET:
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
            case CombatStates.ACTION_OBJECTS:
                StartCoroutine(EsperarIActuar(0, () => AcabarTorn()));
                break;
            case CombatStates.SELECCIONAR_TARGET:
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

    internal void AccioObjecte()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� objecte quan no s'est� esperant una selecci�.");
        ChangeState(CombatStates.ACTION_OBJECTS);
    }

    internal void AccioFugir()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� fugir quan no s'est� esperant una selecci�.");
        ChangeState(CombatStates.ACTION_RUN);
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
