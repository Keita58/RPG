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
        if (EnemySO.EstadosAlterados != null && EnemySO.EstadosAlterados.incapacitat)
        {
            Debug.Log($"{gameObject}/{this}: INICIA VENTAJA ESTADO ALTERADO: {EnemySO.EstadosAlterados.nom}");
            this.estadosAlterados = new EstadosAlterados(EnemySO.EstadosAlterados.nom, EnemySO.EstadosAlterados.incapacitat, EnemySO.EstadosAlterados.torns, EnemySO.EstadosAlterados.hp, EnemySO.EstadosAlterados.modAtk, EnemySO.EstadosAlterados.modDef, EnemySO.EstadosAlterados.modSpd);
            estadosAlterados.Torns--;
            EnemySO.EstadosAlterados = null;
            GameManagerArena.Instance.BucleJoc();
            //TODO: Mirar qu� passa
        }
        bool sortir = false;
        AtacSO at = null;
        while (!sortir)
        {
            print("Sigo dentro");
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
        print("He salido, no teneis razon");
        this.animator.Play(EnemySO.clipAttack.name);

        StartCoroutine(EsperarIActuar(EnemySO.clipAttack.length,
            () =>
            {
                this.animator.Play(EnemySO.clipIdle.name);
                this.escollit = at;
                _Jugador.GetComponent<PlayerCombat>().RebreMal(this.escollit);
                ProcessarEstadoAlterado(at);
                GameManagerArena.Instance.BucleJoc();
            }));
    }
    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        yield return new WaitForSeconds(tempsDespera);
        accio();
    }
    public void ProcessarEstadoAlterado(AtacSO at)
    {
        if (estadosAlterados != null)
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
