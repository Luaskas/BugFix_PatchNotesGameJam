using System;
using System.Collections;
using Debugging;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GameObject mainCamera;
    public GameObject hitBox;
    public Animator animator;
    public TextMeshProUGUI debugText;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("Jump / Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float gravityMultiplier = 2f; // increases gravity after apex
    public float fallMultiplier = 2.5f; 
    public float maxFallSpeed = -20f;     // clamp downward velocity
    public bool isGrounded;

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

    private DebugLine debugLine;

    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("attack");

    private PlayerInputActions inputActions;
    private CharacterController controller;
    private Vector3 velocity;
    private bool reachedApex = false; // track if jump apex reached

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Attack.performed += ctx => StartAttack();
        debugLine = OnScreenDebugController.Instance.CreateLine("PlayerControllerDebug", "PlayerControllerDebug");
    }

    private void OnEnable()
    {
        inputActions.Enable();
        mainCamera.GetComponent<CameraBehaviour>().currentCameraState = CameraStates.ActivePlayScene;
    }

    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        // --- Apply Knockback ---
        if (isKnockedBack)
        {
            Vector3 knockBackPush = knockBackDirection * moveSpeed;
            controller.Move(knockBackPush * Time.deltaTime);
            knockBackTimer -= Time.deltaTime;
            if (knockBackTimer <= 0f)
                isKnockedBack = false;
            return;
        }

        // --- Camera-relative movement ---
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = (camForward * input.y + camRight * input.x).normalized;

        // --- Rotation ---
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

        // --- Apply horizontal movement ---
        controller.Move(move * (moveSpeed * Time.deltaTime));

        // --- Ground check BEFORE gravity ---
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            reachedApex = false; // reset apex on ground
        }

        // --- Jump ---
        if (inputActions.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Apex detection ---
        if (!reachedApex && velocity.y <= 0)
            reachedApex = true;

        // --- Apply variable gravity ---
        float currentGravity = gravity;

        // Falling or short hop
        if (velocity.y < 0)
        {
            currentGravity *= gravityMultiplier;
        }
        else if (velocity.y > 0 && !inputActions.Player.Jump.IsPressed())
        {
            // Short hop: player released jump early
            currentGravity *= fallMultiplier;
        }

        velocity.y += currentGravity * Time.deltaTime;

        // Clamp falling speed
        if (velocity.y < maxFallSpeed)
            velocity.y = maxFallSpeed;

        // --- Apply vertical movement ---
        controller.Move(velocity * Time.deltaTime);

        // --- Update grounded state ---
        isGrounded = controller.isGrounded;
        
        debugLine.Text = $"Velocity Y: {velocity.y:F2} | Grounded: {isGrounded}";
    }

    private void StartAttack()
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
        direction.y = knockBackForceUp;
        direction.Normalize();
        controller.Move(direction * knockBackForceBack);
    }
}
