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
    [SerializeField] List<GameObject> _EnemicsGOPantalla; // Llista de tots els enemics a l'inici de l'escena
    [SerializeField] EnemySO _EnemicPrincipal; // Enemic que hem trobat al OW
    [SerializeField] XpJugadorVictoria _XpJugador;
    private List<GameObject> OrdreJoc = new List<GameObject>();
    private GameObject enemicSeleccionat;
    private int _NumEnemics;
    public event Action<GameObject> OnSeleccionarTarget;
    public event Action<EnemySO> OnAgafarEnemicPrincipal;

    public static GameManagerArena Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _EnemicPrincipal = GameManagerOW.Instance.EnemicPrincipal;
        _Jugador.GetComponent<PlayerCombat>().onMuerto += PlayerMort;
        _Jugador.GetComponent<PlayerCombat>().OnFugir += PlayerFugir;
        int numEnemics = UnityEngine.Random.Range(1, _EnemicsGOPantalla.Count + 1);
        OrdreJoc = new List<GameObject>();

        _EnemicsGOPantalla[0].gameObject.SetActive(true);
        _EnemicsGOPantalla[0].GetComponent<EnemyArena>().Iniciar(_EnemicPrincipal);
        _EnemicsGOPantalla[0].transform.Rotate(0, 180, 0);
        OrdreJoc.Add(_EnemicsGOPantalla[0]);
        _NumEnemics++;

        for (int i = 1; i < numEnemics; i++)
        {            
            _EnemicsGOPantalla[i].gameObject.SetActive(true);
            _EnemicsGOPantalla[i].GetComponent<EnemyArena>().Iniciar(_Enemics[UnityEngine.Random.Range(0, _Enemics.Count)]);
            _EnemicsGOPantalla[i].transform.Rotate(0, 180, 0);

            if (_EnemicPrincipal.EstadosAlterados != null)
            {
                if (_EnemicPrincipal.EstadosAlterados.incapacitat)
                    _EnemicsGOPantalla[i].GetComponent<EnemyArena>().EnemySO.EstadosAlterados = _EnemicPrincipal.EstadosAlterados;
            }

            OrdreJoc.Add(_EnemicsGOPantalla[i]);

            //print("Bueno he generat un enemic " + _Enemics[i].name + "amb estat alterat Bueno " + _Enemics[i].EstadosAlterados);
            _NumEnemics++;
        }

        //Aix� ordena la llista dels enemics per la seva velocitat (una passada)
        OrdreJoc = OrdreJoc.OrderByDescending(enemic => enemic.GetComponent<EnemyArena>().spd).ToList();
        bool personatgeDins = false;
        for (int i = 0; i < OrdreJoc.Count; i++)
        {
            if (OrdreJoc[i].GetComponent<EnemyArena>().spd <= _JugadorSO.Spd)
            {
                OrdreJoc.Insert(i, _Jugador);
                personatgeDins = true;
                break;
            }
        }
        if (!personatgeDins)
            OrdreJoc.Insert(OrdreJoc.Count, _Jugador);
        BucleJoc();
    }

    public void BucleJoc()
    {
        for (int i = 0; i < OrdreJoc.Count; i++)
        {
            if (OrdreJoc[i].TryGetComponent<EnemyArena>(out EnemyArena e))
            {
                if (e.hp <= 0)
                {
                    OrdreJoc.Remove(OrdreJoc[i]);
                }
            }
        }
        //print($"{gameObject}/{this}: Nombre d'entitats a l'escena - {OrdreJoc.Count}");

        if (OrdreJoc.Count == 1 && OrdreJoc[0] == _Jugador)
            ChangeScene("Victoria");
        else
        {
            Debug.Log("Canvi de torn");
            GameObject aux = OrdreJoc[0];
            OrdreJoc.RemoveAt(0);
            OrdreJoc.Add(aux);
            //print("AUX: " + aux);
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
        //Pujar xp del jugador (PlayerCombat)
        if(escena.Equals("Victoria"))
        {
            int xp = (_NumEnemics*10) + UnityEngine.Random.Range(1, 10);
            _Jugador.GetComponent<PlayerCombat>().Xp += xp;
            _JugadorSO.Hp += UnityEngine.Random.Range(7, 16);
            _XpJugador.xpGuanyat = xp;
            if (_Jugador.GetComponent<PlayerCombat>().Xp >= _Jugador.GetComponent<PlayerCombat>().lvl * 20)
            {
                _Jugador.GetComponent<PlayerCombat>().lvlUP();
                _Jugador.GetComponent<PlayerCombat>().Xp = 0;
                GameManagerOW.Instance.lvlUP = true;
                foreach(EnemySO e in _Enemics)
                {
                    e.hp += 8;
                    e.def += 1;
                    foreach (AtacSO a in e.atk)
                    {
                        a.mal += 10;
                    }
                    e.spd += 2;
                }
            }
        }
        _Jugador.GetComponent<PlayerCombat>().SavePlayer();
        foreach (EnemySO e in _Enemics)
        {
            e.EstadosAlterados = null;
        }
        SceneManager.LoadScene(escena);
    }

    private void PlayerMort() => ChangeScene("GAMEOVER");
    private void PlayerFugir() => ChangeScene("Huir");

    public GameObject getJugador()
    {
        return _Jugador;
    }

    public List<GameObject> getEnemics()
    {
        return _EnemicsGOPantalla;
    }

    public void CanviaEnemicSelected(GameObject GO)
    {
        print("Canvi de seleccionat");
        foreach (GameObject go in _EnemicsGOPantalla)
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
