using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 0.5357f;
    [SerializeField] GameObject[] fishPrefabs;

    [Header("Fish Settings")]
    [SerializeField] float minRotationSpeed = 20f;
    [SerializeField] float maxRotationSpeed = 30f;
    [SerializeField] float minRotation = 30f;
    [SerializeField] float maxRotation = 45f;
    [SerializeField] float fishSpeed = 1;

    Transform[] spawnPositions;
    int nextPosition, lastPosition;

    void Start()
    {
        spawnPositions = System.Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform);
        lastPosition = -1;
        StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish() {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            nextPosition = Random.Range(0, spawnPositions.Length);
            while (lastPosition == nextPosition){
                nextPosition = Random.Range(0, spawnPositions.Length);
            }
            Transform randomSpawnPoint = spawnPositions[nextPosition];
            lastPosition = nextPosition;
            int direction = randomSpawnPoint.transform.position.x > transform.position.x ? -1 : 1;
            GameObject fish = Instantiate(fishPrefab, randomSpawnPoint.position, Quaternion.identity);
            if (direction == 1) {
                foreach (Transform child in fish.transform){
                    child.eulerAngles = new Vector3(child.eulerAngles.x, child.eulerAngles.y+180, child.eulerAngles.z);
                }
            }
            fish.GetComponent<FishController>().UpdateSettings(direction, fishSpeed, Random.Range(minRotationSpeed, maxRotationSpeed), Random.Range(minRotation, maxRotation));
        }
    }

}
