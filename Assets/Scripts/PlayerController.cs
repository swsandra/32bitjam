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
    [SerializeField] float maxRotationTilt;
    [Header("Buoyancy")]
    [SerializeField] float buoyancySpeed;
    [SerializeField] float maxBuoyancy;
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
        if (Input.GetAxis("Vertical") > 0) {
            // Acelera hacia adelante
        }
        else if (Input.GetAxis("Vertical") < 0) {
            // frena (va hacia atras?)
        }
        if (Input.GetAxis("Horizontal") > 0) {
            // rota hacia la derecha
        }
        else if (Input.GetAxis("Horizontal") < 0) {
            // rota hacia la izquierda
        }

        movementVertical = Input.GetAxis("Vertical");
        movementHorizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        // Boat rotation
        float rotation = rotationSpeed*movementHorizontal;
        rb.rotation *= Quaternion.AngleAxis(rotation, Vector3.up);

        // Rotation tilt
        //rb.MoveRotation(Quaternion.Euler(0, 0, movementHorizontal*maxRotationTilt));

        // Boat movement
        speed = Mathf.Clamp(speed + movementVertical, 0, maxSpeed);
        rb.velocity = transform.forward * speed;

        // Buoyancy
        float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);

        transform.position = new Vector3(transform.position.x, startingY + buoyancyMovement, transform.position.z);
    }
}
