using UnityEngine;

[CreateAssetMenu(fileName = "AtacSO", menuName = "Scriptable Objects/AtacSO")]
public class AtacSO : ScriptableObject
{

    public int id;
    public string nom;
    public int mal;
    public EstadoAlteradoSO estat;
    public int mana;

}