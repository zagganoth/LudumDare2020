using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player")) GameManager.instance.fireRespawnEvent(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
