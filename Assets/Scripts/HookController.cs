using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;

    [Header("Vertical Bounds")]
    [SerializeField] Transform seaTop;
    [SerializeField] Transform seaBottom;
    Camera cam;
    float leftLimit;
    float rightLimit;
    float hookWidth, hookHeight;
    float hookTranslation, camTranslation;

    private void Start() {
        cam = Camera.main;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        float camXOffset = cam.transform.position.x;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        hookWidth = renderer.bounds.size.x;
        hookHeight = renderer.bounds.size.y;
        leftLimit = screenBounds.x+hookWidth;
        rightLimit = (screenBounds.x*-1)+(camXOffset*2)-hookWidth;
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
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
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
