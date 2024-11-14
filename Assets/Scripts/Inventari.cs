using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Inventari : MonoBehaviour
{
    [SerializeField] GameObject InventariGO;
    [SerializeField] GameObject Dibuix;
    [SerializeField] Sprite TancaMenu;
    [SerializeField] Sprite ObreMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mostraInventari()
    {
        if (InventariGO.activeSelf)
        {
            InventariGO.SetActive(false);
            Dibuix.GetComponent<Image>().sprite = ObreMenu;
        }
        else
        {
            InventariGO.SetActive(true);
            Dibuix.GetComponent<Image>().sprite = TancaMenu;
        }
    }

}
