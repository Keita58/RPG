using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    //[SerializeField] PlayerSO playerso;

    [SerializeField] InputActionAsset actionAsset;
    InputActionAsset inputAction;
    InputAction moviment;

    
    [SerializeField] AnimationClip attackClip;
    [SerializeField] AnimationClip hurtClip;
    [SerializeField] public PlayerSO player;
    public event Action ActivaCamera;

    int hp;
    int lvl;

    [SerializeField] AudioManager audios;

    //LISTA ESTADOS ALTERADOS SUFRIDOS
    //LISTA DE ATAQUES
    //LISTA DE OBJETOS

    //hacer una clase estado alterado, etc.

    private void Awake()
    {
        this.hp = player.Hp;
        this.lvl=player.Lvl;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputAction = Instantiate(actionAsset);
        inputAction.FindActionMap("Player").FindAction("Attack").performed += OnAttack;
        moviment = inputAction.FindActionMap("Player").FindAction("Move");
        inputAction.FindActionMap("Player").Enable();
        ActivaCamera?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHitbox")
        {
            ChangeState(PlayerStates.RECEIVEDAMAGE);
            //Que empiece con ventaja el enemigo y cambiar de escena y todo eso.
            //GAMEMANAGER.getINSTANCE().addd(this)
        }
    }

    enum PlayerStates { IDLE, MOVE, ATTACK, RECEIVEDAMAGE }
    [SerializeField] PlayerStates actualState;
    [SerializeField] float stateTime;

    private void Start()
    {
        ChangeState(PlayerStates.IDLE);
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
                rb.velocity = Vector2.zero;
                animator.Play("Idle");
                break;
            case PlayerStates.MOVE:
                animator.Play("Move");
                break;
            case PlayerStates.ATTACK:
                animator.Play("Attack");
                audios.AtacJugador();
                break;
            case PlayerStates.RECEIVEDAMAGE:
                animator.Play("Hurt");
                break;
            default:
                break;
        }
    }
    private void UpdateState()
    {
        Vector2 direccio = moviment.ReadValue<Vector2>();

        stateTime += Time.deltaTime;

        switch (actualState)
        {
            case PlayerStates.IDLE:
                if (direccio != Vector2.zero)
                    ChangeState(PlayerStates.MOVE);
                break;
            case PlayerStates.MOVE:
                if (direccio == Vector2.zero)
                {
                    ChangeState(PlayerStates.IDLE);
                    rb.velocity = Vector2.zero;
                    break;
                }
                this.rb.velocity = direccio * 4f;

                transform.eulerAngles = Vector3.up * (direccio.x > 0 ? 0 : 180);
                break;
            case PlayerStates.ATTACK:
                if (stateTime >= attackClip.length)
                    ChangeState(PlayerStates.IDLE);
                break;
            case PlayerStates.RECEIVEDAMAGE:
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
            case PlayerStates.MOVE:
                if(rb != null)
                    rb.velocity = Vector2.zero;
                break;
            case PlayerStates.ATTACK:
                break;
            case PlayerStates.RECEIVEDAMAGE:
                break;
            default:
                break;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        switch (actualState)
        {
            case PlayerStates.IDLE:
                ChangeState(PlayerStates.ATTACK);
                break;
            case PlayerStates.MOVE:
                ChangeState(PlayerStates.ATTACK);
                break;
            case PlayerStates.ATTACK:
                break;
            default:
                break;
        }
    }
    void Update()
    {
        UpdateState();
    }
}
