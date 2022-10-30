using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    TMP_Text loadingText;
    [Header("Tips")]
    [SerializeField]
    TMP_Text tip;
    [SerializeField]
    string[] tipsList;
    [Header("Boat")]
    [SerializeField]
    Transform boat;
    [SerializeField]
    float boatRotationSpeed;
    [Header("LoadingBar")]
    [SerializeField]
    RectTransform bar;
    [SerializeField]
    float barFillingSpeed;
    [SerializeField]
    string sceneToLoad;
    float maxBarSize;

    // Start is called before the first frame update
    void Start()
    {
        sceneToLoad = GameManager.instance.nextScene;
        maxBarSize = bar.localScale.x;
        bar.localScale = new Vector3(0, bar.localScale.y, bar.localScale.z);
        ShuffleTipsArray();
        StartCoroutine(textLoading());
        StartCoroutine(changeTip());
    }

    void ShuffleTipsArray() {
        string tempGO;
        for (int i = 0; i < tipsList.Length; i++) {
            int rnd = Random.Range(0, tipsList.Length);
            tempGO = tipsList[rnd];
            tipsList[rnd] = tipsList[i];
            tipsList[i] = tempGO;
        }
    }

    IEnumerator textLoading() {
        while (true){
            loadingText.text = "Loading";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading .";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading . .";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading . . .";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator changeTip() {
        foreach (string newTip in tipsList)
        {
            tip.text = newTip;
            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale += new Vector3(Time.deltaTime * barFillingSpeed, 0, 0);
        boat.Rotate(new Vector3(0, boatRotationSpeed * Time.deltaTime, 0));
        if (bar.localScale.x >= maxBarSize) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
