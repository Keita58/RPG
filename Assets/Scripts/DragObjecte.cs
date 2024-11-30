using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class DragObjecte : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] List<GameObject> InventariMap;
    [SerializeField] GameObject panel;
    public bool drageando = false;
    public GameObject cellCollider;
    int quantity = 1;
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


    public void OnPointerDown(PointerEventData eventData)
    {
        if(!this.panel.activeSelf)
            this.panel.SetActive(true);
        else
            this.panel.SetActive(false);
        print("aaas");    
        this.drageando = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drageando = false;
        if (this.cellCollider != null)
        {
            cellCollider.GetComponent<Cell>().empty = true;
            cellCollider.GetComponent<Collider2D>().enabled = true;

        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        this.panel.SetActive(false);
        Vector3 mousePos = eventData.position;
        //print("Posicio: " + mousePos);
        // print("Em moc");
        this.transform.position = eventData.pointerCurrentRaycast.worldPosition;// new Vector3(mousePos.x, mousePos.y, this.transform.position.z);
            //transform.position = new Vector3(InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, InventariMap.WorldToCell((Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition)).y, this.transform.position.z);
    }

}