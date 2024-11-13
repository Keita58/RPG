using UnityEngine;

[CreateAssetMenu(fileName = "AtacSO", menuName = "Scriptable Objects/AtacSO")]
public class AtacSO : ScriptableObject
{
    enum Tipus
    {
        FOC,
        AIGUA,
        LLUM,
    }

    string nom;
    int mal;
    Tipus tipus;
    EstadoAlteradoSO estat;
}