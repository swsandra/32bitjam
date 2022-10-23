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
    [Header("Collisions")]
    [SerializeField] float collisionForce;
    [SerializeField] float moveCooldown;
    
    Rigidbody rb;
    float speed;
    float movementVertical;
    float movementHorizontal;
    float startingY;
    bool canMove = true;

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

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Rock") {
            Debug.Log("CHOQUE CONTRA ROCK");
            Vector3 dir = other.GetContact(0).point - transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir*collisionForce);
            speed = 0f;

            // TODO damage boat

            canMove = false;
            StartCoroutine(moveRecharge());
        }
    }

    IEnumerator moveRecharge() {
        yield return new WaitForSeconds(moveCooldown);
        canMove = true;
    }

    private void FixedUpdate() {
        if (canMove) {
            // Boat rotation
            float rotation = rotationSpeed*movementHorizontal;
            transform.Rotate(new Vector3(0,rotation,0),Space.World);

            // Boat movement
            speed = Mathf.Clamp(speed + movementVertical, 0, maxSpeed);
            rb.velocity = transform.forward * speed;
            
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
        }
        // Buoyancy
        float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);
        float buoyancyRotation = Mathf.PingPong(Time.time * buoyancyRotationSpeed, maxBuoyancyRotation) - (maxBuoyancyRotation/2);

        transform.position = new Vector3(transform.position.x, startingY + buoyancyMovement, transform.position.z);

        float angleB = transform.localEulerAngles.z;
        angleB = (angleB > 180) ? angleB - 360 : angleB;
        transform.eulerAngles = new Vector3(buoyancyRotation,transform.eulerAngles.y,Mathf.Clamp(angleB,-maxRotationTilt,maxRotationTilt));
        
    }
}
