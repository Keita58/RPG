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
    [SerializeField] private AudioSource _MalMac;
    [SerializeField] private AudioSource _MortMac;

    [Header("Musica")]
    [SerializeField] private AudioSource _OWMusica;
    [SerializeField] private AudioSource _ArenaMusica;
    [SerializeField] private AudioSource _Menu;
    [SerializeField] private AudioSource _Victoria;
    [SerializeField] private AudioSource _XP;
    [SerializeField] private AudioSource _GameOver;
    [SerializeField] private AudioSource _Fugir;

    public void AtacJugador()
    {
        _Atac.Play();
    }

    public void MalJugador()
    {
        _Mal.Play();
    }

    public void MortJugador()
    {
        _Mort.Play();
    }

    public void AtacEspadatxi()
    {
        int numAtac = Random.Range(1, 3);
        switch (numAtac)
        {
            case 1:
                _AtacEspadatxi.Play();
                break;
            case 2:
                _AtacEspadatxi2.Play();
                break;
        }
    }

    public void MalEspadatxi()
    {
        _MalEspadatxi.Play();
    }

    public void MortEspadatxi()
    {
        _MortEspadatxi.Play();
    }

    public void AtacGoblin()
    {
        int numAtac = Random.Range(1, 4);
        switch (numAtac)
        {
            case 1:
                _AtacGoblin.Play();
                break;
            case 2:
                _AtacGoblin2.Play();
                break;
            case 3:
                _AtacGoblin3.Play();
                break;
        }
    }

    public void MalGoblin()
    {
        _MalGoblin.Play();
    }

    public void MortGoblin()
    {
        _MortGoblin.Play();
    }

    public void AtacMac()
    {
        _AtacMac.Play();
    }

    public void MalMac()
    {
        _MalMac.Play();
    }

    public void MortMac()
    {
        _MortMac.Play();
    }
}
