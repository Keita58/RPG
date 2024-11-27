using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


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
    public event Action onMuerto;
    //Accions GUI
    public event Action OnMostrarAccions;
    public event Action OnOcultarAccions;
    public event Action<List<AtacSO>> OnMostrarMagia;
    public event Action OnOcultarMagia;
    public event Action OnDeshabilitarAccions;
    public event Action OnEntrarSeleccionarTarget;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        //StartCoroutine(EsperarIActuar(1, IniciarTorn));
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
        if (this.hp <= 0)
        {
            //INVOKE GAME MANAGER CAMBIAR DE ESCENA
            onMuerto.Invoke();
        }

        if (atac.mal > def)
        {
            int hprestat = atac.mal - def;
            hp -= hprestat;
        }

        if (atac.estat != null)
        {
            estado.IniciarEstadoAlterado(atac.estat);
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
        Assert.AreEqual(combatState, CombatStates.WAITING, $"{gameObject}: Iniciant torn quan no s'està esperant.");
        ChangeState(CombatStates.SELECT_ACTION);
    }

    public void AcabarTorn()
    {
        if (estado != null)
        {
            if (estado.Nom == "Veneno" && estado.Torns > 0)
            {
                this.hp -= estado.Hp;
                estado.Torns--;
            }
        }
        GameManagerArena.Instance.BucleJoc();
    }

    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        yield return new WaitForSeconds(tempsDespera);
        accio();
    }

    //FSM COMBAT
    private void ChangeState(CombatStates newstate)
    {
        ExitState(combatState);
        InitState(newstate);
    }

    private void InitState(CombatStates newstate)
    {
        combatState = newstate;
        switch (combatState)
        {
            case CombatStates.WAITING:
                //GameManagerArena.Instance.BucleJoc();
                Debug.Log("He acabat el torn");
                OnOcultarAccions?.Invoke();
                break;
            case CombatStates.SELECT_ACTION:
                //Si el enemigo empieza con ventaja. Incapacitat sempre serà true en aquest cas.
                if (estado != null && estado.Nom == "Ventaja" && estado.Torns > 0)
                {
                    estado.Torns--;
                    //TODO: Mirar què passa
                    ChangeState(CombatStates.WAITING);
                    break;
                }
                else
                {
                    OnMostrarAccions?.Invoke();
                }
                //AvisarUIMOSTRAR BOTON
                break;
      
                //StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                //List<GameObject> li = GameManagerArena.Instance.getEnemics();
                //foreach (GameObject go in li)
                //{
                //    Debug.Log("ENEMIC: "+ go.GetComponent<EnemyArena>().name);
                //    Debug.Log("ENEMIC SELECCIONAT?: " + go.GetComponent<EnemyArena>().selected);
                //    if (go.GetComponent<EnemyArena>().selected)
                //    {
                //        ChangeState(PlayerAnimations.ATTACK);
                //        Debug.Log("VIDA ANTES DEL ATAQUE: " + go.GetComponent<EnemyArena>().hp);
                //        go.GetComponent<EnemyArena>().RebreMal(ataqueBasico);
                //        Debug.Log("ATACO A"+go.name);
                //        Debug.Log("VIDA DESPUES DEL ATAQUE: " + go.GetComponent<EnemyArena>().hp);
                //    }
                //    else
                //    {
                //        textoTarget.text = "HAS DE SELECCIONAR UN ENEMIC";
                //        ChangeState(CombatStates.SELECT_ACTION);
                //    }
                //}
            case CombatStates.SELECT_MAGIC:
                OnMostrarMagia?.Invoke(atacsBase);
                OnDeshabilitarAccions?.Invoke();
                break;
            case CombatStates.ACTION_MAGIC:
                ChangeState(PlayerAnimations.ATTACK);
                this.mana -= atacSeleccionat.mana;
                target.GetComponent<EnemyArena>().RebreMal(atacSeleccionat);
                StartCoroutine(EsperarIActuar(1, () => AtacAcabat()));
                break;
            case CombatStates.ACTION_OBJECTS:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
            case CombatStates.ACTION_RUN:
                //AVISAR AL GAMEMANAGER PARA CANVIAR DE ESCENA.
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
            case CombatStates.SELECCIONAR_TARGET:
                entroSeleccionado = true;
                GameManagerArena.Instance.OnSeleccionarTarget += TargetSeleccionat;
                OnDeshabilitarAccions.Invoke();
                break;
        }
    }

    private void AtacAcabat()
    {
        ChangeState(CombatStates.WAITING);
    }

    private void ExitState(CombatStates currentState)
    {
        Assert.AreEqual(combatState, currentState, $"{gameObject}: Estàs cridant un sortir d'estat quan no estàs a aquest estat");
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
                AcabarTorn();
                break;
            case CombatStates.SELECCIONAR_TARGET:
                entroSeleccionado=false;
                break;

     
        }
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
                break;
            case PlayerAnimations.HURT:
                animator.Play("Hurt");
                break;
            default:
                break;
        }
    }

    private void UpdateState()
    {
        stateTime += Time.deltaTime;

        switch (actualState)
        {
            case PlayerAnimations.IDLE:
                break;
            case PlayerAnimations.ATTACK:
                if (stateTime >= atacClip.length)
                {
                    ChangeState(PlayerAnimations.IDLE);
                    ChangeState(CombatStates.WAITING);
                }
                break;
            case PlayerAnimations.HURT:
                if (stateTime >= hurtClip.length)
                    ChangeState(PlayerAnimations.IDLE);
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

    private void Start()
    {
        IniState(PlayerAnimations.IDLE);
        Iniciar(playerBase);
    }

    private void Update()
    {
        UpdateState();
    }

    //Accions Menu
    internal void AccioAtacar()
    {

        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acció atack quan no s'està esperant una selecció.");
        this.atacSeleccionat = ataqueBasico;
        ChangeState(CombatStates.SELECCIONAR_TARGET);
       
    }

    internal void AccioSeleccionarMagia()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acció magia quan no s'està esperant una selecció.");
        ChangeState(CombatStates.SELECT_MAGIC);
    } 

    internal void AccioObjecte()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acció objecte quan no s'està esperant una selecció.");
        ChangeState(CombatStates.ACTION_OBJECTS);
    }

    internal void AccioFugir()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acció fugir quan no s'està esperant una selecció.");
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

    private void TargetSeleccionat(GameObject target)
    {
        GameManagerArena.Instance.OnSeleccionarTarget -= TargetSeleccionat;
        this.target = target;
        //guardar target   
        ChangeState(CombatStates.ACTION_MAGIC);
    }

}
