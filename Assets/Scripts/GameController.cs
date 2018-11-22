using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    private PlayerController playerController;
    private SpawnController spawnController;
    private int scoreCheckpoint;
    public bool isGameOver;
    public bool isPaused;
    public GameObject player;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public Button menuButton;
    public Button cameraButton;
    public Text scoreText;

    public static GameController Instance
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

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        spawnController = GetComponent<SpawnController>();
        scoreText.text = "Score: " + playerController.score.ToString();
#if UNITY_ANDROID 
        menuButton.gameObject.SetActive(true);
        cameraButton.gameObject.SetActive(true);
#endif
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (!isPaused)
            {
                scoreText.text = "Score: " + playerController.score.ToString();

                if (!playerController.alive)
                {
                    GameOver();
                    return;
                }

                if (playerController.score - scoreCheckpoint >= 50)
                {
                    SpeedUpGame();
                }
                else if (playerController.score < scoreCheckpoint)
                {
                    scoreCheckpoint = playerController.score;
                }
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        scoreText.enabled = false;
        pausePanel.SetActive(true);
        if (!MusicController.Instance.muted)
        {
            MusicController.Instance.PlayMenuMusic();
        }
#if UNITY_ANDROID
        menuButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
#endif
    }

    private void GameOver()
    {
        isGameOver = true;
        scoreText.enabled = false;
        gameOverPanel.SetActive(true);
        if (!MusicController.Instance.muted)
        {
            MusicController.Instance.PlayMenuMusic();
        }
#if UNITY_ANDROID
        menuButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
#endif
    }

    private void ResumeGame()
    {
        isPaused = false;
        scoreText.enabled = true;
        pausePanel.SetActive(false);
        if (!MusicController.Instance.muted)
        {
            MusicController.Instance.PlayGameMusic();
        }
#if UNITY_ANDROID
        menuButton.gameObject.SetActive(true);
        cameraButton.gameObject.SetActive(true);
#endif
    }

    private void SpeedUpGame()
    {
        playerController.forwardSpeed *= 2;
        spawnController.sphereCycle = spawnController.sphereCycle > 0.125f ? spawnController.sphereCycle / 2 : 0.125f;
        spawnController.colorChangeCycle = spawnController.colorChangeCycle > 2.5 ? spawnController.colorChangeCycle / 2 : 2.5f;
        scoreCheckpoint = playerController.score;
    }
}
