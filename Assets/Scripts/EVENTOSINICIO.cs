using UnityEngine;
using UnityEngine.SceneManagement;

public class EVENTOSINICIO : MonoBehaviour
{
    [SerializeField] PosicioOW PosJugador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        PosJugador.posJugador = Vector2.zero;
        SceneManager.LoadScene("Overworld");
    }

    public void Sortir()
    {
        Application.Quit();
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("INICIO");
    }
}
