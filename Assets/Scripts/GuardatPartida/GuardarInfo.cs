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
        DadesJugador dadesJugador = new()
        {
            Hp = JugadorSO.Hp,
            Mana = JugadorSO.Mana,
            Spd = JugadorSO.Spd,
            DamageAtk = JugadorSO.DamageAtk,
            Lvl = JugadorSO.Lvl,
            Def = JugadorSO.Def,
            Xp = JugadorSO.Xp,
            PosX = JugadorEscena.transform.position.x,
            PosY = JugadorEscena.transform.position.y,
            AtacsJugador = JugadorSO.listaAtaques
        };
        print(dadesJugador.AtacsJugador);
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
