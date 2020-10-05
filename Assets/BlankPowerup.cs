using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankPowerup : MonoBehaviour
{
    public List<GameObject> powerupPrefabs;
    public GameObject curPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnPowerup(0, EventArgs.Empty);
        GameManager.instance.onRespawn += spawnPowerup;
    }

    private void spawnPowerup(object sender, EventArgs e)
    {
        if(curPrefab)
        {
            Destroy(curPrefab);
        }
        int randDex = UnityEngine.Random.Range(0, powerupPrefabs.Count);
        curPrefab = Instantiate(powerupPrefabs[randDex], transform.position, powerupPrefabs[randDex].transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
