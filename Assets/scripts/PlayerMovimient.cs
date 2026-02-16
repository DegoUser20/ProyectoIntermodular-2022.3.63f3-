using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovimient : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    
    //Para correr
    public float runMultiplier = 2f;
    private bool isRunning;

    private Rigidbody rb;
    private bool isGrounded;

    private float xInput;
    private float zInput;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("He pulsado Jump. isGrounded = " + isGrounded);
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("SALTO!");
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        isRunning = Input.GetKey(KeyCode.LeftShift);

    }

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;

        Vector3 move = transform.right * xInput + transform.forward * zInput;
        Vector3 targetVelocity = move * moveSpeed;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = new Vector3(
            targetVelocity.x - velocity.x,
            0,
            targetVelocity.z - velocity.z
        );


        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

}
