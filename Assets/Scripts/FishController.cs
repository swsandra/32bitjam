using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxRotation;

    private void FixedUpdate() {
        float t = Mathf.PingPong(Time.time  * rotationSpeed, maxRotation);
        transform.eulerAngles = new Vector3(0, t, 0);
    }
}
