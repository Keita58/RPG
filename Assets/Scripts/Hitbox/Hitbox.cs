using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EstadoAlteradoSO estadoAlteradoSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("debug moment");
        if (collision.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemy) == true)
        {
            enemy._enemySO.EstadosAlterados.IniciarEstadoAlterado(estadoAlteradoSO);
            SceneManager.LoadScene("Arena");
        }else if(collision.TryGetComponent<Player>(out Player player) == true)
        {
            print(player.player.estadosAlterados);
            player.player.estadosAlterados.IniciarEstadoAlterado(estadoAlteradoSO);
            SceneManager.LoadScene("Arena");
        }
    }
}
