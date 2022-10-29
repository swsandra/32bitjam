using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyButtons : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] GameObject lastScreenBtn;

    [Header("Frame")]
    [SerializeField] GameObject descriptionFrame;
    GameObject lastselect;

    void Start()
    {
        lastselect = new GameObject();
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

    public void SelectLevel2(GameObject descriptionParent){
        // TODO: deshabilitar si no ha pasado los niveles anteriores
        OpenFrame(descriptionParent);
    }

    public void LoadLevel2(){
        GameManager.instance.LoadLevel2();
    }

    public void SelectLevel3(GameObject descriptionParent){
        // TODO: deshabilitar si no ha pasado los niveles anteriores
        OpenFrame(descriptionParent);
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

}
