using System;
using System.Collections;
using Debugging;
using Player;
using TMPro;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public GameObject mainCamera;
        public GameObject hitBox;
        public Animator animator;
        public PlayerVisualController visualController;
    
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;
    
        [Header("Jump / Gravity")]
        public float gravity = -9.81f;
        public float jumpHeight = 2f;
        public float gravityMultiplier = 2f; 
        public float fallMultiplier = 2.5f; 
        public float maxFallSpeed = -20f;     
        public bool isGrounded;
        private bool reachedApex = false;

        [Header("KnockBack")]
        public float knockBackForceBack = 5f;
        public float knockBackForceUp = 0.5f;
        public float knockBackTimer = 0.3f; // shorter feels snappier
        private bool isKnockedBack;
        private bool knockbackTimerUp = true;

        // New: stores temporary knockback velocity
        private Vector3 knockbackVelocity;

        [Header("Attack")]
        public float attackCooldown = 0.5f;
        public float attackDuration = 0.5f;
        private bool canAttack = true;

        private DebugLine debugLine;

        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Attack = Animator.StringToHash("attack");

        private PlayerInputActions inputActions;
        private CharacterController controller;
        private Camera cam;

        private Vector3 velocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            inputActions = new PlayerInputActions();
            inputActions.Player.Attack.performed += ctx => StartAttack();
            debugLine = OnScreenDebugController.Instance.CreateLine("PlayerControllerDebug", "PlayerControllerDebug");
            cam = Camera.main;
        }

        private void OnEnable()
        {
            inputActions.Enable();
            mainCamera.GetComponent<CameraBehaviour>().currentCameraState = CameraStates.ActivePlayScene;
        }

        private void Start()
        {
            visualController.ActivateDefaultShader();
        }

        private void OnDisable() => inputActions.Disable();

        private void Update()
        {
            // --- Camera-relative movement ---
            Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
            Vector3 camForward = cam.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();
            Vector3 camRight = cam.transform.right;
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
                reachedApex = false;
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
            if (velocity.y < 0)
            {
                currentGravity *= gravityMultiplier;
            }
            else if (velocity.y > 0 && !inputActions.Player.Jump.IsPressed())
            {
                currentGravity *= fallMultiplier;
            }

            velocity.y += currentGravity * Time.deltaTime;

            // Clamp falling speed
            if (velocity.y < maxFallSpeed)
                velocity.y = maxFallSpeed;

            // --- Apply vertical + knockback movement ---
            controller.Move((velocity + knockbackVelocity) * Time.deltaTime);

            // --- Decay knockback over time ---
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, 5f * Time.deltaTime);

            // --- Update grounded state ---
            isGrounded = controller.isGrounded;
        
            debugLine.Text = $"Velocity Y: {velocity.y:F2} | Grounded: {isGrounded} | Knockback: {knockbackVelocity}";
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

        private IEnumerator PerformKnockback(Vector3 direction)
        {
            knockbackTimerUp = false;
            visualController.ActivateDamageShader();
        
            direction.y = knockBackForceUp;
            direction.Normalize();

            // Give an impulse (one-time push)
            knockbackVelocity = direction * knockBackForceBack;
            
            yield return new WaitForSeconds(knockBackTimer);
        
            visualController.ActivateDefaultShader();
            knockbackTimerUp = true;
        }

        public void ApplyKnockBack(Vector3 dir)
        {
            if (knockbackTimerUp) StartCoroutine(PerformKnockback(dir));
        }
    }
}
