using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player;
    [SerializeField]
    Vector3 offset;
    Vector3 startOffset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        startOffset = new Vector3(offset.x, offset.y, offset.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.transform.position + offset;
        transform.position = newPos;//new Vector3(newPos.x, Mathf.Clamp(newPos.y,-15,30), newPos.z);
        transform.LookAt(player.transform, Vector3.up);
    }
    public void rotate(float speed)
    {
        offset = Quaternion.Euler(0,180 * speed * Time.deltaTime, 0) * offset;
    }
    public void resetOffset()
    {
        offset = startOffset;
    }
}
