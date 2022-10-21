using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 30f;
    [SerializeField] float maxRotation = 45f;

    private void FixedUpdate() {
        float t = Mathf.PingPong(Time.time  * rotationSpeed, maxRotation);
        transform.eulerAngles = new Vector3(0, t, 0);
    }
}
