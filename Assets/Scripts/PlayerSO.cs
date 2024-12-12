using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private int hp;
    [SerializeField] private int damageAtk;
    [SerializeField] private int mana;
    [SerializeField] private int lvl;
    [SerializeField] private int def;
    [SerializeField] private int spd;
    [SerializeField] private int xp;
    [SerializeField] private EstadosAlterados EstadosAlterados;

    public List<AtacSO> listaAtaques;
    public int Hp { get => hp; set => hp = value; }
    public int DamageAtk { get => damageAtk; set => damageAtk = value; }
    public int Mana { get => mana; set => mana = value; }
    public int Lvl { get => lvl; set => lvl = value; }
    public int Def { get => def; set => def = value; }
    public int Spd {  get => spd; set => spd = value;}
    public int Xp { get => xp; set => xp = value; }

    public EstadoAlteradoSO estadosAlterados;

    public PlayerSO(int hp, int damageAtk, int mana, int lvl, int def, int spd, int xp, List<AtacSO> listaAtaques)
    {
        Hp = hp;
        DamageAtk = damageAtk;
        Mana = mana;
        Lvl = lvl;
        Def = def;
        Spd = spd;
        Xp = xp;
        this.listaAtaques = listaAtaques;
    }
    //poner el inventario y todo.
}
