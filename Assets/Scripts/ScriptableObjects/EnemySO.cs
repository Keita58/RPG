using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public int hp;
    public int spd;
    public int def;
    public int atk;
    public int mana;
}
