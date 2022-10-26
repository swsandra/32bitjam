using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] int lives;
    [SerializeField] float startingY;
    [SerializeField] Vector3 lastPosition;
    [Header("Treasures")]
    [SerializeField] List<string> treasuresCollected;
    [Header("Scenes")]
    [SerializeField] string menu;
    [SerializeField] string level1;
    [SerializeField] string level2;
    [SerializeField] string level3;
    [SerializeField] string hook;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals(level1) || sceneName.Equals(level2) || sceneName.Equals(level3)) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void LoadHookScene() {
        SceneManager.LoadScene(hook);
    }
}
