using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "BDAtacs", menuName = "Scriptable Objects/BDAtacs")]
public class BDAtacs : ScriptableObject
{
    [SerializeField]
    List<AtacSO> ScriptableExemples;

    public List<AtacSO> FromIDs(int[] listIDs)
    {
        AtacSO ScriptableFromID(int id)
        {
            foreach (AtacSO currentScriptable in ScriptableExemples)
            {
                if (currentScriptable.id == id)
                    return currentScriptable;
            }
            return null;
        }

        List<AtacSO> scriptableExemples = new List<AtacSO>(listIDs.Length);

        foreach (int id in listIDs)
            scriptableExemples.Add(ScriptableFromID(id));

        return scriptableExemples;
    }

    public int[] ToIDs(ReadOnlyCollection<AtacSO> listScriptables)
    {
        int[] listIDs = new int[listScriptables.Count];

        for (int i = 0; i < listScriptables.Count; ++i)
        {
            listIDs[i] = listScriptables[i].id;
        }

        return listIDs;
    }

}
