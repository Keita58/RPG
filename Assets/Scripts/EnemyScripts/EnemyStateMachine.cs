
using static UnityEngine.Rendering.DebugUI;
using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
public class EnemyStateMachine : MonoBehaviour
{
    //[SerializeField] public EnemySO _enemySO;
    private Animator _animator;
    //[SerializeField] private Hitbox hitbox;
    //[SerializeField] private RangeDetection _rangPerseguir;
    //[SerializeField] private RangeDetection _rangAtac;
    [SerializeField] private UnityEngine.UI.Slider slider;

    void OnEnable()
    {
        //this.slider.maxValue = this._enemySO.hp;
        //this.slider.value = _enemySO.hp;
       /* this.cooldown = false;
        this.ChangeState(SkeletonStates.IDLE);
        this.GetComponent<SpriteRenderer>().color = _enemySO.color;
        _animator = GetComponent<Animator>();
        this._hp = _enemySO.hp;
        //this._rangAtac.GetComponent<CircleCollider2D>().radius = this._enemySO.rangeAttack;
        //_rangPerseguir.OnEnter += PerseguirDetected;
        //_rangPerseguir.OnStay += PerseguirDetected;
        //_rangPerseguir.OnExit += PerseguirUndetected;
        //_rangAtac.OnEnter += AtacarDetected;
        //_rangAtac.OnStay += AtacarDetected;
        //_rangAtac.OnExit += AtacarUndetected;
        StartCoroutine(patrullar());*/
       StartCoroutine(mirarPersonaje());
    }
    private void Awake()
    {
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

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.right);
            hit.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y) + (hit.distance * Vector2.right), Color.red, 5);
            RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 45) * Vector2.right));
            hit2.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit2.distance * (Quaternion.Euler(0, 0, 45) * Vector2.right)), Color.red, 5);
            RaycastHit2D hit3 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 45) * Vector2.right));
            hit3.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit3.distance * (Quaternion.Euler(0, 0, 22) * Vector2.right)), Color.red, 5);
            RaycastHit2D hit4 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 45) * Vector2.down));
            hit4.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit4.distance * (Quaternion.Euler(0, 0, 42) * Vector2.down)), Color.red, 5);
            RaycastHit2D hit5 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 45) * Vector2.down));
            hit5.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit5.distance * (Quaternion.Euler(0, 0, 55) * Vector2.down)), Color.red, 5);
            RaycastHit2D hit6 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 75) * Vector2.down));
            hit6.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit6.distance * (Quaternion.Euler(0, 0, 75) * Vector2.down)), Color.red, 5);
            RaycastHit2D hit7 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 55) * Vector2.right));
            hit7.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit7.distance * (Quaternion.Euler(0, 0, 35) * Vector2.right)), Color.red, 5);
            RaycastHit2D hit8 = Physics2D.Raycast(this.transform.position, (Quaternion.Euler(0, 0, 55) * Vector2.right));
            hit8.distance = 5;
            Debug.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + (hit8.distance * (Quaternion.Euler(0, 0, 10) * Vector2.right)), Color.red, 5);
            yield return new WaitForSeconds(1);

        }
    }
    /*IEnumerator mirarPersonaje()
    {
        while (true)
        {

            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 45);
            yield return new WaitForSeconds(1);

        }
    }*/
 /*   private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(this.transform.position.x+1,this.transform.position.y, this.transform.position.z), 1);

    }*/
    private void Start()
    {
       // ChangeState(SkeletonStates.IDLE);
    }
    /*
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
                _animator.Play("RedSkeletonIdle");
                break;
            case SkeletonStates.MOVE:
                _animator.Play("RedSkeletonMove");
                break;

            case SkeletonStates.ATTACK:
                _animator.Play("RedSkeletonAttack");
                hitbox.Damage = _enemySO.dmg;
                break;
            case SkeletonStates.ATTACK2:
                _animator.Play("RedSkeletonAttack2");
                hitbox.Damage = _enemySO.dmg2;
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
    private void LateUpdate()
    {
        this.slider.transform.eulerAngles = Vector3.zero;
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
    IEnumerator DamagedColor()
    {
        this.GetComponent<SpriteRenderer>().color = Color.grey;
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = _enemySO.color;
    }
    public void ReceiveDamage(float damage)
    {
        this._hp -= damage;
        this.slider.value = _hp;
        StartCoroutine(DamagedColor());
        if (this._hp <= 0)
        {
            ronda.enemicsActuals--;
            this.gameObject.SetActive(false);
            _rangPerseguir.OnEnter -= PerseguirDetected;
            _rangPerseguir.OnStay -= PerseguirDetected;
            _rangPerseguir.OnExit -= PerseguirUndetected;
            _rangAtac.OnEnter -= AtacarDetected;
            _rangAtac.OnStay -= AtacarDetected;
            _rangAtac.OnExit -= AtacarUndetected;
        }
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
            if (this._enemySO.color == Color.white || this._enemySO.color == Color.red)
                ChangeState(SkeletonStates.ATTACK);
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
    }
    IEnumerator cooldownFalse()
    {
        yield return new WaitForSeconds(2f);
        cooldown = false;
    }
    private void spawnKife()
    {
        for (int x = 0; x < knifes.Length; x++)
        {
            if (!knifes[x].gameObject.activeSelf)
            {
                knifes[x].Damage = (int)(_enemySO.dmg2);
                knifes[x].gameObject.transform.position = this.transform.position;
                if (knifes[x] != null)
                    knifes[x].gameObject.SetActive(true);
                break;
            }
        }
    }
    private void AtacarUndetected(GameObject personatge)
    {

    }
}*/


}