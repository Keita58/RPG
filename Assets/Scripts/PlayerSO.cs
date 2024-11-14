using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
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



    //poner el inventario y todo.
}
