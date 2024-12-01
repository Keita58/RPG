using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class EnemyArena : MonoBehaviour,  IPointerDownHandler, Avisable
{
    [SerializeField] HealthBar vidaPantalla;
    public GameObject Seleccionat;
    private AtacSO escollit;
    private Animator animator;
    private EnemySO EnemySO;
    public int id { get; private set; }
    public bool selected { get; set; }
    public int hp { get; private set; }
    public int HP;
    public AtacSO[] atk { get; private set; }
    public int def { get; private set; }
    public int spd { get; private set; }
    public int mana { get; private set; }
    public event Action<AtacSO> onDamaged;
    public event Action<AtacSO> atacar;
    public event Action<string> OnIniciarTornUI;
    public event Action<string, string> OnRebreEstadoAlteradoUI;
    public event Action<string, int> OnRebreMalUI;
    public event Action<string> OnEmpezarVentajaUI;
    public event Action OnSeleccionarTargetUI;

    EstadosAlterados estadosAlterados;
    [SerializeField] string estadoNom;
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
        this.HP = hp;
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
            OnEmpezarVentajaUI?.Invoke("jugador");
            Debug.Log($"{gameObject}/{this}: INICIA VENTAJA ESTADO ALTERADO: {EnemySO.EstadosAlterados.nom}");
            this.estadosAlterados = new EstadosAlterados(EnemySO.EstadosAlterados.nom, EnemySO.EstadosAlterados.incapacitat, EnemySO.EstadosAlterados.torns, EnemySO.EstadosAlterados.hp, EnemySO.EstadosAlterados.modAtk, EnemySO.EstadosAlterados.modDef, EnemySO.EstadosAlterados.modSpd);
            estadosAlterados.Torns--;
            if(estadosAlterados.Torns > 0)
                GameManagerArena.Instance.BucleJoc();
            //TODO: Mirar que passa
        }
        OnIniciarTornUI?.Invoke("enemic");
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
            estadoNom = estadosAlterados.Nom;
            Debug.Log($"{gameObject}/{this}: INICIA ESTADO ALTERADO: {estadosAlterados.Nom}");

            OnRebreEstadoAlteradoUI?.Invoke("enemic", estadosAlterados.Nom);
            this.hp -= estadosAlterados.Hp;
            if (estadosAlterados.Nom != "Ventaja")
            {
                StartCoroutine(AnimacioMal());
            }
            vidaPantalla.UpdateHealth(estadosAlterados.Hp);
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

    public void RebreMal(AtacSO atac, int damageAtackPlayer)
    {
        if (atac.mal > this.def && this.hp>0)
        {
            OnRebreMalUI?.Invoke("L'enemic", atac.mal);
            StartCoroutine(AnimacioMal());
            Debug.Log("Vida abans mal: " + this.hp);
            this.hp -= (atac.mal*damageAtackPlayer) - this.def;
            Debug.Log("Vida despr�s mal: " + this.hp);
            if (atac.estat != null)
            {
                Debug.Log($"{gameObject}/{this}: INICIA ESTADO ALTERADO REBRE MAL: {atac.estat.nom}");
                this.estadosAlterados = new EstadosAlterados(atac.estat.nom, atac.estat.incapacitat, atac.estat.torns, atac.estat.hp, atac.estat.modAtk, atac.estat.modDef, atac.estat.modSpd);
            }
            vidaPantalla.UpdateHealth(atac.mal*damageAtackPlayer);
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
