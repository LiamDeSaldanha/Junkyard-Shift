using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Awake()
    {
        // Standard Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchToBossMusic()
    {
        StartCoroutine(FadeTrack(bossMusic));
    }

    public void SwitchToBackgroundMusic()
    {
        StartCoroutine(FadeTrack(backgroundMusic));
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        if (audioSource.clip == newClip) yield break;

        // Fade Out
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade In
        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}