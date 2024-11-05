using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;
    private bool IsSprinting = false;
    private float speed;

    public float runSpeed = 5f;
    public float walkSpeed = 3f;
    public float jumpForce = 5f;
    public float rotationSpeed = 720f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJump;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Jump.canceled -= OnJump;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        IsSprinting = context.ReadValueAsButton();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        Vector3 move;

        if (IsSprinting)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        move = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;

        //rb.MovePosition(rb.position + move);
        transform.position += move;

        if (moveInput != Vector2.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            speed = 0f;
        }
        anim.SetFloat("VerticalSpeed", speed);


    }

    private void FixedUpdate()
    {
        //Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * walkSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + move);

        //if (moveInput != Vector2.zero) 
        //{
        //    anim.SetBool("IsWalking", true);
        //    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y)); 
        //    rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); 
        //}
        //else
        //{
        //    anim.SetBool("IsWalking", false);
        //}


    }


    private bool IsGrounded()
    {
        if (transform.position.y < 0.2f)
        {
            return true;
        }
        return false;
       
    }


}
