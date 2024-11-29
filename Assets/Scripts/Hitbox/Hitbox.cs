using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EstadoAlteradoSO estadoAlteradoSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("debug moment");
        if (collision.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemy))
        {
            enemy._enemySO.EstadosAlterados = estadoAlteradoSO;
            SceneManager.LoadScene("Arena");
        }
        else if(collision.TryGetComponent<Player>(out Player player))
        {
            player.player.estadosAlterados=estadoAlteradoSO;
            print(player.player.estadosAlterados);
            SceneManager.LoadScene("Arena");
        }
    }
}
