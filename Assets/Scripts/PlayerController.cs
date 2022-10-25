using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] int maxLives;
    [SerializeField] int lives;
    [SerializeField] float sinkDistance;
    [SerializeField] float blinkRate = .1f;
    [Header("death")]
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] float deathRotationSpeed;
    [SerializeField] float deathSinkSpeed;
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
    [SerializeField] float invulnerableDuration;
    [Header("Whirlpool")]
    [SerializeField] float whirlpoolForce;
    
    Rigidbody rb;
    float speed;
    float movementVertical;
    float movementHorizontal;
    float startingY;
    bool canCollide = true;
    Vector3 rockMovement;
    Vector3 whirlpoolMovement;
    bool dead = false;
    float deathRotationCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingY = transform.position.y;
        lives = maxLives;
        deathRotationCounter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        movementVertical = Input.GetAxis("Vertical");
        movementHorizontal = Input.GetAxis("Horizontal");
        if (lives <= 0) {
            dead = true;
            canCollide = false;
            vCam.enabled = false;
            rb.velocity = Vector3.zero;
            StartCoroutine(Death());
        }
    }

    private void FixedUpdate() {
        if (!dead) {
            // Boat rotation
            float rotation = rotationSpeed*movementHorizontal;
            transform.Rotate(new Vector3(0,rotation,0),Space.World);

            // Boat movement
            speed = Mathf.Clamp(speed + movementVertical*accelerationSpeed, 0, maxSpeed);
            rb.velocity = (transform.forward * speed);

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

            // Extra movement
            rb.velocity += whirlpoolMovement + rockMovement;

            // Buoyancy
            float buoyancyMovement = Mathf.PingPong(Time.time * buoyancySpeed, maxBuoyancy) - (maxBuoyancy/2);
            float buoyancyRotation = Mathf.PingPong(Time.time * buoyancyRotationSpeed, maxBuoyancyRotation) - (maxBuoyancyRotation/2);

            transform.position = new Vector3(transform.position.x, startingY + buoyancyMovement - ((maxLives-lives) * sinkDistance), transform.position.z);

            float angleB = transform.localEulerAngles.z;
            angleB = (angleB > 180) ? angleB - 360 : angleB;
            transform.eulerAngles = new Vector3(buoyancyRotation,transform.eulerAngles.y,Mathf.Clamp(angleB,-maxRotationTilt,maxRotationTilt));
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        if (canCollide && other.gameObject.tag == "Rock") {
            Vector3 dir = other.GetContact(0).point - transform.position;
            dir = -dir.normalized;
            rockMovement = dir*collisionForce;
            speed = 0f;

            canCollide = false;
            lives--;
            StartCoroutine(Blink(invulnerableDuration));
            StartCoroutine(invulnerable());
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Whirlpool") {
            Vector3 dir = other.transform.position - transform.position;
            dir = Vector3.Cross(dir, Vector3.up);
            dir = dir.normalized;
            whirlpoolMovement = dir * whirlpoolForce;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Whirlpool") {
            whirlpoolMovement = Vector3.zero;
        }
    }

    IEnumerator Death() {
        yield return new WaitForSeconds(2);
        while (deathRotationCounter < 90) {
            transform.Rotate(new Vector3(-deathRotationSpeed*Time.fixedDeltaTime, 0, 0), Space.Self);
            yield return new WaitForFixedUpdate();
            deathRotationCounter += deathRotationSpeed*Time.fixedDeltaTime;
            Debug.Log(deathRotationCounter);
        }
        yield return new WaitForSeconds(1);
        while(true) {
            transform.position -= new Vector3(0, deathSinkSpeed*Time.fixedDeltaTime, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator invulnerable() {
        yield return new WaitForSeconds(invulnerableDuration);
        canCollide = true;
        rockMovement = Vector3.zero;
    }

    IEnumerator Blink(float blinkTime) {
        var endTime = Time.time + blinkTime;
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
        while(Time.time<endTime) {

            foreach (MeshRenderer meshRenderer in childRenderers)
                meshRenderer.enabled = false;

            yield return new WaitForSeconds(blinkRate);

            foreach (MeshRenderer meshRenderer in childRenderers)
                meshRenderer.enabled = true;

            yield return new WaitForSeconds(blinkRate);
        }
    }
}
