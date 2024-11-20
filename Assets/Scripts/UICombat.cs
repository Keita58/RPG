using UnityEngine;

public class UICombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] AtacSO atac;

    public void MostrarInfo(AtacSO atac)
    {
        this.atac = atac;

    }
}
