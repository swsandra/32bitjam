using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{
    [Header("Movement")]
    public float speed = .3f;
    public float maxYDistance = 2f;
    Vector3 pos1, pos2;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = new Vector3(transform.position.x, transform.position.y-maxYDistance, transform.position.z);
        pos2 = new Vector3(transform.position.x, transform.position.y+maxYDistance, transform.position.z);
    }

    void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time*speed, 1.0f);
        transform.position = Vector3.Lerp(pos1, pos2, t);
    }
}
