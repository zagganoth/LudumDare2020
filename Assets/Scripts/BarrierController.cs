using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : ObstacleController
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    Vector3 dimensions;
    bool movingUp;
    bool movingDown;
    protected override void childStart()
    {
        movingUp = true;
        movingDown = false;
    }
    protected override void Update()
    {
        if (movingUp)
        {
            transform.Translate(new Vector3(0, +moveSpeed * Time.deltaTime, 0));
        }

        if (transform.position.y > dimensions.y)
        {
            movingUp = false;
            movingDown = true;
        }

        if(movingDown)
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
        
        if (transform.position.y <= 0)
        {
            movingDown = false;
            movingUp = true;
        }

    }
}
