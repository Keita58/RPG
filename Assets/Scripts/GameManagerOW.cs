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
    //[SerializeField] List<GameObject> ListEnemics;
    [SerializeField] HpMaxJugador HpMax;
    public EnemySO EnemicPrincipal;
    public bool lvlUP;

    public static GameManagerOW Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(gameObject);
        lvlUP = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
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
            case "Victoria":
                StartCoroutine(canviaEscena());
                break;
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        if (scene.name.Equals("INICIO"))
            HpMax.hpMax = JugadorSO.Hp;
    }

    IEnumerator canviaEscena()
    {
        yield return new WaitForSeconds(4);
        if (lvlUP)
        {
            lvlUP = false;
            SceneManager.LoadScene("LVLUP");
        }
        else
            SceneManager.LoadScene("Overworld");
    }
}
