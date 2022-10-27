using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] junkPrefabs;
    [Header("Junk Settings")]
    [SerializeField] float minSpeed = .3f;
    [SerializeField] float maxSpeed = .5f;
    [SerializeField] float minYDistance = 1f;
    [SerializeField] float maxYDistance = 2f;
    [SerializeField] int spawnPointsCount;
    Transform[] spawnPositions;
    void Start()
    {
        spawnPositions = System.Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform);
        // TODO: elegir las posiciones de spawn
        List<Transform> spawnPos = new List<Transform>();
        while(spawnPos.Count < spawnPointsCount) {
            int randomInt = Random.Range(0,spawnPositions.Length);
            if (!spawnPos.Contains(spawnPositions[randomInt])) {
                spawnPos.Add(spawnPositions[randomInt]);
            }
        }
        
        GameObject junkPrefab;
        GameObject junk;
        foreach (Transform pos in spawnPos)
        {
            junkPrefab = junkPrefabs[Random.Range(0, junkPrefabs.Length)];
            junk = Instantiate(junkPrefab, pos.position, junkPrefab.transform.rotation);
            junk.transform.eulerAngles = new Vector3(Random.Range(0, 360), junk.transform.eulerAngles.y, junk.transform.eulerAngles.z);
            junk.GetComponent<Junk>().speed = Random.Range(minSpeed, maxSpeed);
            junk.GetComponent<Junk>().maxYDistance = Random.Range(minYDistance, maxYDistance);
        }
    }

}
