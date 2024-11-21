using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
public class EnemyArena : MonoBehaviour, IAttack, IDamageable
{
    private AtacSO escollit;
    private Animator animator;
    private EnemySO EnemySO;
    public int id { get; private set; }
    public bool selected { get; private set; }
    public int hp { get; private set; }
    public AtacSO[] atk { get; private set; }
    public int def { get; private set; }
    public int spd { get; private set; }
    public int mana { get; private set; }
    public event Action<AtacSO> onDamaged;
    public event Action<AtacSO> atacar;
    EstadosAlterados estadosAlterados;

    public AtacSO atac { set => value = escollit; }

    private void Awake()
    {
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
        }
        this.escollit = at;
        atacar.Invoke(at);
    }

    private void OnMouseDown()
    {
        this.selected = true;
        //GameManagerArena.instance.setId(this.id);
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
