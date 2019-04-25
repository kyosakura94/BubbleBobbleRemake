using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    CanvasGroup cg;
    GameObject myGO;

    Canvas myCanvas;
    public Button btnPause;
    public Button btnquitGame;
    public Button btnRestart;
    public Button btnStartOver;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject startOverPanel;

    Slider volumeSlider;
    Toggle volumeMute;

    GameObject go;

    // Use this for initialization
    void Start()
    {

        go = GameObject.FindWithTag("Player");

        cg = GetComponent<CanvasGroup>();

        if (!cg)

            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0.0f;

        if (btnPause)
            btnPause.onClick.AddListener(PauseGame);

        if (btnquitGame)
            btnquitGame.onClick.AddListener(QuitGame);

        if (btnRestart)
            btnRestart.onClick.AddListener(Restart);

        if (btnStartOver)
            btnRestart.onClick.AddListener(BackToMenu);

        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startOverPanel.SetActive(false);

        volumeSlider = pausePanel.GetComponentInChildren<Slider>();
        volumeMute = pausePanel.GetComponentInChildren<Toggle>();

        if (volumeSlider)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (volumeMute)
        {
            bool value;

            if (PlayerPrefs.GetFloat("VolumeMute") == 0)
            {
                value = true;
            }
            else
            {
                value = false;
            }

            volumeMute.isOn = value;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            PauseGame();
        }

        if (GameManager.instance.gm == GameState.GameOver)
        {
            Debug.Log("END GAME");
            Gameover();
        }

        if (GameManager.instance.gm == GameState.Victory)
        {
            StartOver();
        }
    }

    public void BackToMenu() {
        GameManager.instance.LoadLevel("Screen_Menu");
    }
    public void PauseGame()
    {

        if (cg.alpha == 0.0f)
        {
            Debug.Log("Active");
            cg.alpha = 1.0f;
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
    public void Gameover()
    {
        FindObjectOfType<AudioManager>().Play("Gameovermusic");
        FindObjectOfType<AudioManager>().Stop("PlayingMusic");

        cg.alpha = 1.0f;
            gameOverPanel.SetActive(true);
            StartCoroutine(Delay());

        GameManager.instance.gm = GameState.NullState;
    }
    public void StartOver()
    {
        FindObjectOfType<AudioManager>().Play("TileMusic");
        FindObjectOfType<AudioManager>().Stop("Gameovermusic");
        FindObjectOfType<AudioManager>().Stop("PlayingMusic");

        GameManager.instance.LoadLevel("Screen_Menu");
        cg.alpha = 1.0f;
            startOverPanel.SetActive(true);
            StartCoroutine(Delay());

        GameManager.instance.gm = GameState.NullState;
        GameManager.instance.currentlevel = 0;
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);
        GameManager.instance.SpawnPlayer();

        GameManager.instance.gm = GameState.LevelSetup;

        FindObjectOfType<AudioManager>().Play("PlayingMusic");

        if (cg.alpha == 0.0f)
        {
            Debug.Log("Active");
            cg.alpha = 1.0f;
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;

        }

    }

    public void RestartWhenPlayerNotDie()
    {
        pausePanel.SetActive(false);

        GameManager.instance.clearenemylist();

        GameManager.instance.SpawnPlayer();

        GameManager.instance.gm = GameState.PlayerPrepare;

        if (cg.alpha == 0.0f)
        {
            Debug.Log("Active");
            cg.alpha = 1.0f;
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;

        }

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        Time.timeScale = 0.0f;

    }
}
