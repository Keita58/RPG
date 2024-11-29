using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Inventari : MonoBehaviour
{
    [SerializeField] GameObject InventariGO;
    [SerializeField] GameObject Botons;
    [SerializeField] GameObject Dibuix;
    [SerializeField] Sprite TancaMenu;
    [SerializeField] Sprite ObreMenu;

    public void mostraInventari()
    {
        if (InventariGO.activeSelf)
        {
            InventariGO.SetActive(false);
            Botons.SetActive(false);
            Dibuix.GetComponent<Image>().sprite = ObreMenu;
        }
        else
        {
            InventariGO.SetActive(true);
            Botons.SetActive(true);
            Dibuix.GetComponent<Image>().sprite = TancaMenu;
        }
    }

}
