using UnityEngine;

[CreateAssetMenu(fileName = "EstadoAlteradoSO", menuName = "Scriptable Objects/EstadoAlteradoSO")]
public class EstadoAlteradoSO : ScriptableObject
{
    public int incapacitat;
    public int torns;
    public int hp;
    public int modAtk;
    public int modDef;
    public int modSpd;
}
