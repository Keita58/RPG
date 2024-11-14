using UnityEngine;

[CreateAssetMenu(fileName = "AtacSO", menuName = "Scriptable Objects/AtacSO")]
public class AtacSO : ScriptableObject
{
    public enum Tipus
    {
        FOC,
        AIGUA,
        LLUM,
    }

    public string nom;
    public int mal;
    public Tipus tipus;
    public EstadoAlteradoSO estat;


}