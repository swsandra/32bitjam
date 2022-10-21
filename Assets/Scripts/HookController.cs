using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float horizontalSpeed = 10f;
    [SerializeField]
    float verticalSpeed = 10f;
    [SerializeField]
    Camera cam;
    [SerializeField]
    Transform seaTop, seaBottom;
    Vector3 screenBounds;
    float hookWidth, hookHeight, hookTranslation, camTranslation, camXOffset;

    private void Start() {
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        camXOffset = cam.transform.position.x;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        hookWidth = renderer.bounds.size.x;
        hookHeight = renderer.bounds.size.y;
    }

    void Update()
    {
        hookTranslation = Input.GetAxisRaw("Horizontal");
        camTranslation = Input.GetAxisRaw("Vertical");

        Vector3 hookMovement = new Vector3(hookTranslation, 0, 0).normalized * horizontalSpeed * Time.deltaTime;
        Vector3 camMovement = new Vector3(0, camTranslation, 0).normalized * verticalSpeed * Time.deltaTime;

        transform.position += hookMovement;
        cam.transform.position += camMovement;

        // Hook movement bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, screenBounds.x+hookWidth, (screenBounds.x*-1)+(camXOffset*2)-hookWidth),
            transform.position.y,
            transform.position.z);

        // Camera movement bounds
        cam.transform.position = new Vector3(
            cam.transform.position.x,
            Mathf.Clamp(cam.transform.position.y, seaBottom.position.y+(hookHeight*2), seaTop.position.y-hookHeight),
            cam.transform.position.z);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Fish")){
            Debug.Log("choco contra un pez");
        }else if (other.CompareTag("Treasure")){
            Debug.Log("toco el tesoro");
        }
    }
}
