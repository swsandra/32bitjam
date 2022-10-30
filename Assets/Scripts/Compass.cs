using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Compass : MonoBehaviour
{   
    [SerializeField]
    List<GameObject> treasures;
    public Transform dummy;
    public Transform player;
    public Vector3 rotate;
    // Start is called before the first frame update
    void Start()
    {
        treasures = GameObject.FindGameObjectsWithTag("Treasure").ToList();
        treasures.AddRange(GameObject.FindGameObjectsWithTag("Junk").ToList());
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < treasures.Count)
        {
            GameObject t = treasures[i];
            if (t == null) {
                treasures.Remove(t);
                continue;
            }
            i++;
        }
        if (treasures.Count > 0) {
            var nearestTreasure = treasures.OrderBy(t=> Vector3.Distance(player.position, t.transform.position)).FirstOrDefault();
            dummy.position = nearestTreasure.transform.position - player.position + transform.position;
            transform.LookAt(dummy.position);
            transform.eulerAngles += rotate;
        }
    }
}
