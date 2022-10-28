using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player")]
    public int maxLives;
    public int lives;
    public float startingY;
    public Vector3 lastPosition;
    public Vector3 startingPosition;
    [SerializeField] Vector3 startingPosition1;
    [SerializeField] Vector3 startingPosition2;
    [SerializeField] Vector3 startingPosition3;
    GameObject player;
    [Header("Treasures")]
    public List<string> treasuresCollected;
    public string treasureType;
    [Header("Scenes")]
    [SerializeField] string menu;
    [SerializeField] string level1;
    [SerializeField] string level2;
    [SerializeField] string level3;
    [SerializeField] string hook1;
    [SerializeField] string hook2;
    [SerializeField] string hook3;
    [SerializeField] string loading;
    public string nextScene;


    private void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void LoadHookSceneFromLevel(string nextTreasureType, string treasureName) {
        lastPosition = player.transform.position;
        startingY = startingPosition.y;
        treasureType = nextTreasureType;
        treasuresCollected.Add(treasureName);
        
        // Dictionary(?)
        if (SceneManager.GetActiveScene().name.Equals(level1)) {
            nextScene = hook1;
        }
        else if (SceneManager.GetActiveScene().name.Equals(level2)) {
            nextScene = hook2;
        }
        else if (SceneManager.GetActiveScene().name.Equals(level3)) {
            nextScene = hook3;
        }
        SceneManager.LoadScene(loading);
    }

    public void LoadLevelSceneFromHook() {

        // Dictionary(?)
        if (SceneManager.GetActiveScene().name.Equals(hook1)) {
            nextScene = level1;
        }
        else if (SceneManager.GetActiveScene().name.Equals(hook2)) {
            nextScene = level2;
        }
        else if (SceneManager.GetActiveScene().name.Equals(hook3)) {
            nextScene = level3;
        }
        SceneManager.LoadScene(loading);
    }

    public void LoadLevel1() {
        treasuresCollected = new List<string>();
        startingPosition = startingPosition1;

        nextScene = level1;
        SceneManager.LoadScene(loading);
    }

    public void LoadLevel2() {
        treasuresCollected = new List<string>();
        startingPosition = startingPosition2;

        nextScene = level2;
        SceneManager.LoadScene(loading);
    }

    public void LoadLevel3() {
        treasuresCollected = new List<string>();
        startingPosition = startingPosition3;

        nextScene = level3;
        SceneManager.LoadScene(loading);
    }

    public void Respawn() {
        lives = maxLives;
        lastPosition = startingPosition;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
    //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
    //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(level1) || scene.name.Equals(level2) || scene.name.Equals(level3)) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
