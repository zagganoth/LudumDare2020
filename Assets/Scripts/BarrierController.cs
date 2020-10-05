using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : ObstacleController
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    Vector3 dimensions;
    [SerializeField]
    bool movingUp;
    [SerializeField]
    bool movingDown;
    [SerializeField]
    bool movingLeft;
    [SerializeField]
    bool movingRight;

    [SerializeField]
    bool moveVertical;
    [SerializeField]
    bool moveHorizontal;
    protected override void childStart()
    {

        transform.localScale = new Vector3(Random.Range(0.1f, transform.localScale.x), Random.Range(0.1f, transform.localScale.y), Random.Range(0.1f, transform.localScale.y));
        if(UnityEngine.Random.Range(0f,1f) > 0.5f)
        {
            moveVertical = true;
        }
        if ((!moveVertical && UnityEngine.Random.Range(0f, 1f) > 0.5f) || UnityEngine.Random.Range(0f,1f) > 0.8f)
        {
            moveHorizontal = true;
        }
        if(moveVertical)
        {
            movingUp = true;
            movingDown = false;
        }
        if (moveHorizontal)
        {
            movingRight = true;
            movingLeft = false;
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (movingUp)
        {
            movingUp = false;
            movingDown = true;
        }   
        else if (movingDown)
        {
            movingUp = true;
            movingDown = false;
        }

        if(movingLeft)
        {
            movingLeft = false;
            movingRight = true;
        }
        else if(movingRight)
        {
            movingRight = false;
            movingLeft = true;
        }
    }
    protected override void Update()
    {
        if (movingUp && moveVertical)
        {
            transform.Translate(new Vector3(0, +moveSpeed * Time.deltaTime, 0));
        }
        /*
        if (transform.position.y > dimensions.y)
        {
            movingUp = false;
            movingDown = true;
        }*/

        if(movingDown && moveVertical)
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
        /*
        if (transform.position.y <= 0)
        {
            movingDown = false;
            movingUp = true;
        }*/

        if (movingLeft && moveHorizontal)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
        }
        /*
        if(transform.position.x <= 0)
        {
            movingLeft = false;
            movingRight = true;
        }*/

        if (movingRight && moveHorizontal)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        }
        /*
        if (transform.position.x > dimensions.x)
        {
            movingLeft = true;
            movingRight = false;
        }*/


    }
}
