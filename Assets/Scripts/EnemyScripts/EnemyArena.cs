using System;
using UnityEngine;
public class EnemyArena : MonoBehaviour, IAttack, IDamageable
{
    public EnemySO EnemySO;
    private int id;
    private bool selected;
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

    void OnEnable()
    {
        this.hp = this.EnemySO.hp;
        this.atk = this.EnemySO.atk;
        this.def = this.EnemySO.def;
        this.spd = this.EnemySO.spd;
        this.mana = this.EnemySO.mana;
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
        if (atac.mal>this.def)
            this.hp-=atac.mal-this.def;
        if(atac.estat!=null)
            this.estadosAlterados.IniciarEstadoAlterado(atac.estat);
        if (this.hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        //GameManager.instance.ActivarAtac -= EscollirAtac();
    }
}
