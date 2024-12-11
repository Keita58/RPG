using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Inventari : MonoBehaviour
{
    [SerializeField] GameObject Botons;
    [SerializeField] GameObject Dibuix;
    [SerializeField] Sprite TancaMenu;
    [SerializeField] Sprite ObreMenu;
    [SerializeField] TextMeshProUGUI LvlPersonatge;
    [SerializeField] PlayerSO PersonatgeSO;

    bool a = false;

    private void Start()
    {
        LvlPersonatge.text = "Lvl. " + PersonatgeSO.Lvl;
    }

    public void mostraInventari()
    {
        if (Botons.activeSelf)
        {
            Botons.SetActive(false);
            Dibuix.GetComponent<Image>().sprite = ObreMenu;
        }
        else
        {
            Botons.SetActive(true);
            Dibuix.GetComponent<Image>().sprite = TancaMenu;
        }
    }

    public void Pause()
    {
        if (a)
        {
            a = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            a = true;
            Time.timeScale = 0f;
        }
    }
}
