using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Movement")]
    public int direction; // Either 1 (right), or -1 (left)
    public float speed;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 30f;
    [SerializeField] float maxRotation = 45f;

    float leftLimit;
    float rightLimit;

    private void Start() {
        Camera cam = Camera.main;
        float camXOffset = cam.transform.position.x;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        leftLimit = screenBounds.x;
        rightLimit = (screenBounds.x*-1)+(camXOffset*2);

        foreach (Transform child in transform)
        {
            child.eulerAngles = new Vector3(child.eulerAngles.x, child.eulerAngles.y-(maxRotation/2), child.eulerAngles.z);
        }
    }

    private void Update() {
        transform.Translate(direction * Time.deltaTime * speed, 0, 0, Space.World);

        bool outOfScreen = direction > 0 ? transform.position.x > rightLimit : transform.position.x < leftLimit;
        if (outOfScreen){
            Destroy(gameObject, .1f);
        }
    }

    private void FixedUpdate() {
        float t = Mathf.PingPong(Time.time  * rotationSpeed, maxRotation);
        transform.eulerAngles = new Vector3(0, t, 0);
    }
}
