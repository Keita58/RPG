using System;
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
    [SerializeField] HpMaxJugador HpMax;
    [SerializeField] BDAtacs Atacs;
    [SerializeField] GameObject JugadorOw;

    [Serializable]
    private class SaveDataInfo
    {
        public int[] ids;
    }

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
            AtacsJugador = Atacs.ToIDs(JugadorSO.listaAtaques.AsReadOnly()),
            MaxHp = HpMax.hpMax,
            MaxMana = HpMax.manaMax,
            PosJugador = new Vector2(JugadorOw.transform.position.x, JugadorOw.transform.position.y),
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
