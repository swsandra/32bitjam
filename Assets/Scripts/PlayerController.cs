using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float accelerationSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
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
        float rotation = rotationSpeed*movementHorizontal;
        rb.rotation *= Quaternion.AngleAxis(rotation, transform.up);
        //rb.rotation = new Quaternion(0, rb.rotation.y + (movementHorizontal*rotationSpeed), 0, 1).normalized;
        speed = Mathf.Clamp(speed + movementVertical, 0, maxSpeed);
        rb.velocity = transform.forward * speed;
    }
}
