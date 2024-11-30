using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class DadesJugador
{
    public int Hp { get; set; }
    public int DamageAtk { get; set; }
    public int Mana { get; set; }
    public int Lvl { get; set; }
    public int Def { get; set; }
    public int Spd { get; set; }
    public int Xp { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public List<AtacSO> AtacsJugador { get; set; }
}
