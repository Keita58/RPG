using System;
using System.Collections;
using UnityEngine;
public class EnemyArena : MonoBehaviour, IAttack, IDamageable
{
    public EnemySO EnemySO;
    private int id;
    private bool selected;
    Animator animator;
    public int getId()
    {
        return id; 
    }
    public void setId(int i)
    {
        this.id = i;
    }
    private int hp;
    private AtacSO[] atk;
    private int def;
    private int spd;
    private int mana;
    private AtacSO escollit;
    public event Action<AtacSO> onDamaged;
    EstadosAlterados estadosAlterados;
    public AtacSO atac { set => value = escollit; }

    void Awake()
    {
        this.hp = this.EnemySO.hp;
        this.atk = this.EnemySO.atk;
        this.def = this.EnemySO.def;
        this.spd = this.EnemySO.spd;
        this.mana = this.EnemySO.mana;
        this.animator = this.EnemySO.animator;
    }
    private void Start()
    {
        //GameManager.instance.ActivarAtac += EscollirAtac();
    }
    private void EscollirAtac()
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
        }
        this.escollit = at;
    }
    private void OnMouseDown()
    {
        this.selected = true;
        //GameManager.instance.setId(this.id);
    }

    public void RebreMal(AtacSO atac)
    {
        if (atac.mal > this.def)
        {
            StartCoroutine(AnimacioMal());
            this.hp -= atac.mal - this.def;
            if (atac.estat != null)
                this.estadosAlterados.IniciarEstadoAlterado(atac.estat);
        }
        if (this.hp <= 0)
        {
            Destroy(this.gameObject);
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
}
