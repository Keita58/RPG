using System.IO;
using UnityEngine;

public class GuardarInfo : MonoBehaviour
{
    [SerializeField] TextAsset json;
    [SerializeField] PlayerSO Jugador;
    
    void Start()
    {
        DadesJugador dadesJugador = new DadesJugador();
        dadesJugador.Spd = Jugador.Spd;
        dadesJugador.Mana = Jugador.Mana;
        dadesJugador.DamageAtk = Jugador.DamageAtk;
        dadesJugador.Def = Jugador.Def;
        dadesJugador.Lvl = Jugador.Lvl;
        dadesJugador.Hp = Jugador.Hp;

        string infoAGuardar = JsonUtility.ToJson(dadesJugador);
        File.WriteAllText("./Assets/Json/save.json", infoAGuardar);
    }
}
