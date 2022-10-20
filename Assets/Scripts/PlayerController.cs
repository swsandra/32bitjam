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
    float buoyancyDirection = 1;
    static float t = 0f;
    
    Rigidbody rb;
    float speed;
    float movementVertical;
    float movementHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        // Boat Buoyancy
        float buoyancyMovement = Mathf.Lerp(-maxBuoyancy, maxBuoyancy, t) * buoyancyDirection;
        t += buoyancySpeed * Time.fixedDeltaTime;
        if (t > 1.0f) {
            buoyancyDirection *= -1;
            t = 0f;
        }

        transform.position = new Vector3(transform.position.x, buoyancyMovement, transform.position.z);
    }
}
