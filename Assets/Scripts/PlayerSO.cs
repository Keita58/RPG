using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    private int hp;
    private int damageAtk;
    private int mana;
    private int lvl;
    private int def;


    public List<AtacSO> listaAtaques;
    public int Hp { get => hp; set => hp = value; }
    public int DamageAtk { get => damageAtk; set => damageAtk = value; }
    public int Mana { get => mana; set => mana = value; }
    public int Lvl { get => lvl; set => lvl = value; }
    public int Def { get => def; set => def = value; }
    public int spd {  get => spd; set => spd = value;}
    



    //poner el inventario y todo.
}
