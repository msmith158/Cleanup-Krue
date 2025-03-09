using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [Header("Values")] 
    public bool CanMove = true;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintModifier = 2f;
    [SerializeField] private float jumpPower = 5f;
    private float currentSpeed;
    
    [Header("Object References")]
    private GameObject playerCube;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCube = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            // Holding down the right button
            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
                else rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
            }

            // Holding down the left button
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
                else rb.linearVelocity = new Vector2(-currentSpeed, rb.linearVelocity.y);
            }

            // Holding down the Shift key to sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = playerSpeed * sprintModifier;
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = playerSpeed;
            }

            // Pressing the Space key to jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    rb.AddForce(transform.up * jumpPower);
                }
            }

            // Letting go of either key
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else if (!CanMove && rb.linearVelocity.x != 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
