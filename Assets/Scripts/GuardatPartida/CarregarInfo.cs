using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarInfo : MonoBehaviour
{
    [SerializeField] TextAsset Json;

    public void Carrega()
    {
        DadesJugador Jugador;
        if (JsonUtility.FromJson<DadesJugador>(Json.ToString()) != null)
        {
            Jugador = JsonUtility.FromJson<DadesJugador>(Json.ToString());
            GameManagerOW.Instance.JugadorSO.Hp = Jugador.Hp;
            GameManagerOW.Instance.JugadorSO.DamageAtk = Jugador.DamageAtk;
            GameManagerOW.Instance.JugadorSO.Mana = Jugador.Mana;
            GameManagerOW.Instance.JugadorSO.Lvl = Jugador.Lvl;
            GameManagerOW.Instance.JugadorSO.Def = Jugador.Def;
            GameManagerOW.Instance.JugadorSO.Spd = Jugador.Spd;
            GameManagerOW.Instance.JugadorSO.Xp = Jugador.Xp;

            GameManagerOW.Instance.Jugador.transform.position = new Vector3(Jugador.PosX, Jugador.PosY, 0);
            SceneManager.LoadScene("Overwold");
        }
    }
}
