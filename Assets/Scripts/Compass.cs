using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Compass : MonoBehaviour
{   
    [SerializeField]
    GameObject[] treasures;
    public Transform dummy;
    public Transform player;
    public Vector3 rotate;
    // Start is called before the first frame update
    void Start()
    {
        treasures = GameObject.FindGameObjectsWithTag("Treasure");
    }

    // Update is called once per frame
    void Update()
    {
        var nearestTreasure = treasures.OrderBy(t=> Vector3.Distance(player.position, t.transform.position)).FirstOrDefault();
        dummy.position = nearestTreasure.transform.position - player.position + transform.position;
        transform.LookAt(dummy.position);
        transform.eulerAngles += rotate;
        Debug.DrawLine(transform.position, dummy.position);
    }
}
