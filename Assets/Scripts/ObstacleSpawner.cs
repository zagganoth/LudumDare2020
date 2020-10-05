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
    List<MeshRenderer> childRenderers;
    private Material defaultMaterial;
	private Material defaultMaterial2;
    [SerializeField]
    private Material asteroidMaterial;
    private bool asteroidUnit = true;
    [SerializeField]
    GameObject asteroidPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnedPrefabs = new List<GameObject>();
        playerRef = GameObject.FindObjectOfType<PlayerController>();
        GameManager.instance.onRespawn += resetObstacles;
        childRenderers = new List<MeshRenderer>();
        var cComps = GetComponentsInChildren<MeshRenderer>();
        foreach(var comp in cComps)
        {
            if (comp.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                childRenderers.Add(comp);
            }
        }
        defaultMaterial = childRenderers[0].material;
		defaultMaterial2 = childRenderers[3].material;
        
        SpawnPrefabs();
    }

    private void resetObstacles(object sender, EventArgs e)
    {
        if(UnityEngine.Random.Range(0,64) == 1)
        {
            asteroidUnit = true;
        }
        else
        {
            asteroidUnit = false;
        }
        foreach(var prefab in spawnedPrefabs)
        {
            Destroy(prefab);
        }
        spawnedPrefabs = new List<GameObject>();
        if (!asteroidUnit)
        {	/*
            foreach(var render in childRenderers)
            {
                render.material = defaultMaterial;
            }
			*/
			childRenderers[0].material = defaultMaterial;
			childRenderers[1].material = defaultMaterial;
			childRenderers[2].material = defaultMaterial2;
			childRenderers[3].material = defaultMaterial2;
            SpawnPrefabs();
        } 
        else
        {
			/*
            foreach(var render in childRenderers)
            {
                render.material = asteroidMaterial;
            }
			*/
			childRenderers[0].material = asteroidMaterial;
			childRenderers[1].material = asteroidMaterial;
			childRenderers[2].material = defaultMaterial2;
			childRenderers[3].material = defaultMaterial2;
            StartCoroutine(SpawnAsteroids());
        }
    }
    IEnumerator SpawnAsteroids()
    {
        while(asteroidUnit)
        {
            
            yield return new WaitForSeconds(0.5f);
            int randNum = UnityEngine.Random.Range(1, Math.Max(1,Mathf.FloorToInt(Mathf.Sqrt(GameManager.instance.currentLevel * 2))));
            for (int i = 0; i < randNum; i++)
            {
                float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
                float yRand = UnityEngine.Random.Range(transform.position.y + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
                float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);

                spawnedPrefabs.Add(Instantiate(asteroidPrefab, new Vector3(xRand, yRand, zRand), asteroidPrefab.transform.rotation));
            }
        }
    }

    private void SpawnPrefabs()
    {
        int spawnedCount = 0;
        int numToSpawn = UnityEngine.Random.Range(1, Mathf.FloorToInt(Mathf.Sqrt(maxSpawnAmount + GameManager.instance.currentLevel)));
        while(spawnedCount < numToSpawn)
        {
            int prefabIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Count);
            if(obstaclePrefabs[prefabIndex].minLevel > GameManager.instance.currentLevel)
            {
                continue;
            }
            float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
            float yRand = UnityEngine.Random.Range(transform.position.y - (dimensions.y / 2) + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
            float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);
            spawnedPrefabs.Add(Instantiate(obstaclePrefabs[prefabIndex].gameObject, new Vector3(xRand, yRand, zRand), obstaclePrefabs[prefabIndex].transform.rotation * transform.rotation));
            spawnedCount++;
        }
        spawnedCount = 0;
        while(spawnedCount < numCoinsToSpawn)
        {
            float xRand = UnityEngine.Random.Range(transform.position.x - (dimensions.x / 2) + borderDistance, transform.position.x + (dimensions.x / 2) - borderDistance);
            float yRand = UnityEngine.Random.Range(transform.position.y - (dimensions.y / 2) + borderDistance, transform.position.y + (dimensions.y / 2) - borderDistance);
            float zRand = UnityEngine.Random.Range(transform.position.z - (dimensions.z / 2) + borderDistance, transform.position.z + (dimensions.z / 2) - borderDistance);
            spawnedPrefabs.Add(Instantiate(lootPrefab, new Vector3(xRand, yRand, zRand), lootPrefab.transform.rotation * transform.rotation));
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
