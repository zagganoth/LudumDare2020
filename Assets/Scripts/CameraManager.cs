using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player;
    [SerializeField]
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform, Vector3.up);
    }
    public void rotate(float speed)
    {
        offset = Quaternion.Euler(0,180 * speed * Time.deltaTime, 0) * offset;
    }
}
