using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FugirEscena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        yield return new WaitForSeconds(tempsDespera);
        accio();
    }

    private void Start()
    {
        StartCoroutine(EsperarIActuar(5, () =>
        {
            SceneManager.LoadScene("Overworld");
        }));
    }


}
