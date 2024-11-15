using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerArena : MonoBehaviour
{
    [SerializeField] PlayerCombat _Jugador;
    [SerializeField] PlayerSO _JugadorSO;
    [SerializeField] List<EnemySO> _Enemics;
    [SerializeField] List<GameObject> _OrdreAtac; // Llista de tots els enemics a l'inici
    private Queue<IAttack> PilaEnemics = new Queue<IAttack>();

    public static GameManagerArena Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        _Jugador.onAttack += AtacaJugador;
        foreach(GameObject enemic in _OrdreAtac)
        {
            enemic.GetComponent<EnemyArena>().atacar += AtacaEnemic;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case "Arena":
                PilaEnemics = from enemic in _OrdreAtac
                              orderby enemic descending
                              select enemic;
                int numEnemics = Random.Range(0, _Enemics.Count);

                for(int i = 0; i < numEnemics; i++)
                {
                    //_Enemics[i].SetActive(true);
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
}
