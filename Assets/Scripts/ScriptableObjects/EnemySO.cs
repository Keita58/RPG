using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public int hp;
    public int spd;
    public int def;
    public AtacSO[] atk;
    public int mana;
    public RuntimeAnimatorController animator;
    public AnimationClip clipIdle;
    public AnimationClip clipMove;
    public AnimationClip clipAttack;
    public AnimationClip clipAttack2;
    public AnimationClip clipHurt;
    public AnimationClip clipDeath;
    public EstadoAlteradoSO EstadosAlterados;
}
