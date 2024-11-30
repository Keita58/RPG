using UnityEngine;
using UnityEngine.SceneManagement;

public class SeguirJugador : MonoBehaviour
{
    private GameObject Jugador;

    private void Start()
    {
        Jugador = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        this.transform.position = new Vector3(Jugador.transform.position.x, Jugador.transform.position.y, this.transform.position.z);
    }
}
