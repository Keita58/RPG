using TMPro;
using UnityEngine;

public class ManagerLvlAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] PlayerSO playerSO;
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        switch (playerSO.Lvl)
        {
            case 2:
                text.text = "Has guanyat l'atac Cop de Falç!";
                break;
            case 5:
                text.text = "Has guanyat l'atac Shadowflame!";
                break;
            case 10:
                text.text = "Has guanyat l'atac Psicosis!";
                break;
            case 15:
                text.text = "Has guanyat l'atac Armagedon!";
                break;
        }
    }
}
