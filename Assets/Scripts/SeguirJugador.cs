using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject Jugador;
    void Update()
    {
        this.transform.position = new Vector3(Jugador.transform.position.x, Jugador.transform.position.y, this.transform.position.z);
    }
}
