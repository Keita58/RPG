using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class GuardarInfo : MonoBehaviour
{
    [SerializeField] TextAsset json;
    [SerializeField] PlayerSO JugadorSO;
    [SerializeField] GameObject JugadorEscena;
    [SerializeField] TextMeshProUGUI Missatge;
    
    public void Guardar()
    {
        DadesJugador dadesJugador = new DadesJugador();
        dadesJugador.Spd = JugadorSO.Spd;
        dadesJugador.Mana = JugadorSO.Mana;
        dadesJugador.DamageAtk = JugadorSO.DamageAtk;
        dadesJugador.Def = JugadorSO.Def;
        dadesJugador.Lvl = JugadorSO.Lvl;
        dadesJugador.Hp = JugadorSO.Hp;
        dadesJugador.PosX = JugadorEscena.transform.position.x;
        dadesJugador.PosY = JugadorEscena.transform.position.y;

        string infoAGuardar = JsonUtility.ToJson(dadesJugador);
        File.WriteAllText("./Assets/Json/save.json", infoAGuardar);
        Missatge.text = "Has guardat la partida!";
        StartCoroutine(EsperaCanviText());
    }

    IEnumerator EsperaCanviText()
    {
        yield return new WaitForSeconds(2);
        Missatge.text = "";
    }
}
