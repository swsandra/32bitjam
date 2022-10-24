using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] GameObject hook;
    [SerializeField] float verticalSpeed = 10f;
    public bool isGrounded;
    public bool isHooked;

    void Start()
    {
        isGrounded = false;
    }

    void Update()
    {
        if (isGrounded || isHooked) return;

        Vector3 movement = Vector3.down * verticalSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bottom")){
            isGrounded = true;
        }else if (other.CompareTag("Obstacle") && isHooked){
            hook.GetComponent<HookController>().DropTreasure();
        }
    }

}
