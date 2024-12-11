using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Jugador")]
    [SerializeField] private AudioSource _Atac;
    [SerializeField] private AudioSource _Atac2;
    [SerializeField] private AudioSource _Mal;
    [SerializeField] private AudioSource _Mort;

    [Header("Enemic-Espadatxi")]
    [SerializeField] private AudioSource _AtacEspadatxi;
    [SerializeField] private AudioSource _AtacEspadatxi2;
    [SerializeField] private AudioSource _AtacEspadatxi3;
    [SerializeField] private AudioSource _MalEspadatxi;
    [SerializeField] private AudioSource _MortEspadatxi;

    [Header("Enemic-Goblin")]
    [SerializeField] private AudioSource _AtacGoblin;
    [SerializeField] private AudioSource _AtacGoblin2;
    [SerializeField] private AudioSource _AtacGoblin3;
    [SerializeField] private AudioSource _MalGoblin;
    [SerializeField] private AudioSource _MortGoblin;

    [Header("Enemic-Mac")]
    [SerializeField] private AudioSource _AtacMac;
    [SerializeField] private AudioSource _AtacMac2;
    [SerializeField] private AudioSource _AtacMac3;
    [SerializeField] private AudioSource _MalMac;
    [SerializeField] private AudioSource _MortMac;

    [Header("Musica")]
    [SerializeField] private AudioSource _OWMusica;
    [SerializeField] private AudioSource _ArenaMusica;

    public void AtacJugador()
    {
        _Atac.Play();
    }

    public void AtacEspadatxi()
    {

    }

    public void AtacGoblin()
    {

    }

    public void AtacMac()
    {

    }
}
