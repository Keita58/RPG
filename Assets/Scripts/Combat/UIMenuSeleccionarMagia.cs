using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuSeleccionarMagia : MonoBehaviour
{
    [SerializeField] PlayerCombat player;
    [SerializeField] Transform botons;

    private void Awake()
    {
        player.OnMostrarMagia += Mostrar;
        player.OnOcultarMagia += Ocultar;

        Ocultar();
    }

    private void Mostrar(List<AtacSO> magies)
    {
        gameObject.SetActive(true);
        int i = 0;
        Debug.Log($"Assignant atacs: {magies.Count}");
        for (i = 0; i < magies.Count; i++)
            botons.GetChild(i).GetComponent<UIBotoAtac>().Inicialitzar(magies[i], this);
        Debug.Log($"Atacs assignats {i}");
        for (; i < botons.childCount; i++)
            botons.GetChild(i).GetComponent<UIBotoAtac>().Ocultar();
    }

    private void Ocultar()
    {
        gameObject.SetActive(false);
    }

    public void UtilitzaHabilitat(AtacSO atac)
    {
        //avisar al player que faci l'atac
        player.AccioHabilitat(atac);
    }

    public void CancelarAccio()
    {
        //dir al player que cancelem
    }
}
