using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField]
    public string powername;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.fireCollectEvent(powername);
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 180 * Time.deltaTime);
    }
}
