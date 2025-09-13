using System;
using System.Collections;
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
    
    [Header("Attack)")]
    public float attackCooldown = 0.5f;
    public float attackDuration = 0.5f;
    private bool canAttack = true;
    
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
        
        // Camera-relative Movement
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();
        
        Vector3 move = camForward * input.y + camRight * input.x;
        move = move.normalized;
        
        /*
        //-------- Ground Check ------------
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        //-------- Movement ---------------
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        //Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        //-------- Gravity --------------
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);*/

        
        // Rotation in Movementdirection
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        
        // Apply Movement
        Vector3 displacement = move * moveSpeed;
        controller.Move(displacement * Time.deltaTime);
        
        // Ground check
        
        if(controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        // Jump
        isGrounded = controller.isGrounded;
        if (inputActions.Player.Jump.triggered && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHight * jumpSpeed * gravity);
            Debug.Log("Player Jumped");
        }
            
        
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

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
