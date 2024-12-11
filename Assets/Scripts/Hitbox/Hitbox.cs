using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EstadoAlteradoSO estadoAlteradoSO;
    [SerializeField] PosicioOW PosicioJugadorOw;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        print("AWAKE HITBOX"+PosicioJugadorOw);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemy))
        {
            PosicioJugadorOw.posJugador = new Vector2(this.transform.position.x, this.transform.position.y);
            //Debug.Log($"{gameObject}/{this}: COLISIONO CON ENEMIGO: {collision.gameObject}");
            enemy._enemySO.EstadosAlterados = estadoAlteradoSO;
            GameManagerOW.Instance.EnemicPrincipal = enemy._enemySO;
            SceneManager.LoadScene("Arena");
            Destroy(collision.gameObject);
        }
        else if(collision.TryGetComponent<Player>(out Player player))
        {
            PosicioJugadorOw.posJugador = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
            //Debug.Log($"{gameObject}/{this}: COLISIONO CON JUGADOR: {collision.gameObject}");
            player.player.estadosAlterados=estadoAlteradoSO;
            GameManagerOW.Instance.EnemicPrincipal = this.GetComponentInParent<EnemyStateMachine>()._enemySO;
            print(player.player.estadosAlterados);
            SceneManager.LoadScene("Arena");
        }
    }
}
