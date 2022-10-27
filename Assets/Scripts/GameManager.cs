using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player")]
    public int lives;
    public float startingY;
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

    public void LoadHookSceneFromLevel(string nextTreasureType) {
        lastPosition = player.transform.position;
        startingY = player.GetComponent<PlayerController>().startingY;
        treasureType = nextTreasureType;
        
        // Dictionary(?)
        if (SceneManager.GetActiveScene().name.Equals(level1)) {
            SceneManager.LoadScene(hook1);
        }
        else if (SceneManager.GetActiveScene().name.Equals(level2)) {
            SceneManager.LoadScene(hook2);
        }
        else if (SceneManager.GetActiveScene().name.Equals(level3)) {
            SceneManager.LoadScene(hook3);
        }
    }

    public void LoadLevelSceneFromHook() {
        treasureType = null;

        // Dictionary(?)
        if (SceneManager.GetActiveScene().name.Equals(hook1)) {
            SceneManager.LoadScene(level1);
        }
        else if (SceneManager.GetActiveScene().name.Equals(hook2)) {
            SceneManager.LoadScene(level2);
        }
        else if (SceneManager.GetActiveScene().name.Equals(hook3)) {
            SceneManager.LoadScene(level3);
        }
    }
}
