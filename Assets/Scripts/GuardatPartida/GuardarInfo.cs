using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class GuardarInfo : MonoBehaviour
{
    private const string saveFileName = "savegame.json";
    [SerializeField] TextAsset json;
    [SerializeField] PlayerSO JugadorSO;
    [SerializeField] TextMeshProUGUI Missatge;
    
    public void Guardar()
    {
        string filePath = Application.persistentDataPath +"/"+ saveFileName;
        GameObject JugadorEscena = GameObject.FindGameObjectWithTag("Player");
        
        DadesJugador dadesJugador = new()
        {
            Hp = JugadorSO.Hp,
            Mana = JugadorSO.Mana,
            Spd = JugadorSO.Spd,
            DamageAtk = JugadorSO.DamageAtk,
            Lvl = JugadorSO.Lvl,
            Def = JugadorSO.Def,
            Xp = JugadorSO.Xp,
            AtacsJugador = JugadorSO.listaAtaques
        };

        string infoAGuardar = JsonUtility.ToJson(dadesJugador, true);
        File.WriteAllText(filePath, infoAGuardar);
        Missatge.text = "Has guardat la partida!";
        StartCoroutine(EsperaCanviText());
    }

    IEnumerator EsperaCanviText()
    {
        yield return new WaitForSecondsRealtime(2f);
        Missatge.text = "";
    }
}
