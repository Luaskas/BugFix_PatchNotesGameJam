using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject hitBox;
    public GameObject player;
    
    public Rigidbody rb;
    
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHight = 2f;
    public float jumpSpeed = 8f;

    public float attackCooldown = 0.5f;
    public float attackDuration = 0.5f;
    private bool canAttack = true;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    
    public float knockBackForce = 5f;
    
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();

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
        //-------- Ground Check ------------
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        //-------- Movement ---------------
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        //-------- Jump --------------
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
        direction.y = 0;
        direction.Normalize();

        controller.Move(direction * knockBackForce);

    }

}
