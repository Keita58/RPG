using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool empty = true;
    GameObject actualItem = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (actualItem != null)
        {
            if (this.transform.position != this.actualItem.transform.position)
            {
                this.empty = true;
                this.actualItem = null;
                this.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collision"+collision.GetType().Name);
        if (collision.tag=="Item" && this.empty) {
           
            print("He entrado");
            if (!collision.GetComponent<DragObjecte>().drageando)
            {
                this.empty = false;
                //Destroy(this.actualItem.GetComponent<DragObjecte>());
                actualItem = collision.gameObject;
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

    }

}
