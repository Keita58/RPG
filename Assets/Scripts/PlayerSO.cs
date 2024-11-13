using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public AnimatorController animator;
    public int hp;
    public int damageAtk;
    public int mana;
    //poner el inventario y todo.
}
