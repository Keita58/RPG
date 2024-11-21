using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerSO _Jugador;
    [SerializeField] List<EnemySO> _Enemics;
    private Queue<IAttack> PilaEnemics = new Queue<IAttack>();
    public event System.Action ActivarAtac;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case "Overworld":
                
                break;
            case "Arena":
                /*PilaEnemics = from enemic in _Enemics
                              orderby enemic.spd descending
                              select enemic;
                int numEnemics = Random.Range(0, _Enemics.Count);

                for(int i = 0; i < numEnemics; i++)
                {
                    _Enemics[i].SetActive(true);
                }*/
                break;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        switch (scene.name)
        {
            case "Overworld":

                break;
            case "Arena":

                break;
        }
    }
}
