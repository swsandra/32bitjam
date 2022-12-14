using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Levels UI")]
    [SerializeField] TMP_Text treasuresText;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite emptyHeart;
    // [SerializeField] Sprite fullHeart;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameOverSelectBtn;

    [Header("Win")]
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject winScreenSelectBtn;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void UpdateLives(){
        if (GameManager.instance.maxLives == GameManager.instance.lives) return;
        int i = GameManager.instance.maxLives-1;
        while (i >= GameManager.instance.lives){
            hearts[i].sprite = emptyHeart;
            i--;
        }
    }

    public void UpdateTreasureCount(){
        int currentTreasures = GameObject.FindGameObjectsWithTag("Treasure").Length;
        treasuresText.text = (GameManager.instance.totalTreasures-currentTreasures).ToString()+"/"+GameManager.instance.totalTreasures.ToString();
    }

    private void Update() {
        UpdateTreasureCount();
    }

    [ContextMenu("Game Over")]
    public void ShowGameOverScreen(){
        KeyButtons buttonScript = GameObject.FindObjectOfType<KeyButtons>();
        buttonScript.OpenFrame(gameOverScreen);
        buttonScript.SetCurrentButton(gameOverSelectBtn);
    }

    [ContextMenu("Win")]
    public void ShowWinScreen(){
        KeyButtons buttonScript = GameObject.FindObjectOfType<KeyButtons>();
        buttonScript.OpenFrame(winScreen);
        buttonScript.SetCurrentButton(winScreenSelectBtn);
    }
}
