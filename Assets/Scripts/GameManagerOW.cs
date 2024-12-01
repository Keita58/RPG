using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class GameManagerOW : MonoBehaviour
{
    [SerializeField] public GameObject Jugador;
    [SerializeField] public GameObject Enemic;
    [SerializeField] public PlayerSO JugadorSO;
    [SerializeField] List<GameObject> ListEnemics;
    bool lvlUP;

    public static GameManagerOW Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);
        lvlUP = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Overworld":
                GameObject jugador = Instantiate(Jugador);
                break;
            case "LVLUP":
                StartCoroutine(canviaEscena());
                break;
        }
    }

    IEnumerator canviaEscena()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Overworld");
    }
}
