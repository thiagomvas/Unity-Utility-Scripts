using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    PhotonView view;
    private PlayerControls controls;
    private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [Header("Hover Settings")]
    [SerializeField] private float desiredHeight;
    [SerializeField] private float forceMult;
    [Header("Movement Settings")]
    [SerializeField] private float drag;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxGroundVelo;
    [SerializeField] private float maxAirVelo;
    [SerializeField] private float fullStopThreshold;
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Vector3 groundCheckOffset;
    [Header("Double Jump Settings")]
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce;
    
    
    private Vector3 movement;

    private float maxSpeed;

    private bool canJump;
    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (!view.IsMine) return;
        groundCheck.position = new Vector3(transform.position.x, transform.position.y - desiredHeight, transform.position.z) + groundCheckOffset;

        Hover();
        Movement();
        GroundCheck();
        //ControlDrag();
    }

    private void Hover()
    {

        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, desiredHeight);
        Debug.DrawLine(this.transform.position, transform.position + Vector3.down * desiredHeight, Color.red);

        if (hit.collider != null)
        {
            float x = desiredHeight - hit.distance;
            float springForce = (x * forceMult) - (desiredHeight * rb.velocity.y);
            Debug.DrawLine(this.transform.position, transform.position + Vector3.down * desiredHeight, Color.yellow);

            rb.AddForce(Vector3.up * springForce * (desiredHeight / Vector3.Distance(hit.point, this.transform.position)));

            maxSpeed = maxGroundVelo;

        }
        if (hit.collider == null) maxSpeed = maxAirVelo;

        //Extra gravity
        rb.AddForce(Vector3.down * 10 * rb.mass, ForceMode.Force);
    }

    private void Movement()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, desiredHeight, 3);
        Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0f, input.y);
        float prevMoveMag;

        //Speed cap
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -50f, jumpForce), Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));



        //If the movement keys are being pressed
        if (input != Vector2.zero)
        {
            Vector3 moveDir = transform.right * movement.x + transform.forward * movement.z;
            //rb.AddForce(moveDir * movementSpeed);
            if(canJump) rb.velocity = new Vector3(moveDir.x * movementSpeed, rb.velocity.y, moveDir.z * movementSpeed);
            else rb.AddForce(moveDir * movementSpeed);

            prevMoveMag = rb.velocity.magnitude; 
        }

        //Drag
        if(input == Vector2.zero && hit.collider)
        {
            prevMoveMag = -rb.velocity.magnitude;
            float x = Mathf.Abs(rb.velocity.x);
            float z = Mathf.Abs(rb.velocity.z);
            if(x > fullStopThreshold || z > fullStopThreshold) rb.AddForce(rb.velocity * prevMoveMag);
            rb.velocity = new Vector3(x < fullStopThreshold ? 0 : rb.velocity.x, rb.velocity.y, z < fullStopThreshold ? 0 : rb.velocity.z);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!canJump && !canDoubleJump) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse) ;
        if (!canJump) canDoubleJump = false;
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(groundCheck.position, 1f, 3)) { canJump = true; canDoubleJump = true;  }
        else canJump = false;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
