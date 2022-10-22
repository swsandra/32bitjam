using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float accelerationSpeed;
    [SerializeField] float maxSpeed;
    [Header("Rotation")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float tiltSpeed;
    [SerializeField] float maxRotationTilt;
    [Header("Buoyancy")]
    [SerializeField] float buoyancySpeed;
    [SerializeField] float buoyancyRotationSpeed;
    [SerializeField] float maxBuoyancy;
    [SerializeField] float maxBuoyancyRotation;
    float startingY;
    
    Rigidbody rb;
    float speed;
    float movementVertical;
    float movementHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        movementVertical = Input.GetAxis("Vertical");
        movementHorizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        // Boat rotation
        float rotation = rotationSpeed*movementHorizontal;
        transform.Rotate(new Vector3(0,rotation,0),Space.World);

        // Rotation tilt
        float tilt = tiltSpeed*movementHorizontal;
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

        // Boat movement
        speed = Mathf.Clamp(speed + movementVertical, 0, maxSpeed);
        rb.velocity = transform.forward * speed;

        // Buoyancy
        float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);
        float buoyancyRotation = Mathf.PingPong(Time.time * buoyancyRotationSpeed, maxBuoyancyRotation) - (maxBuoyancyRotation/2);

        transform.position = new Vector3(transform.position.x, startingY + buoyancyMovement, transform.position.z);

        angle = transform.localEulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        transform.eulerAngles = new Vector3(buoyancyRotation,transform.eulerAngles.y,Mathf.Clamp(angle,-maxRotationTilt,maxRotationTilt));
    }
}
