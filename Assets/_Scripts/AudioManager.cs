using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundLoop;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip death;
    [SerializeField] private bool playMenuMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.clip = backgroundLoop;
        audioSource.spatialBlend = 0f;
    }

    private void OnEnable()
    {
        GameController.GameOver += OnGameOver;

        if (playMenuMusic)
        {
            PlayBackgroundLoop();
        }
        else if (GameController.IsInitialized && GameController.IsGameOver())
        {
            OnGameOver(GameController.GetGameOverReason());
        }
        else
        {
            PlayBackgroundLoop();
        }
    }

    private void OnDisable()
    {
        GameController.GameOver -= OnGameOver;
        StopBackgroundLoop();
    }

    private void OnGameOver(string reason)
    {
        StopBackgroundLoop();

        if (reason == "Got all coins")
        {
            PlayEffect(victory);
        }
        else if (reason == "Lost all lives" || reason == "Time ran out")
        {
            PlayEffect(death);
        }
    }

    private void PlayBackgroundLoop()
    {
        if (backgroundLoop == null || audioSource.isPlaying)
        {
            return;
        }

        audioSource.loop = true;
        audioSource.clip = backgroundLoop;
        audioSource.Play();
    }

    private void StopBackgroundLoop()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayEffect(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        audioSource.loop = false;
        audioSource.PlayOneShot(clip);
    }
}
