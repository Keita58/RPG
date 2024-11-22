using UnityEngine;
using UnityEngine.UI;

public class UICombat : MonoBehaviour
{
    [SerializeField] PlayerCombat player;

    private void Awake()
    {
        player.OnMostrarAccions += Mostrar;
        player.OnOcultarAccions += Ocultar;
        player.OnDeshabilitarAccions += Deshabilitar;
        Ocultar();
    }

    private void Mostrar()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = true;

        }
        gameObject.SetActive(true);
    }

    private void Ocultar()
    {
        gameObject.SetActive(false);
    }

    private void Deshabilitar()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).GetComponent<Button>().interactable = false;

        }
    }

    public void OnBotoAtacar()
    {
        player.AccioAtacar();
    }

    public void OnBotoMagia()
    {
        player.AccioSeleccionarMagia();
    }

    public void OnBotoObjecte()
    {
        player.AccioObjecte();
    }

    public void OnBotoFugir()
    {
        player.AccioFugir();
    }

    public void Cancelar()
    {
        player.AccioCancelar();
    }
}
