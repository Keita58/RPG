using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerArena : MonoBehaviour
{
    [SerializeField] GameObject _Jugador;
    [SerializeField] PlayerSO _JugadorSO;
    [SerializeField] List<EnemySO> _Enemics; // Llista de tots els tipus diferents d'enemics 
    [SerializeField] List<GameObject> _OrdreAtac; // Llista de tots els enemics a l'inici de l'escena
    [SerializeField] EnemySO _EnemicPrincipal; // Enemic que hem trobat al OW
    private LinkedList<GameObject> PilaEnemics = new LinkedList<GameObject>();
    private GameObject enemicSeleccionat;
    public event Action<GameObject> OnSeleccionarTarget;

    public static GameManagerArena Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        //_Jugador.GetComponent<PlayerCombat>().onMuerto += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case "Arena":
                int numEnemics = UnityEngine.Random.Range(1, _OrdreAtac.Count);

                _OrdreAtac[0].gameObject.SetActive(true);
                _OrdreAtac[0].GetComponent<EnemyArena>().Iniciar(_EnemicPrincipal);
                _OrdreAtac[0].transform.Rotate(0, 180, 0);

                for (int i = 1; i < numEnemics; i++)
                {
                    _OrdreAtac[i].gameObject.SetActive(true);
                    _OrdreAtac[i].GetComponent<EnemyArena>().Iniciar(_Enemics[UnityEngine.Random.Range(0, _Enemics.Count)]);
                    _OrdreAtac[i].transform.Rotate(0, 180, 0);
                }

                //Això ordena la llista dels enemics per la seva velocitat (una passada)
                var aux = _OrdreAtac.OrderByDescending(enemic => enemic.GetComponent<EnemyArena>().spd).ToList();
                PilaEnemics = new LinkedList<GameObject>(aux);

                for (var nodeActual = PilaEnemics.First; nodeActual != null; nodeActual = nodeActual.Next)
                {
                    
                    if(nodeActual.Value.GetComponent<EnemyArena>().spd <= _JugadorSO.Spd)
                    {
                        LinkedListNode<GameObject> aux2 = new LinkedListNode<GameObject>(_Jugador);
                        PilaEnemics.AddBefore(nodeActual, aux2);
                        break;
                    }
                }
                BucleJoc();
                break;
        }
    }

    public void BucleJoc()
    {
        for(var nodeActual = PilaEnemics.First; nodeActual != null; nodeActual = nodeActual.Next) 
        {
            EnemyArena e = nodeActual.Value.GetComponent<EnemyArena>();
            if (e != null)
            {
                if (e.hp <= 0)
                {
                    PilaEnemics.Remove(nodeActual);
                }
            }
        }

        if (PilaEnemics.Count == 1 && PilaEnemics.First.Value == _Jugador)
            OnSceneUnloaded(SceneManager.GetSceneByName("Overworld"));
        else
        {
            GameObject aux = PilaEnemics.First.Value;
            PlayerCombat jugador = aux.GetComponent<PlayerCombat>();
            EnemyArena enemic = aux.GetComponent<EnemyArena>();
            if(jugador != null)
            {
                jugador.IniciarTorn();
            }
            else if(enemic != null)
            {
                enemic.EscollirAtac();
            }
            PilaEnemics.RemoveFirst();
            PilaEnemics.AddLast(aux);
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _Jugador.GetComponent<PlayerCombat>().SavePlayer();
        SceneManager.LoadScene(scene.name);
    }

    public GameObject getJugador()
    {
        return _Jugador;
    }

    public List<GameObject> getEnemics()
    {
        return _OrdreAtac;
    }

    public void CanviaEnemicSelected(GameObject GO)
    {
        print("Canvi de seleccionat");
        foreach (GameObject go in _OrdreAtac)
        {
            if (GO != go)
            {
                go.GetComponent<EnemyArena>().Seleccionat.SetActive(false);
                go.GetComponent<EnemyArena>().selected = false;
            }
            else
            {
                OnSeleccionarTarget.Invoke(go);
            }
        }
    }
}
