using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarInfo : MonoBehaviour
{
    [SerializeField] TextAsset Json;
    [SerializeField] TextMeshProUGUI Missatges;

    public void Carrega()
    {
        DadesJugador Jugador;
        if(!Json.ToString().Equals(""))
        {
            Jugador = JsonUtility.FromJson<DadesJugador>(Json.ToString());
            GameManagerOW.Instance.JugadorSO.Hp = Jugador.Hp;
            GameManagerOW.Instance.JugadorSO.DamageAtk = Jugador.DamageAtk;
            GameManagerOW.Instance.JugadorSO.Mana = Jugador.Mana;
            GameManagerOW.Instance.JugadorSO.Lvl = Jugador.Lvl;
            GameManagerOW.Instance.JugadorSO.Def = Jugador.Def;
            GameManagerOW.Instance.JugadorSO.Spd = Jugador.Spd;
            GameManagerOW.Instance.JugadorSO.Xp = Jugador.Xp;

            SceneManager.LoadScene("Overworld");
        }
        else
        {
            Missatges.text = "No hi ha cap partida guardada!";
            StartCoroutine(EsborrarText());
        }
    }

    IEnumerator EsborrarText()
    {
        yield return new WaitForSeconds(2);
        Missatges.text = "";
    }
}
