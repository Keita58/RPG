using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject, IDamageable
{
    private int hp;
    private int damageAtk;
    private int mana;
    private int lvl;
    private int def;
    private AtacSO[] atacs;

    public List<AtacSO> listaAtaques;
    public int Hp { get => hp; }
    public int DamageAtk { get => damageAtk;  }
    public int Mana { get => mana;  }
    public int Lvl { get => lvl;}
    public int Def { get => def; }
    EstadosAlterados estado;

    public event Action<AtacSO> onDamaged;

    public void RebreMal(AtacSO atac)
    {
        if ( atac.mal>def)
        {
            int hprestat = atac.mal - def;
            hp -= hprestat;
        }
        if (atac.estat != null)
        {
            estado.IniciarEstadoAlterado(atac.estat);
        }
        
    }

    public void lvlUP()
    {
        lvl++;
        hp += 10;
        mana += 10;
        def += 2;
        damageAtk += 1;
        if (lvl == 2)
        {
            listaAtaques.Add(atacs[0]);
        }
        else if (lvl == 5)
        {
            listaAtaques.Add(atacs[1]);
        }else if (lvl == 10)
        {
            listaAtaques.Add(atacs[2]);
        }
        else if (lvl == 15)
        {
            listaAtaques.Add(atacs[3]);
        }
    }


    


    //poner el inventario y todo.
}
