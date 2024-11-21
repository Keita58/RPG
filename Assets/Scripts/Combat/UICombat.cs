using UnityEngine;

public class UICombat : MonoBehaviour
{
    [SerializeField] PlayerCombat player;

    private void Awake()
    {
        player.OnMostrarAccions += Mostrar;
        player.OnOcultarAccions += Ocultar;

        Ocultar();
    }

    private void Mostrar()
    {
        gameObject.SetActive(true);
    }

    private void Ocultar()
    {
        gameObject.SetActive(false);
    }

    public void OnBotoAtacar()
    {
        player.AccioAtacar();
    }

    public void OnBotoMagia()
    {
        player.AccioMagia();
    }

    public void OnBotoObjecte()
    {
        player.AccioObjecte();
    }

    public void OnBotoFugir()
    {
        player.AccioFugir();
    }
}
