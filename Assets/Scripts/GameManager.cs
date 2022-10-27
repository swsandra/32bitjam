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
    public Vector3 startingPosition;
    public Vector3 lastPosition;
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

    private void Start() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals(level1) || sceneName.Equals(level2) || sceneName.Equals(level3)) {
            player = GameObject.FindGameObjectWithTag("Player");
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

    public void Respawn() {
        lives = maxLives;
        lastPosition = startingPosition;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
