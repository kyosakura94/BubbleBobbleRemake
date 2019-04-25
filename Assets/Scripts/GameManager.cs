using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState { NullState, MainMenu, Game, GameOver, Victory, Starting, CleanUp, LevelSetup, PlayerPrepare }

public class GameManager : MonoBehaviour
{

    static GameManager _instance = null;

    public List<GameObject> currentEnemy = new List<GameObject>();
    public List<GameObject> enemyPrefab = new List<GameObject>();

    public List<GameObject> enemyPosition = new List<GameObject>();

    public int currentlevel;
    public Transform enemyOldTransform;

    public GameObject level = null;
    public GameObject canvas_HUD;

    public GameObject startpoint;
    public GameObject camraPosition;
    public GameObject eSpawnPosition;
    public GameObject spawnLocation;

    public Transform firstspawnPlayer;

    GameObject player;

    int _lives;
    int currentLevel;

    GameState _gm = GameState.NullState;

    public bool isChangingLevel = false;


    Transform heartSpacer;
    Text lifeText;

    //for score
    Text txtScore;


    public GameObject playerPrefab;

    // Use this for initialization
    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
        {
            instance = this;

            DontDestroyOnLoad(this);
        }

        currentlevel = 0;

        lives = 3;

        gm = GameState.MainMenu;

    }

    // Update is called once per frame
    void Update()
    {
        TurnPhase();
    }

    void IgoreCollide()
    {
        Physics2D.IgnoreLayerCollision(11, 8, true);
        Physics2D.IgnoreLayerCollision(11, 12, true);
        Physics2D.IgnoreLayerCollision(11, 13, true);
        Physics2D.IgnoreLayerCollision(11, 14, true);
        Physics2D.IgnoreLayerCollision(11, 0, true);
    }

    void RestoreCollide()
    {
        Physics2D.IgnoreLayerCollision(11, 8, false);
        Physics2D.IgnoreLayerCollision(11, 12, false);
        Physics2D.IgnoreLayerCollision(11, 13, false);
        Physics2D.IgnoreLayerCollision(11, 14, false);
        Physics2D.IgnoreLayerCollision(11, 0, false);
    }

    void TurnPhase()
    {

        switch (gm)
        {

            case GameState.LevelSetup:
                clearenemylist();

                if (currentlevel >= 2 && currentEnemy.Count <= 0)
                {
                    //LoadLevel("Screen_Credit");
                    gm = GameState.Victory;
                    return;
                }
                else level = GameObject.FindGameObjectWithTag("Level_" + currentlevel);

                if (level == null)
                {
                    Debug.Log("level is null");
                }
                else
                {
                    levelSetUp();
                    CreateEnemy();
                    gm = GameState.Starting;
                } 

                break;

              
            case GameState.PlayerPrepare:

                clearenemylist();
                gm = GameState.LevelSetup;
                break;

            case GameState.Starting:

                isChangingLevel = false;
                IgoreCollide();

                break;

            case GameState.Game:

                RestoreCollide();
                if (currentEnemy.Count <= 0 && FindObjectOfType<Character>().isDead == false)
                {
                    
                    gm = GameState.CleanUp;
                    
                }

                break;

            case GameState.CleanUp:

                
                currentlevel += 1;
                enemyPosition.Clear();
                enemyPrefab.Clear();

                gm = GameState.LevelSetup;


                break;

            case GameState.GameOver:

                break;

            case GameState.MainMenu:

                break;

            case GameState.NullState:

                clearenemylist();

                break;

            default:
                Debug.LogError("THIS SHOULD NEVER HAPPEN.  Unknown TurnState.");
                break;
        }

    }

    public void clearenemylist()
    {

        enemyPosition.Clear();
        enemyPrefab.Clear();

        for (int i = 0; i < currentEnemy.Count; i++)
        {
            Destroy(currentEnemy[i]);
            Debug.Log("Clear all enemy?");
        }

        currentEnemy.Clear();

    }

    void updateEnemy()
    {


    }

    public void levelSetUp()
    {
        Transform parent = level.transform;

        foreach (Transform enemy in parent)
        {
            if (enemy.CompareTag("Startpoint"))
            {
                startpoint = enemy.gameObject;
            }
            if (enemy.CompareTag("ESpawn_pos"))
            {
                enemyPosition.Add(enemy.gameObject);
            }

            if (enemy.CompareTag("CamPos"))
            {
                camraPosition = enemy.gameObject;
            }

            if (enemy.CompareTag("SpawnPosition"))
            {
                spawnLocation = enemy.gameObject;
            }
        }

        if (currentlevel >= 2 && currentEnemy.Count <= 0)
        {
            gm = GameState.Victory;
            //LoadLevel("Screen_Credit");
        }
        else
        { 
            for (int i = 0; i < level.GetComponent<EnemyInLevel>().enemyPrefab.Count; i++)
            {
                Debug.Log("bug");

                enemyPrefab.Add(level.GetComponent<EnemyInLevel>().enemyPrefab[i]);
            }
        }
        for (int i = 0; i < enemyPosition.Count; i++)
        {
            GameObject go = Instantiate(enemyPrefab[i], enemyPosition[i].transform.position, Quaternion.identity);

            go.name = enemyPrefab[i].name;

            go.transform.parent = level.transform;
        }
    }

    public void CreateEnemy() {

        Transform parent = level.transform;

        foreach (Transform objects in parent)
        {
            if (objects.CompareTag("Enemy"))
            {
                Debug.Log(objects.name);
                currentEnemy.Add(objects.gameObject);
            }
        }
    }

    public void removeEnemy(GameObject enemy)
    {
        Debug.Log("One is removed" + enemy.name);

        currentEnemy.Remove(enemy.gameObject);
    }

    IEnumerator WaitThenChangeLevel()
    {
        yield return new WaitForSeconds(10.0f);
        gm = GameState.LevelSetup;

    }

    public void StartGame()
    {
        gm = GameState.LevelSetup;
        FindObjectOfType<AudioManager>().Stop("TileMusic");
        FindObjectOfType<AudioManager>().Play("PlayingMusic");

        SceneManager.LoadScene("Bubble_level_01");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Screen_Menu");
        gm = GameState.MainMenu;
        currentlevel = 0;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    public void SpawnPlayer()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("StartLevel"));

        if (playerPrefab && spawnLocation)
        {
            GameObject player = Instantiate(playerPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation);
            player.name = playerPrefab.name;
            player.GetComponent<Character>().lifeText = GameObject.Find("LifeText").GetComponent<UnityEngine.UI.Text>();
            player.GetComponent<Character>().txtScore = GameObject.Find("Txt_Score").GetComponent<UnityEngine.UI.Text>();
            player.GetComponent<Character>().heartSpacer = GameObject.Find("Heart_spacer").GetComponent<UnityEngine.Transform>();

            for (int i = 0; i < player.GetComponent<Character>().lifeHeart.Count; i++)
            {
                Destroy(player.GetComponent<Character>().lifeHeart[i]);
            }

            player.GetComponent<Character>().lifeHeart.Clear();
        }

    }

    public void FirstspawnPlayer(Transform firstSpawn)
    {
        if (playerPrefab && firstSpawn)
        {
            Debug.Log("Create Player??");
            GameObject player = Instantiate(playerPrefab.gameObject, firstSpawn.position, firstSpawn.rotation);
            player.name = playerPrefab.name;
            player.transform.parent = gameObject.transform;
            player.GetComponent<Character>().lifeText = GameObject.Find("LifeText").GetComponent<UnityEngine.UI.Text>();
            player.GetComponent<Character>().txtScore = GameObject.Find("Txt_Score").GetComponent<UnityEngine.UI.Text>();
            player.GetComponent<Character>().heartSpacer = GameObject.Find("Heart_spacer").GetComponent<UnityEngine.Transform>();
        }
    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public int lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            if (gm == GameState.Game)
            {
                if (_lives > 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else
                    Debug.Log("Player Dead");
            }
        }
    }

    public GameState gm
    {
        get { return _gm; }
        set
        {
            _gm = value;
            Debug.Log("Current State: " + _gm);
        }
    }
}
