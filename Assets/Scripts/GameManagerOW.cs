using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerOW : MonoBehaviour
{
    

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
