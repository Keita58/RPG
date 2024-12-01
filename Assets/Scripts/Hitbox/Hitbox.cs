using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EstadoAlteradoSO estadoAlteradoSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemy))
        {
            Debug.Log($"{gameObject}/{this}: COLISIONO CON ENEMIGO: {collision.gameObject}");
            enemy._enemySO.EstadosAlterados = estadoAlteradoSO;
            SceneManager.LoadScene("Arena");
            Destroy(collision.gameObject);
        }
        else if(collision.TryGetComponent<Player>(out Player player))
        {
            player.player.estadosAlterados=estadoAlteradoSO;
            print(player.player.estadosAlterados);
            SceneManager.LoadScene("Arena");
        }
    }
}
