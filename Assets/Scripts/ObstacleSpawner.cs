using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    Vector3 dimensions;
    [SerializeField]
    int borderDistance;
    [SerializeField]
    int maxSpawnAmount;
    [SerializeField]
    List<ObstacleController> obstaclePrefabs;
    List<GameObject> spawnedPrefabs;
    [SerializeField]
    GameObject lootPrefab;
    [SerializeField]
    List<GameObject> powerupPrefabs;
    PlayerController playerRef;
    [SerializeField]
    int numCoinsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        spawnedPrefabs = new List<GameObject>();
        playerRef = GameObject.FindObjectOfType<PlayerController>();
        GameManager.instance.onRespawn += resetObstacles;
        SpawnPrefabs();
    }

    private void resetObstacles(object sender, EventArgs e)
    {
        foreach(var prefab in spawnedPrefabs)
        {
            Destroy(prefab);
        }
        spawnedPrefabs = new List<GameObject>();
        SpawnPrefabs();
    }

    private void SpawnPrefabs()
    {
        int spawnedCount = 0;
        while(spawnedCount < maxSpawnAmount)
        {
            int prefabIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Count);
            if(obstaclePrefabs[prefabIndex].minLevel > GameManager.instance.currentLevel)
            {
                continue;
            }
            float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
            float yRand = UnityEngine.Random.Range(transform.position.y - (dimensions.y / 2) + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
            float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);
            spawnedPrefabs.Add(Instantiate(obstaclePrefabs[prefabIndex].gameObject, new Vector3(xRand, yRand, zRand), obstaclePrefabs[prefabIndex].transform.rotation));
            spawnedCount++;
        }
        spawnedCount = 0;
        while(spawnedCount < numCoinsToSpawn)
        {
            float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
            float yRand = UnityEngine.Random.Range(transform.position.y - (dimensions.y / 2) + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
            float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);
            spawnedPrefabs.Add(Instantiate(lootPrefab, new Vector3(xRand, yRand, zRand), lootPrefab.transform.rotation));
            spawnedCount++;
        }
        if (UnityEngine.Random.Range(0,8) == 1)
        {
            int randDex = UnityEngine.Random.Range(0, powerupPrefabs.Count);
            float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
            float yRand = UnityEngine.Random.Range(transform.position.y - (dimensions.y / 2) + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
            float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);
            spawnedPrefabs.Add(Instantiate(powerupPrefabs[randDex], new Vector3(xRand, yRand, zRand), powerupPrefabs[randDex].transform.rotation));
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
