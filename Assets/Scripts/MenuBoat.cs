using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoat : MonoBehaviour
{
    [Header("Buoyancy")]
    [SerializeField] float buoyancySpeed;
    [SerializeField] float buoyancyRotationSpeed;
    [SerializeField] float maxBuoyancy;
    [SerializeField] float maxBuoyancyRotation;
    [SerializeField] float maxRotationTilt;

    private void FixedUpdate() {
        // Buoyancy
        float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);
        float buoyancyRotation = Mathf.PingPong(Time.time * buoyancyRotationSpeed, maxBuoyancyRotation) - (maxBuoyancyRotation/2);

        float angleB = transform.localEulerAngles.z;
        angleB = (angleB > 180) ? angleB - 360 : angleB;
        transform.eulerAngles = new Vector3(buoyancyRotation,transform.eulerAngles.y,Mathf.Clamp(angleB,-maxRotationTilt,maxRotationTilt));
    }
}
