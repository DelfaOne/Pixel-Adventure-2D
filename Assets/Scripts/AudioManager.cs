using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Background Level Music")]
    public AudioClip[] levelBackgroundMusic;

    [Header("Audio Clip")]
    public AudioClip jump;
    public AudioClip collectItem;
    public AudioClip finish;
    public AudioClip death;
    public AudioClip trampoline;


    private int currentScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        int newScene = SceneManager.GetActiveScene().buildIndex;
        if (newScene != currentScene)
        {
            currentScene = newScene;
            PlayBackgroundMusic();
        }
    }

    private void PlayBackgroundMusic()
    {
        musicSource.clip = levelBackgroundMusic[currentScene - 1];
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}
