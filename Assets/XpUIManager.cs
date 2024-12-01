using TMPro;
using UnityEngine;

public class XpUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI xp;
    [SerializeField] XpJugadorVictoria _XpJugador;
    void Start()
    {
        xp.text = _XpJugador.xpGuanyat.ToString();
    }

}
