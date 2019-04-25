using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Script : MonoBehaviour
{
    public Button btnStart;
    public Button btnQuuit;
    public Button btnRestart;

    // Use this for initialization
    void Start()
    {
        if (GameManager.instance && btnStart)
                btnStart.onClick.AddListener(GameManager.instance.StartGame);

        if (GameManager.instance && btnQuuit)
                btnQuuit.onClick.AddListener(GameManager.instance.QuitGame);

        if (GameManager.instance && btnRestart)
                btnRestart.onClick.AddListener(GameManager.instance.Restart);
    }
    private void Update()
    {
        if (GameManager.instance && btnStart)
            btnStart.onClick.AddListener(GameManager.instance.StartGame);

        if (GameManager.instance && btnQuuit)
            btnQuuit.onClick.AddListener(GameManager.instance.QuitGame);

        if (GameManager.instance && btnRestart)
            btnRestart.onClick.AddListener(GameManager.instance.Restart);
    }
}
