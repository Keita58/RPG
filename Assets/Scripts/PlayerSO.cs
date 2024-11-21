using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField]private int hp;
    [SerializeField] private int damageAtk;
    [SerializeField] private int mana;
    [SerializeField] private int lvl;
    [SerializeField] private int def;
    [SerializeField] private int Spd;


    public List<AtacSO> listaAtaques;
    public int Hp { get => hp; set => hp = value; }
    public int DamageAtk { get => damageAtk; set => damageAtk = value; }
    public int Mana { get => mana; set => mana = value; }
    public int Lvl { get => lvl; set => lvl = value; }
    public int Def { get => def; set => def = value; }
    public int spd {  get => Spd; set => Spd = value;}
    //poner el inventario y todo.
}
