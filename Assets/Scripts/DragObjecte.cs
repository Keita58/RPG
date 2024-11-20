using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class DragObjecte : MonoBehaviour
{
    [SerializeField] List<GameObject> InventariMap;
    public bool drageando = false;
    public GameObject cellCollider;
    int quantity = 1;
    private void OnMouseOver()
    {
        
    }
    private void OnMouseDown()
    {

    }
    public void OnMouseDrag()
    {
        print(quantity);

        drageando=true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (((mousePos.x < 9 && mousePos.x > -9) && (mousePos.y < 5 && mousePos.y > -5)))
        {
            //print("Posicio: " + mousePos);
           // print("Em moc");
            this.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            //transform.position = new Vector3(InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).y, this.transform.position.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(cellCollider);
        if (this.cellCollider != null)
        {
            if (collision.tag == "Item" && !this.cellCollider.GetComponent<Cell>().empty)
            {
                Destroy(collision.gameObject);
                this.quantity++;
            }
        }
    }
    private void OnMouseUp()
    {
        drageando = false;
        if (this.cellCollider != null)
        {
            cellCollider.GetComponent<Cell>().empty = true;
            cellCollider.GetComponent<Collider2D>().enabled = true;

        }
    }
}
