using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;

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
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
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

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * walkSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (moveInput != Vector2.zero) 
        {
            anim.SetBool("IsWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y)); 
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); 
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }


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
