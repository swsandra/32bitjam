using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField]
    float hookSpeed = 10f;
    [SerializeField]
    float camSpeed = 10f;
    [SerializeField]
    Camera cam;

    Vector3 screenBounds;
    float hookWidth;

    float hookTranslation, camTranslation, camXOffset;

    private void Start() {
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        camXOffset = cam.transform.position.x;
        hookWidth = GetComponent<MeshRenderer>().bounds.size.x;
    }

    void Update()
    {
        hookTranslation = Input.GetAxisRaw("Horizontal");
        camTranslation = Input.GetAxisRaw("Vertical");

        Vector3 hookMovement = new Vector3(hookTranslation, 0, 0).normalized * hookSpeed * Time.deltaTime;
        Vector3 camMovement = new Vector3(0, camTranslation, 0).normalized * camSpeed * Time.deltaTime;

        transform.position += hookMovement;
        cam.transform.position += camMovement;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, screenBounds.x+hookWidth, (screenBounds.x * -1)+(camXOffset*2)-hookWidth),
            transform.position.y,
            transform.position.z);
    }
}
