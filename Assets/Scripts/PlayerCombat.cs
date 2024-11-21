using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour, Tornable
{
    [SerializeField] PlayerSO playerBase;

    List<AtacSO> atacs;
    public event Action<AtacSO> onAttack;
    Animator animator;
    [SerializeField] AnimationClip atacClip;
    [SerializeField] AnimationClip hurtClip;

    enum CombatStates { WAITING, SELECT_ACTION, ACTION_ATTACK, ACTION_MAGIC, ACTION_OBJECTS, ACTION_RUN }
    [SerializeField] CombatStates combatState;
    enum PlayerAnimations { IDLE, HURT, ATTACK}
    [SerializeField] PlayerAnimations actualState;
    [SerializeField] float stateTime;
    int hp;
    int lvl;
    int mana;
    int def;
    int damageAtk;
    int spd;
    EstadosAlterados estado;
    [SerializeField] AtacSO[] atacsBase;
    public event Action onMuerto;
    //Accions GUI
    public event Action OnMostrarAccions;
    public event Action OnOcultarAccions;
    public event Action<List<AtacSO>> OnMostrarMagia;
    public event Action OnOcultarMagia;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.hp = playerBase.Hp;
        this.lvl = playerBase.Lvl;
        this.mana = playerBase.Mana;
        this.def = playerBase.Def;
        this.damageAtk = playerBase.DamageAtk;
        this.spd=playerBase.Spd;

        StartCoroutine(EsperarIActuar(1, IniciarTorn));
    }

    public void RebreMal(AtacSO atac)
    {
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

    //public void atacar1()
    //{
    //    onAttack.Invoke(atacs.ElementAt(0));
    //    ChangeState(PlayerStates.ATTACK);
    //    this.mana-=atacs.ElementAt(0).mana;
        
    //}

    //public void atacar2()
    //{
    //    onAttack.Invoke(atacs.ElementAt(1));
    //    ChangeState(PlayerStates.ATTACK);
    //    this.mana -= atacs.ElementAt(1).mana;
    //}

    //public void atacar3()
    //{
    //    onAttack.Invoke(atacs.ElementAt(2));
    //    ChangeState(PlayerStates.ATTACK);
    //    this.mana -= atacs.ElementAt(2).mana;
    //}

    //public void atacar4()
    //{
    //    onAttack.Invoke(atacs.ElementAt(3));
    //    ChangeState(PlayerStates.ATTACK);
    //    this.mana -= atacs.ElementAt(3).mana;
    //}


    public void IniciarTorn()
    {
        Assert.AreEqual(combatState, CombatStates.WAITING, $"{gameObject}: Iniciant torn quan no s'est� esperant.");
        ChangeState( CombatStates.SELECT_ACTION );
    }

    public void AcabarTorn()
    {
        if (this.hp <= 0)
        {
            //INVOKE GAME MANAGER CAMBIAR DE ESCENA
            onMuerto.Invoke();
        }
        else
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
        switch(combatState)
        {
            case CombatStates.WAITING:
                //GameManagerArena.Instance.BucleJoc();
                Debug.Log("He acabat el torn");
                StartCoroutine(EsperarIActuar(3, () => ChangeState(CombatStates.SELECT_ACTION)) );
                break;
            case CombatStates.SELECT_ACTION:
                //Si el enemigo empieza con ventaja. Incapacitat sempre ser� true en aquest cas.
                if (estado != null && estado.Nom == "Ventaja" && estado.Torns > 0)
                {
                    estado.Torns--; 
                    //TODO: Mirar qu� passa
                    ChangeState( CombatStates.WAITING );
                    break;
                }
                else
                {
                    OnMostrarAccions?.Invoke();

                }
                //AvisarUIMOSTRAR BOTON
                break;
            case CombatStates.ACTION_ATTACK:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
            case CombatStates.ACTION_MAGIC:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                OnMostrarMagia.Invoke(atacs);
                break;
            case CombatStates.ACTION_OBJECTS:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
            case CombatStates.ACTION_RUN:
                StartCoroutine(EsperarIActuar(1, () => ChangeState(CombatStates.WAITING)));
                break;
        }
    }

    private void ExitState(CombatStates currentState)
    {
        Assert.AreEqual(combatState, currentState, $"{gameObject}: Est�s cridant un sortir d'estat quan no est�s a aquest estat");
        switch(currentState)
        {
            case CombatStates.WAITING:
                break;
            case CombatStates.SELECT_ACTION:
                OnOcultarAccions?.Invoke();
                break;
            case CombatStates.ACTION_ATTACK:
                break;
            case CombatStates.ACTION_MAGIC:
                OnOcultarMagia?.Invoke();
                break;
            case CombatStates.ACTION_OBJECTS:
                break;
            case CombatStates.ACTION_RUN:
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
                    ChangeState(PlayerAnimations.IDLE);
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
    }

    private void Update()
    {
        UpdateState();
    }

    //Accions Menu
    internal void AccioAtacar()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� atack quan no s'est� esperant una selecci�.");
        ChangeState(CombatStates.ACTION_ATTACK);
    }

    internal void AccioMagia()
    {
        Assert.AreEqual(combatState, CombatStates.SELECT_ACTION, $"{gameObject}: seleccio d'acci� magia quan no s'est� esperant una selecci�.");
        ChangeState(CombatStates.ACTION_MAGIC);
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


    //hacer una funcion que se suscriba que sea rebre mal;

}
