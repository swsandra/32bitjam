using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class KeyButtons : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] GameObject lastScreenBtn;
    [SerializeField] GameObject lvl2Btn;
    [SerializeField] GameObject lvl3Btn;

    [Header("Frame")]
    [SerializeField] GameObject descriptionFrame;
    GameObject lastselect;

    void Start()
    {
        lastselect = new GameObject();
        if (lvl2Btn && lvl3Btn){
            if (!GameManager.instance.completedLevel2) {
                DisableLvlBtn(lvl3Btn);
            }

            if (!GameManager.instance.completedLevel1) {
                DisableLvlBtn(lvl2Btn);
            }
        }
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null){
            EventSystem.current.SetSelectedGameObject(lastselect);
        } else {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void MenuBack(GameObject descriptionParent){
        descriptionParent.SetActive(false);
        descriptionFrame.SetActive(false);
        EventSystem.current.SetSelectedGameObject(lastScreenBtn);
    }

    public void ButtonPressed(GameObject btn){
        lastScreenBtn = btn;
    }

    public void SetCurrentButton(GameObject btn){
        EventSystem.current.SetSelectedGameObject(btn);
        lastselect = btn;
    }

    public void OpenFrame(GameObject descriptionParent){
        descriptionFrame.SetActive(true);
        descriptionParent.SetActive(true);
    }

    public void LoadLevel1(){
        GameManager.instance.LoadLevel1();
    }

    public void LoadLevel2(){
        GameManager.instance.LoadLevel2();
    }

    public void LoadLevel3(){
        GameManager.instance.LoadLevel3();
    }

    public void Menu(){
        GameManager.instance.LoadMenu();
    }

    public void Respawn(){
        GameManager.instance.Respawn();
    }

    public void Quit(){
        Application.Quit();
    }

    void DisableLvlBtn(GameObject btn){
        Button btnScript = btn.GetComponent<Button>();
        Button btnAbove = btnScript.navigation.selectOnUp.GetComponent<Button>();
        Button btnBelow = btnScript.navigation.selectOnDown.GetComponent<Button>();

        // Disable button
        btnScript.interactable = false;
        btn.GetComponentInChildren<TMP_Text>().faceColor = new Color32(0, 0, 0, 128);

        // Change button's navigation
        Navigation newNavAbove = new Navigation();
        newNavAbove.mode = Navigation.Mode.Explicit;
        newNavAbove.selectOnUp = btnAbove.navigation.selectOnUp;
        newNavAbove.selectOnDown = btnBelow;
        btnAbove.navigation = newNavAbove;

        Navigation newNavBelow = new Navigation();
        newNavBelow.mode = Navigation.Mode.Explicit;
        newNavBelow.selectOnUp = btnAbove;
        newNavBelow.selectOnDown = btnBelow.navigation.selectOnDown;
        btnBelow.navigation = newNavBelow;
    }

}
