using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UITextos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] EnemyArena[] enemys;
    //Suscribirse a la funcion del enemigo
    void Start()
    {
        playerCombat.OnIniciarTornUI += InciTorn;
        playerCombat.OnRebreEstadoAlteradoUI += RebreEstats;
        playerCombat.OnRebreMalUI += RebreMal;
        playerCombat.OnEmpezarVentajaUI += EmpezarVentaja;
        playerCombat.OnSeleccionarTargetUI += SeleccionarTarget;
        playerCombat.OnSeleccionatTargetUI += DesactivarSeleccionarTarget;
        playerCombat.OnFallarHuirUI += FallarHuida;

        foreach (EnemyArena enemy in enemys)
        {
            enemy.OnIniciarTornUI += InciTorn;
            enemy.OnRebreEstadoAlteradoUI += RebreEstats;
            enemy.OnRebreMalUI += RebreMal;
            enemy.OnEmpezarVentajaUI += EmpezarVentaja;
        }

    }

    IEnumerator EsperarIActuar(float tempsDespera, Action accio)
    {
        if (tempsDespera > 0)
            yield return new WaitForSeconds(tempsDespera);
        accio();
    }

    private void EmpezarVentaja(string nom)
    {
      text.text=nom+ " comença amb avantatge!";
      text.gameObject.SetActive(true);
      StartCoroutine(EsperarIActuar(3, DesctivarTexto));
    }

    private void FallarHuida()
    {
        text.text= "Els enemics t'han atrapat!";
        text.gameObject.SetActive(true);
        StartCoroutine(EsperarIActuar(3, DesctivarTexto));
    }

    private void SeleccionarTarget()
    {
        text.text= "Has de seleccionar un target!";
        text.gameObject.SetActive(true);
    }

    private void RebreMal(string nom, int dmg)
    {
        text.text = nom + " ha rebut " + dmg + " punts de dany!";
        text.gameObject.SetActive(true);
        StartCoroutine(EsperarIActuar(3, DesctivarTexto));

    }

    private void RebreEstats(string nom, string nomEstat)
    {
        text.text=nom+ " pateix un estat alterat: "+nomEstat+"!";
        text.gameObject.SetActive(true);
        StartCoroutine(EsperarIActuar(3, DesctivarTexto));
    }

    public void InciTorn(string nom)
    {
        text.text = "És el torn de "+nom+"!";
        text.gameObject.SetActive(true);
        StartCoroutine(EsperarIActuar(3, DesctivarTexto));
    }

    public void NoMana()
    {
        text.text = "No tens suficient mana!";
        StartCoroutine(EsperarIActuar(3, DesctivarTexto));
    }

    public void DesactivarSeleccionarTarget()
    {
        text.gameObject.SetActive(false);
    }

    public void DesctivarTexto()
    {
        text.gameObject.SetActive(false);
    }


}
