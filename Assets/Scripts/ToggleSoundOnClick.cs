using UnityEngine;

public class ToggleSoundOnClick : MonoBehaviour
{
    public AudioSource source;

    public void ToggleSound()
    {
        if (source.isPlaying)
        {
            source.Pause();
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            source.Play();
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Sound");
    }
}
