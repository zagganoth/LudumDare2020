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
    [SerializeField]
    public float flyAcceleration;
    public Material defaultMaterial;
    [SerializeField]
    public Material invulnerableMaterial;
    private MeshRenderer m;
    [SerializeField]
    CameraManager mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
        startingFlySpeed = flySpeed;
        breakSpeed = flySpeed * .5f;

        GameManager.instance.onRespawn += respawn;
        GameManager.instance.onLevelIncrease += increaseFlySpeed;
        m = GetComponent<MeshRenderer>();
        defaultMaterial = m.material;
    }

    private void increaseFlySpeed(object sender, EventArgs e)
    {
        flySpeed = UnityEngine.Random.Range(0.1f, GameManager.instance.currentLevel * 0.05f);
        startingFlySpeed = flySpeed;
        breakSpeed = flySpeed * 0.5f;
    }
    
    public void SetInvulnerable()
    {
        m.material = invulnerableMaterial;
    }

    public void SetVulnerable()
    {
        m.material = defaultMaterial;
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
        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(0, verticalSpeed * moveSpeed, 0), Space.World);
        transform.Translate(transform.forward * flySpeed);
        transform.RotateAround(transform.position, Vector3.up, 180 * horizontalSpeed * Time.deltaTime);
        mainCamera.rotate(horizontalSpeed);
        if(Input.GetKey(KeyCode.RightShift))
        {
            if (flySpeed < startingFlySpeed + boostSpeed)
            {
                flySpeed += flyAcceleration;
            }
            particles.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.RightControl))
        {
            flySpeed = breakSpeed;
        }
        else
        {
            if (flySpeed > startingFlySpeed)
            {
                flySpeed -= flyAcceleration;
            }
            particles.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Creating bullet");
            GameObject g = Instantiate(projectilePrefab, transform.position + transform.forward, projectilePrefab.transform.rotation);
            Rigidbody rb2 = g.GetComponent<Rigidbody>();
            rb2.velocity = transform.up * 300f;
        }

        if(Input.GetKey(KeyCode.RightControl))
        {
            transform.RotateAround(transform.position, Vector3.forward, 180 * Time.deltaTime);
        }
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
