using UnityEngine;

public class PosicioJugador : MonoBehaviour
{
    [SerializeField] PosicioOW PosicioJugadorOw;
    
    void Start()
    {
        transform.position = new Vector3(PosicioJugadorOw.posJugador.x, PosicioJugadorOw.posJugador.y, 0);
    }
}
