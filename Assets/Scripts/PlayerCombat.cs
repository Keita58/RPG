using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour, Tornable
{
    PlayerSO playerBase;

    List<AtacSO> atacs;
    public event Action<AtacSO> onAttack;
    Animator animator;
    [SerializeField] AnimationClip atacClip;
    [SerializeField] AnimationClip hurtClip;

    enum PlayerStates { IDLE, HURT, ATTACK}
    [SerializeField] PlayerStates actualState;
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
    public event Action onIniciarTurnoUI;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.hp = playerBase.Hp;
        this.lvl = playerBase.Lvl;
        this.mana = playerBase.Mana;
        this.def = playerBase.Def;
        this.damageAtk = playerBase.DamageAtk;
        this.spd=playerBase.spd;
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
        playerBase.spd = this.spd;
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
        //AvisarUIMOSTRAR BOTON
        onIniciarTurnoUI.Invoke();

        //Si el enemigo empieza con ventaja. Incapacitat sempre serà true en aquest cas.
        if (estado != null && estado.Nom=="Ventaja" && estado.Torns>0)
        {
            AcabarTorn();
            estado.Torns--;
        }
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
            //GameManagerArena.Instance.BucleJoc();

        }
       
    }

    private void ChangeState(PlayerStates newstate)
    {
        ExitState(actualState);
        IniState(newstate);
    }
    private void IniState(PlayerStates initState)
    {
        actualState = initState;
        stateTime = 0f;

        switch (actualState)
        {
            case PlayerStates.IDLE:
                animator.Play("Idle");
                break;
            case PlayerStates.ATTACK:
                animator.Play("atac2");
                break;
            case PlayerStates.HURT:
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
            case PlayerStates.IDLE:
                break;
            case PlayerStates.ATTACK:
                if (stateTime >= atacClip.length)
                    ChangeState(PlayerStates.IDLE);
                break;
            case PlayerStates.HURT:
                if (stateTime >= hurtClip.length)
                    ChangeState(PlayerStates.IDLE);
                break;
        }
    }

    private void ExitState(PlayerStates exitState)
    {
        switch (exitState)
        {
            case PlayerStates.IDLE:
                break;
            case PlayerStates.ATTACK:
                break;
            case PlayerStates.HURT:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        IniState(PlayerStates.IDLE);
    }

    private void Update()
    {
        UpdateState();
    }


    //hacer una funcion que se suscriba que sea rebre mal;

}
