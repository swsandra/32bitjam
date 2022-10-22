using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 0.5357f;
    [SerializeField] GameObject[] fishPrefabs;
    [SerializeField] float fishSpeed = 1;

    Transform[] spawnPositions;

    void Start()
    {
        StartCoroutine(SpawnFish());
        spawnPositions = System.Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform);
    }

    IEnumerator SpawnFish() {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            Transform randomSpawnPoint = spawnPositions[Random.Range(0, spawnPositions.Length)];
            int direction = randomSpawnPoint.transform.position.x > transform.position.x ? -1 : 1;
            GameObject fish = Instantiate(fishPrefab, randomSpawnPoint.position, Quaternion.identity);
            if (direction == 1) {
                foreach (Transform child in fish.transform){
                    child.eulerAngles = new Vector3(child.eulerAngles.x, child.eulerAngles.y+180, child.eulerAngles.z);
                }
            }
            fish.GetComponent<FishController>().direction = direction;
            fish.GetComponent<FishController>().speed = fishSpeed;
        }
    }

}
