using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerArena : MonoBehaviour
{
    [SerializeField] GameObject _Jugador;
    [SerializeField] PlayerSO _JugadorSO;
    [SerializeField] List<EnemySO> _Enemics;
    [SerializeField] List<GameObject> _OrdreAtac; // Llista de tots els enemics a l'inici
    LinkedList<GameObject> PilaEnemics = new LinkedList<GameObject>();

    public static GameManagerArena Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case "Arena":
                int numEnemics = Random.Range(0, _Enemics.Count);

                for (int i = 0; i < numEnemics; i++)
                {
                    _OrdreAtac[i].SetActive(true);

                }

                var aux = _OrdreAtac.OrderByDescending(enemic => enemic.GetComponent<EnemySO>().spd).ToList();
                PilaEnemics = new LinkedList<GameObject>(aux);

                for (var nodeActual = PilaEnemics.First; nodeActual != null; nodeActual = nodeActual.Next)
                {
                    if(nodeActual.Value.GetComponent<EnemySO>().spd <= _JugadorSO.spd)
                    {
                        LinkedListNode<GameObject> aux2 = new LinkedListNode<GameObject>(_Jugador);
                        PilaEnemics.AddBefore(nodeActual, aux2);
                        break;
                    }
                }
                break;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        switch (scene.name)
        {
            case "Arena":
                //_JugadorSO.Hp = _Jugador.hp;
                break;
        }
    }

    private void AtacaJugador(AtacSO atac)
    {

    }

    private void AtacaEnemic(AtacSO atac)
    {

    }

    public GameObject getJugador()
    {
        return _Jugador;
    }

    public List<GameObject> getEnemics()
    {
        return _OrdreAtac;
    }

    public void AcabarTorn()
    {

    }
}
