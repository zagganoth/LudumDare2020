using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    public float flySpeed;
    [SerializeField]
    float breakSpeed;
    [SerializeField]
    float boostSpeed;
    public float startingFlySpeed;
    public bool moving;
    public Vector3 spawnPosition;
    [SerializeField]
    public GameObject particles;
    [SerializeField]
    private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
        startingFlySpeed = flySpeed;
        breakSpeed = flySpeed * .5f;

        GameManager.instance.onRespawn += respawn;
        GameManager.instance.onLevelIncrease += increaseFlySpeed;


    }

    private void increaseFlySpeed(object sender, EventArgs e)
    {
        flySpeed = UnityEngine.Random.Range(0.1f, GameManager.instance.currentLevel * 0.05f);
        startingFlySpeed = flySpeed;
        breakSpeed = flySpeed * 0.5f;
    }
    

    private void respawn(object sender, EventArgs e)
    {
        transform.position = spawnPosition;
    }

    public void OnMovement(InputValue value)
    {
        Debug.Log("Moving!");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0,moveSpeed, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(moveSpeed, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0,-moveSpeed, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(-moveSpeed, 0, 0), Space.World);
        }
        if(Input.GetKey(KeyCode.RightShift))
        {
            flySpeed = startingFlySpeed + boostSpeed;
            particles.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            flySpeed = breakSpeed;
        }
        else
        {
            flySpeed = startingFlySpeed;
            particles.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject g = Instantiate(projectilePrefab, transform.position + new Vector3(0, 0, -10), projectilePrefab.transform.rotation);
            Rigidbody rb2 = g.GetComponent<Rigidbody>();
            rb2.velocity = new Vector3(0, 0, -300f);
        }

        if(Input.GetKey(KeyCode.RightControl))
        {
            transform.RotateAround(transform.position, Vector3.forward, 180 * Time.deltaTime);
        }

        transform.position += new Vector3(0, 0, -flySpeed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("moving!");
        Vector2 input = context.ReadValue<Vector2>();
        rb.velocity = input * moveSpeed;
        if(rb.velocity.magnitude > 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }
    public void OnFly(InputAction.CallbackContext context)
    {
        Debug.Log("Flying!");
        /*float input = context.ReadValue<float>();
        rb.velocity += new Vector3(0,0,input * flySpeed);*/
    }


}
