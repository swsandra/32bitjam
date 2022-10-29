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

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void UpdateLives(){
        if (GameManager.instance.maxLives == GameManager.instance.lives) return;
        hearts[GameManager.instance.lives].sprite = emptyHeart;
    }

    public void UpdateTreasureCount(){
        // treasuresText.text = currentTreasures.ToString()+"/"+totalTreasures.ToString();
    }
}
