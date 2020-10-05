using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !GameManager.instance.playerInvulnerable)
        {
            GameManager.instance.fireRespawnEvent();
        }
        Destroy(this.gameObject);
    }
}
