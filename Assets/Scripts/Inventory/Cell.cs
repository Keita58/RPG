using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Cell : MonoBehaviour
{
    public bool empty = true;
   // GameObject actualItem = null;
    int quantity = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(this.empty);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collision"+collision.GetType().Name);
        if (collision.tag=="Item" && this.empty) {
           
            print("He entrado");
            if (!collision.GetComponent<DragObjecte>().drageando)
            {
                this.empty = false;
                this.quantity = 1;
                //Destroy(this.actualItem.GetComponent<DragObjecte>());
                collision.transform.position = this.transform.position;
                this.GetComponent<Collider2D>().enabled = false;
                collision.GetComponent<DragObjecte>().cellCollider = this.gameObject;
            }

            if (collision == null && this.GetComponent<Collider2D>().isActiveAndEnabled)
            {
                this.empty = true;
            }
        }/*else if(collision.tag == "Item" && !this.empty)
        {
            if (!collision.GetComponent<DragObjecte>().drageando)
            {
                if (collision.gameObject.name == actualItem.name)
                {
                    this.quantity++;
                    Destroy(collision.gameObject);
                }
            }
        }*/
        print("Quantity: " + quantity);
    }

}
