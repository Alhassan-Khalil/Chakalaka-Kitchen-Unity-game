using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private const string PLAYER_PREF_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager instance { get; private set; }
    private AudioSource musicSource;


    private float volume = 0.3f;
    private void Awake()
    {
        instance = this;
        musicSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREF_MUSIC_VOLUME, 0.3f);
        musicSource.volume = volume;



    }
    private void Start()
    {
    }
    public void ChangeVolume(float v)
    {
        musicSource.volume = v;

        PlayerPrefs.SetFloat(PLAYER_PREF_MUSIC_VOLUME , v);
        PlayerPrefs.Save();
    }

    public float GetVolume() => musicSource.volume;
}
