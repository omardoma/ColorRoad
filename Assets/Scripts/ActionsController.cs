using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionsController : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        PlayerPrefs.DeleteKey("Sound");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
