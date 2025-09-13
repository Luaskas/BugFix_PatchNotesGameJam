using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject hitBox;
    
    [Header("Jump / Gravitiy")]
    public float gravity = -9.81f;
    public float jumpHight = 2f;
    public float jumpSpeed = 8f;
    public bool isGrounded;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("KnockBack")]
    public float knockBackForceBack = 5f;
    public float knockBackForceUp = 0.5f;
    public float knockBackTimer = 0f;
    private Vector3 knockBackDirection;
    private bool isKnockedBack;
    
    [Header("Attack")]
    public float attackCooldown = 0.5f;
    public float attackDuration = 0.5f;
    private bool canAttack = true;
    
    [Header("Animation")]
    public Animator animator;

    [Header("Debug")] public TextMeshProUGUI debugText;
    
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("attack");
    private PlayerInputActions inputActions;
    private CharacterController controller;
    private Vector3 velocity;
    
    private void Awake()
    {
        
        controller = GetComponent<CharacterController>();
        
        inputActions = new PlayerInputActions();
        /*inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;*/

        //inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Attack.performed += ctx => StartAttack();
    }
    
    void OnEnable()
    {
        inputActions.Enable();
        mainCamera.GetComponent<CameraBehaviour>().currentCameraState = CameraStates.ActivePlayScene;
    }

    void OnDisable() => inputActions.Disable();
    
    void Update()
    {
        
        // Apply Knockback
        if (isKnockedBack)
        {
            Vector3 knockBackPush = knockBackDirection * moveSpeed;
            controller.Move(knockBackPush * Time.deltaTime);
            knockBackTimer -= Time.deltaTime;
            if(knockBackTimer <= 0f)
                isKnockedBack = false;
            return;
        }
        
        // Camera-relative movement
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = (camForward * input.y + camRight * input.x).normalized;

        // Rotation
        if (move.magnitude > 0.1f)
        {
            animator.SetBool(IsMoving, true);
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool(IsMoving, false);
        }

        // Apply movement (horizontal)
        controller.Move(move * (moveSpeed * Time.deltaTime));

        // Ground check BEFORE gravity

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        debugText.text = velocity.y.ToString();
        
        
        // Jump
        if (inputActions.Player.Jump.triggered && isGrounded)
        { 
            velocity.y = Mathf.Sqrt(jumpHight * jumpSpeed * gravity);
        }
           

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);

        // ✅ Final grounded state
        isGrounded = controller.isGrounded;

    }

    void Jump()
    {
        if(isGrounded)
            velocity.y = Mathf.Sqrt(jumpHight * jumpSpeed * gravity);
    }

    void StartAttack()
    {
        if (canAttack)
            StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger(Attack);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        hitBox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        
        canAttack = true;
    }

    public void ApplyKnockBack(Vector3 direction)
    {
        //isKnockedBack = true;
        direction.y = knockBackForceUp;
        direction.Normalize();
        controller.Move(direction * knockBackForceBack);
        
    }
    
}
