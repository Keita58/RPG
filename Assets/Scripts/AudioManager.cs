using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private AudioSource _Atac;
    [SerializeField] private AudioSource _Atac2;
    [SerializeField] private AudioSource _Mort;

    [Header("Enemy")]
    [SerializeField] private AudioSource _AtacEnemic;
    [SerializeField] private AudioSource _AtacEnemic2;

    [Header("Music")]
    [SerializeField] private AudioSource _OWMusica;
    [SerializeField] private AudioSource _ArenaMusica;
}
