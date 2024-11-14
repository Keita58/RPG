using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public int hp;
    public int spd;
    public int def;
    public AtacSO[] atk;
    public int mana;
}
