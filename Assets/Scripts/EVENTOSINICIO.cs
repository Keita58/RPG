using UnityEngine;
using UnityEngine.SceneManagement;

public class EVENTOSINICIO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Overworld");
    }
    public void LoadData()
    {

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
