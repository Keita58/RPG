using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class GameManagerOW : MonoBehaviour
{
    [SerializeField] public GameObject Jugador;
    [SerializeField] public GameObject Enemic;
    [SerializeField] public PlayerSO JugadorSO;
    [SerializeField] List<GameObject> ListEnemics;

    public static GameManagerOW Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Overworld":
                GameObject jugador = Instantiate(Jugador);

                break;
            case "INICIO":

                break;
        }
    }
}
