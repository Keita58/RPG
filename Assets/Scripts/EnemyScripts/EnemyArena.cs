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
    [SerializeField] private GameObject _Jugador;

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
        if (estadosAlterados != null && estadosAlterados.Incapacitat && estadosAlterados.Torns > 0)
        {
            estadosAlterados.Torns--;
            GameManagerArena.Instance.BucleJoc();
            //TODO: Mirar qu� passa
        }
        bool sortir = false;
        AtacSO at = null;
        while (!sortir)
        {
            at = atk[UnityEngine.Random.Range(0, atk.Length)];
            if (at.mana < this.mana)
            {
                this.mana -= at.mana;
                sortir = true;
            }
            else
            {
                break;
            }
        }
        this.escollit = at;
<<<<<<< HEAD
        _Jugador.GetComponent<PlayerCombat>().RebreMal(this.escollit);
=======
        atacar.Invoke(at);
        ProcessarEstadoAlterado(at);
>>>>>>> origin/hector
        //HACER AQUI LO DE LOS ESTADOS ALTERADOS?
    }

    public void ProcessarEstadoAlterado(AtacSO at)
    {
        if (estadosAlterados == null)
        {
            this.hp -= estadosAlterados.Hp;
            this.def += estadosAlterados.ModDef;
            this.spd += estadosAlterados.ModSpd;
            at.mal += estadosAlterados.ModAtk;
            estadosAlterados.Torns--;
            if (estadosAlterados.Torns <= 0)
            {
                Debug.Log($"L'estat {estadosAlterados.Nom} ha finalitzat");
                estadosAlterados = null;
            }
        }
    }

    public void RebreMal(AtacSO atac)
    {
        if (atac.mal > this.def)
        {
            StartCoroutine(AnimacioMal());
            Debug.Log("Vida abans mal: " + this.hp);
            this.hp -= atac.mal - this.def;
            Debug.Log("Vida despr�s mal: " + this.hp);
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
        if (!this.selected && _Jugador.GetComponent<PlayerCombat>().entroSeleccionado)
        {   
            this.selected = true;
            Seleccionat.SetActive(true);
        }
        GameManagerArena.Instance.CanviaEnemicSelected(gameObject);
    }
}
