using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class EnemyStateMachine : MonoBehaviour
{
    public LayerMask ly;
    [SerializeField] public EnemySO _enemySO;
    private Animator _animator;

    [SerializeField] AudioManager audios;

    void OnEnable()
    {
        this.ChangeState(SkeletonStates.IDLE);
        this.cooldown = false;
    }
    private void Awake()
    {
        this._hp = _enemySO.hp;
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _enemySO.animator;
        StartCoroutine(mirarPersonaje());
    }

    private enum SkeletonStates { NULL, IDLE, ATTACK, ATTACK2, MOVE, COMBO12, COMBO21 }
    [SerializeField] private SkeletonStates _CurrentState;
    [SerializeField] private float _StateTime;
    [SerializeField] private float _hp;

    public event Action<float> OnDamaged;

    IEnumerator mirarPersonaje()
    {
        while (true)
        {
            RaycastHit2D a = Physics2D.BoxCast(this.transform.position + transform.right * 2.5f, new Vector2(5, 5), 0, Vector2.right, 5, ly);
            if (a.collider != null)
            {
                if (this.transform.eulerAngles == Vector3.zero)
                {
                    if (a.point.x < a.centroid.x - 1.5f && (a.point.y >= a.centroid.y - 0.50 && a.point.y <= a.centroid.y + 0.50))
                    {
                        ChangeState(SkeletonStates.ATTACK);
                    }
                    else
                    {
                        this.GetComponent<Rigidbody2D>().velocity = (a.collider.gameObject.transform.position - this.transform.position).normalized;
                    }
                }
                else if (this.transform.eulerAngles != Vector3.zero)
                {
                    if (a.point.x > a.centroid.x + 1.5f && (a.point.y >= a.centroid.y - 0.50 && a.point.y <= a.centroid.y + 0.50))
                    {
                        ChangeState(SkeletonStates.ATTACK);
                    }
                    else
                    {
                        this.GetComponent<Rigidbody2D>().velocity = (a.collider.gameObject.transform.position - this.transform.position).normalized;
                    }
                }

            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ChangeState(SkeletonStates newState)
    {
        //tornar al mateix estat o no
        if (newState == _CurrentState)
            return;

        ExitState(_CurrentState);
        InitState(newState);
    }

    private void InitState(SkeletonStates initState)
    {
        _CurrentState = initState;
        _StateTime = 0f;

        switch (_CurrentState)
        {
            case SkeletonStates.IDLE:
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _animator.Play(_enemySO.clipIdle.name);
                break;
            case SkeletonStates.MOVE:
                _animator.Play(_enemySO.clipMove.name);
                break;

            case SkeletonStates.ATTACK:
                _animator.Play(_enemySO.clipAttack.name);
                switch (_enemySO.clipAttack.name)
                {
                    case "EvilWizardAttack":
                        audios.AtacMac();
                        break;
                    case "EvilKnightAttack":
                        audios.AtacEspadatxi();
                        break;
                    case "OrcAttack":
                        audios.AtacGoblin();
                        break;
                }
                break;
            case SkeletonStates.ATTACK2:
                _animator.Play(_enemySO.clipAttack2.name);
                break;
            default:
                break;
        }
    }

    private void UpdateState(SkeletonStates updateState)
    {
        Vector2 dir = this.GetComponent<Rigidbody2D>().velocity;
        _StateTime += Time.deltaTime;

        switch (updateState)
        {
            case SkeletonStates.IDLE:
                if (dir != Vector2.zero)
                    ChangeState(SkeletonStates.MOVE);
                break;

            case SkeletonStates.MOVE:
                if (dir == Vector2.zero)
                {
                    ChangeState(SkeletonStates.IDLE);
                    break;
                }

                if (dir.x > 0)
                {
                    this.transform.eulerAngles = Vector3.zero;
                }
                else if (dir.x < 0)
                {
                    this.transform.eulerAngles = Vector3.up * 180;
                }

                this.GetComponent<Rigidbody2D>().velocity = dir * 1;
                break;


            case SkeletonStates.ATTACK:
                if (_StateTime >= _enemySO.clipAttack.length)
                    ChangeState(SkeletonStates.IDLE);
                break;
            case SkeletonStates.ATTACK2:
                if (_StateTime >= _enemySO.clipAttack2.length)
                    ChangeState(SkeletonStates.IDLE);
                break;
            default:
                break;
        }
    }

    private void ExitState(SkeletonStates exitState)
    {
        switch (exitState)
        {
            case SkeletonStates.MOVE:
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                break;

            case SkeletonStates.ATTACK:
            case SkeletonStates.ATTACK2:
                break;

            default:
                break;
        }
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        switch (_CurrentState)
        {
            case SkeletonStates.IDLE:
            case SkeletonStates.MOVE:
                ChangeState(SkeletonStates.ATTACK);
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        UpdateState(_CurrentState);
    }

    private void PerseguirDetected(GameObject personatge)
    {
        if (personatge.name == "PJ" && _enemySO.clipAttack.length <= _StateTime)
        {
            detected = true;
            this.GetComponent<Rigidbody2D>().velocity = (personatge.gameObject.transform.position - this.transform.position).normalized;
        }
    }

    bool detected = false;
    IEnumerator patrullar()
    {
        while (!detected && this.isActiveAndEnabled)
        {
            if (this.transform.position.x < 0)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
                yield return new WaitForSeconds(3);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
                yield return new WaitForSeconds(3);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
            }
        }
    }

    private void PerseguirUndetected(GameObject personatge)
    {
        detected = false;
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(patrullar());
        }
    }

    public bool cooldown = false;
    private void AtacarDetected(GameObject personatge)
    {
        if (personatge.name == "PJ")
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ChangeState(SkeletonStates.ATTACK);
        }
        else
        {
            if (!cooldown)
            {
                cooldown = true;
                ChangeState(SkeletonStates.ATTACK2);
                StartCoroutine(cooldownFalse());
            }
        }
    }

    IEnumerator cooldownFalse()
    {
        yield return new WaitForSeconds(2f);
        cooldown = false;
    }
}
