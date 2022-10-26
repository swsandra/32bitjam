using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDutchman : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] Transform player;
    [Header("Rotation")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float tiltSpeed;
    [SerializeField] float maxRotationTilt;
    [Header("Buoyancy")]
    [SerializeField] float buoyancySpeed;
    [SerializeField] float maxBuoyancy;
    float startingY;

    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Boat rotation
        Vector3 direction = player.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        float angleDiference = Vector3.SignedAngle(new Vector3(transform.forward.x, 0, transform.forward.z), direction, Vector3.up);
        if (Mathf.Abs(angleDiference) < 5) {
            angleDiference = 0;
        }
        angleDiference = Mathf.Clamp(angleDiference, -1, 1);
        float rotation = rotationSpeed*angleDiference;
        transform.Rotate(new Vector3(0,rotation,0),Space.World);

        // Boat movement
        transform.position += transform.forward * speed;

        // Rotation tilt
        float tilt = tiltSpeed*angleDiference;
        float angle = transform.localEulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        if(Mathf.Abs(angle) <= maxRotationTilt){
            transform.Rotate(new Vector3(0,0,-tilt), Space.Self);
        }

        if (tilt == 0) {
            transform.eulerAngles = Vector3.Lerp(
                new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, angle),
                new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0),
                Time.deltaTime * tiltSpeed
            );
        }

        // Buoyancy
        float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);

        transform.position = new Vector3(transform.position.x, startingY + buoyancyMovement, transform.position.z);

        float angleB = transform.localEulerAngles.z;
        angleB = (angleB > 180) ? angleB - 360 : angleB;

    }
}
