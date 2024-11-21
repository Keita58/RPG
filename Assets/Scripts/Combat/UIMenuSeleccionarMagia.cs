using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuSeleccionarMagia : MonoBehaviour
{
    [SerializeField] PlayerCombat player;

    private void Awake()
    {
        player.OnMostrarMagia += Mostrar;
        player.OnOcultarMagia += Ocultar;

        Ocultar();
    }

    private void Mostrar(List<AtacSO> magies)
    {
        gameObject.SetActive(true);
    }

    private void Ocultar()
    {
        gameObject.SetActive(false);
    }
}
