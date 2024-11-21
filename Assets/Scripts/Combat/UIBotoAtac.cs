using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBotoAtac : MonoBehaviour
{
    [SerializeField] AtacSO atacSO;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button boto;
    UIMenuSeleccionarMagia menuSeleccionarMagia;

    public void Inicialitzar(AtacSO atac, UIMenuSeleccionarMagia menu)
    {
        gameObject.SetActive(true);
        atacSO = atac;
        menuSeleccionarMagia = menu;
        text.text = atac.nom;
    }

    public void Ocultar()
    {
        gameObject.SetActive(false);
    }

    public void UtilitzaHabilitat()
    {
        menuSeleccionarMagia.UtilitzaHabilitat(atacSO);
    }
}
