using System;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject, IAttack, IDamageable
{
    private int hp;
    private int damageAtk;
    private int mana;
    private int lvl;
    private int def;

    public int Hp { get => hp; }
    public int DamageAtk { get => damageAtk;  }
    public int Mana { get => mana;  }
    public int Lvl { get => lvl;}
    public int Def { get => def; }
    public AtacSO atac { set => throw new NotImplementedException(); }

    public event Action<AtacSO> onDamaged;

    public void RebreMal(AtacSO atac)
    {
        int hprestat= atac.mal-def ;
        hp-=hprestat;
    }

    


    //poner el inventario y todo.
}
