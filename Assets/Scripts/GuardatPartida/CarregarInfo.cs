using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Missatges;
    [SerializeField] HpMaxJugador HpMaxJugador;
    [SerializeField] BDAtacs Atacs;
    [SerializeField] PosicioOW PosicioJugador;

    public void Carrega()
    {
        DadesJugador Jugador;
        string filePath = Application.persistentDataPath + "/" + "savegame.json";

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            Jugador = JsonUtility.FromJson<DadesJugador>(jsonContent); // Deserializar el contenido JSON
            HpMaxJugador.hpMax = Jugador.MaxHp;
            HpMaxJugador.manaMax = Jugador.MaxMana;

            GameManagerOW.Instance.JugadorSO.Hp = Jugador.Hp;
            GameManagerOW.Instance.JugadorSO.DamageAtk = Jugador.DamageAtk;
            GameManagerOW.Instance.JugadorSO.Mana = Jugador.Mana;
            GameManagerOW.Instance.JugadorSO.Lvl = Jugador.Lvl;
            GameManagerOW.Instance.JugadorSO.Def = Jugador.Def;
            GameManagerOW.Instance.JugadorSO.Spd = Jugador.Spd;
            GameManagerOW.Instance.JugadorSO.Xp = Jugador.Xp;
            GameManagerOW.Instance.JugadorSO.listaAtaques = Atacs.FromIDs(Jugador.AtacsJugador);
            PosicioJugador.posJugador = Jugador.PosJugador;


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
