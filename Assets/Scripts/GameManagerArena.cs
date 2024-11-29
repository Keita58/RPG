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
    private List<GameObject> PilaEnemics = new List<GameObject>();
    private GameObject enemicSeleccionat;
    public event Action<GameObject> OnSeleccionarTarget;

    public static GameManagerArena Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _Jugador.GetComponent<PlayerCombat>().onMuerto += PlayerMort;
        _Jugador.GetComponent<PlayerCombat>().OnFugir += PlayerFugir;
        int numEnemics = UnityEngine.Random.Range(1, _OrdreAtac.Count + 1);
        PilaEnemics = new List<GameObject>();

        _OrdreAtac[0].gameObject.SetActive(true);
        _OrdreAtac[0].GetComponent<EnemyArena>().Iniciar(_EnemicPrincipal);
        _OrdreAtac[0].transform.Rotate(0, 180, 0);
        PilaEnemics.Add(_OrdreAtac[0]);

        for (int i = 1; i < numEnemics; i++)
        {
            _OrdreAtac[i].gameObject.SetActive(true);
            _OrdreAtac[i].GetComponent<EnemyArena>().Iniciar(_Enemics[UnityEngine.Random.Range(0, _Enemics.Count)]);
            _OrdreAtac[i].transform.Rotate(0, 180, 0);
            PilaEnemics.Add(_OrdreAtac[i]);
        }

        //Això ordena la llista dels enemics per la seva velocitat (una passada)
        PilaEnemics = PilaEnemics.OrderByDescending(enemic => enemic.GetComponent<EnemyArena>().spd).ToList();

        for (int i = 0; i < PilaEnemics.Count; i++)
        {
            if (PilaEnemics[i].GetComponent<EnemyArena>().spd <= _JugadorSO.Spd)
            {
                PilaEnemics.Insert(i, _Jugador);
                break;
            }
        }
        BucleJoc();
    }

    public void BucleJoc()
    {
        for (int i = 0; i < PilaEnemics.Count; i++)
        {
            if (PilaEnemics[i].TryGetComponent<EnemyArena>(out EnemyArena e))
            {
                if (e.hp <= 0)
                {
                    PilaEnemics.Remove(PilaEnemics[i]);
                }
            }
        }
        print($"{gameObject}/{this}: Nombre d'entitats a l'escena - {PilaEnemics.Count}");

        if (PilaEnemics.Count == 1 && PilaEnemics[0] == _Jugador)
            ChangeScene("Overworld");
        else
        {
            Debug.Log("Canvi de torn");
            GameObject aux = PilaEnemics[0];
            PilaEnemics.RemoveAt(0);
            PilaEnemics.Add(aux);
            print("AUX: " + aux);
            if (aux.TryGetComponent<PlayerCombat>(out PlayerCombat p))
            {
                Debug.Log("Canvi de torn Jugador");
                p.IniciarTorn();
            }
            else if (aux.TryGetComponent<EnemyArena>(out EnemyArena e))
            {
                Debug.Log("Canvi de torn Enemic");
                e.EscollirAtac();
            }
        }
    }

    private void ChangeScene(string escena)
    {
        _Jugador.GetComponent<PlayerCombat>().SavePlayer();
        SceneManager.LoadScene(escena);
    }

    private void PlayerMort() => ChangeScene("Overworld");
    private void PlayerFugir() => ChangeScene("Overworld");

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
                OnSeleccionarTarget?.Invoke(go);
            }
        }
    }
}
