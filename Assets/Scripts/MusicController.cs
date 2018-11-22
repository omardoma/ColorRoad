using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;
    private float gameMusicPauseSecs;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioSource source;
    public bool muted;

    public static MusicController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        muted = PlayerPrefs.HasKey("Sound") && PlayerPrefs.GetInt("Sound") == 0;

        if (!muted)
        {
            PlayGameMusic();
        }
    }

    public void PlayGameMusic()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
        source.clip = gameMusic;
        source.time = gameMusicPauseSecs;
        source.Play();
    }

    public void PlayMenuMusic()
    {
        if (source.isPlaying)
        {
            gameMusicPauseSecs = source.time;
            source.Stop();
        }
        source.PlayOneShot(menuMusic);
    }
}
