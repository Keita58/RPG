using UnityEngine;

public class ManaBar : MonoBehaviour
{
    private float restarBarra;
    private Vector3 escalaBarra;
    
    public void IniciarBarra(int manaJugador)
    {
        escalaBarra = transform.localScale;
        restarBarra = escalaBarra.x / manaJugador;
    }

    public void UpdateHealth(int mal)
    {
        this.transform.localScale -= new Vector3(restarBarra * mal, 0, 0);
    }

    public void RetornALaPull()
    {
        transform.localScale = escalaBarra;
    }
}
