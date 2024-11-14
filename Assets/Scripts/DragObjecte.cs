using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DragObjecte : MonoBehaviour
{
    [SerializeField] List<GameObject> InventariMap;
    
    public void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (((mousePos.x < 9 && mousePos.x > -9) && (mousePos.y < 5 && mousePos.y > -5)))
        {
            print("Posicio: " + mousePos);
            print("Em moc");
            this.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            //transform.position = new Vector3(InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).y, this.transform.position.z);
        }
    }
}
