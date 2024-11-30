using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManagerOW : MonoBehaviour
{
    [SerializeField] public GameObject Jugador { get; set; }
    [SerializeField] public PlayerSO JugadorSO;

    public static GameManagerOW Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
