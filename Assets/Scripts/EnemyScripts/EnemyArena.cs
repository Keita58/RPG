using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class EnemyArena : MonoBehaviour, IAttack, IDamageable, IPointerDownHandler
{
    [SerializeField] HealthBar vidaPantalla;
    public GameObject Seleccionat;
    private AtacSO escollit;
    private Animator animator;
    private EnemySO EnemySO;
    public int id { get; private set; }
    public bool selected { get; set; }
    public int hp { get; private set; }
    public AtacSO[] atk { get; private set; }
    public int def { get; private set; }
    public int spd { get; private set; }
    public int mana { get; private set; }
    public event Action<AtacSO> onDamaged;
    public event Action<AtacSO> atacar;
    EstadosAlterados estadosAlterados;
    GameObject _Jugador;

    public AtacSO atac { set => value = escollit; }

    private void Awake()
    {
        _Jugador = GameManagerArena.Instance.getJugador();
        animator = GetComponent<Animator>();
    }

    public void Iniciar(EnemySO enemic)
    {
        Assert.IsNull(EnemySO, $"Ja hi ha un EnemicSO al {gameObject}");
        this.EnemySO = enemic;
        this.hp = this.EnemySO.hp;
        this.atk = this.EnemySO.atk;
        this.def = this.EnemySO.def;
        this.spd = this.EnemySO.spd;
        this.mana = this.EnemySO.mana;
        this.animator.runtimeAnimatorController = this.EnemySO.animator;
        vidaPantalla.IniciarBarra(this.hp);
    }

    public void EscollirAtac()
    {
        bool sortir = false;
        AtacSO at = null;
        while (!sortir)
        {
            at = atk[UnityEngine.Random.Range(0, atk.Length)];
            if (at.mana < this.mana)
            {
                this.mana-=at.mana;
                sortir = true;
            }
            sortir = true;
        }
        this.escollit = at;
        atacar.Invoke(at);
    }

    public void RebreMal(AtacSO atac)
    {
        if (atac.mal > this.def)
        {
            StartCoroutine(AnimacioMal());
            Debug.Log("Vida abans mal: " + this.hp);
            this.hp -= atac.mal - this.def;
            Debug.Log("Vida després mal: " + this.hp);
            if (atac.estat != null)
                this.estadosAlterados.IniciarEstadoAlterado(atac.estat);
            vidaPantalla.UpdateHealth(atac.mal);
        }
        if (this.hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator AnimacioMal()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(1);
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnDestroy()
    {
        //GameManager.instance.ActivarAtac -= EscollirAtac();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!this.selected)
        {   
            this.selected = true;
            Seleccionat.SetActive(true);
        }
        GameManagerArena.Instance.CanviaEnemicSelected(gameObject);
    }
}
