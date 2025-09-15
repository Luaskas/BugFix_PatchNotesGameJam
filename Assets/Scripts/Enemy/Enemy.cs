using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int movingAnimation = Animator.StringToHash("moving");
    public int health = 10;
    public int damage = 3;
    
    [Header("Movement")]
    public float startSpeed = 1;
    public float maxSpeed = 10;
    public float stopDistance = 0.5f;
    public float acceleration = 2;
    public float rotationSpeed = 1;
    
    
    
    public Vector3 knockBack = new Vector3(0f, 0f, 0f);
    
    private Vector3 _spawnPosition;
    private EnemyAwareness _awareness;
    private EntityShaderController _entityShaderController;
    private Coroutine _goToPositionCoroutine;
    private GameObject _player;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _awareness = GetComponentInChildren<EnemyAwareness>();
        _entityShaderController = GetComponentInChildren<EntityShaderController>();
    }
    
    private void Start()
    {
        _spawnPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void OnEnable()
    {
        _awareness.onPlayerEnteredReach += FollowPlayer;
        _awareness.onPlayerLeftReach += ReturnToSpawnPoint;
    }

    private void OnDisable()
    {
        _awareness.onPlayerEnteredReach -= FollowPlayer;
        _awareness.onPlayerLeftReach += ReturnToSpawnPoint;
    }
    
    private void ReturnToSpawnPoint(object sender, EventArgs e)
    {
        if(_goToPositionCoroutine != null) StopCoroutine(_goToPositionCoroutine);
        _goToPositionCoroutine = StartCoroutine(FollowToPosition(null, _spawnPosition, false));
    }

    private void FollowPlayer(object sender, EventArgs e)
    {
        if(_goToPositionCoroutine != null) StopCoroutine(_goToPositionCoroutine);
        _goToPositionCoroutine = StartCoroutine(FollowToPosition(_player.transform, Vector3.zero, true));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} got {damage} damage.");
        if (health <= 0)
            Die();
    }

    public int MakeDamage()
    {
        return damage;
    }
    
    void Die()
    {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return _entityShaderController.ExecuteDisolveEffect();
        Destroy(gameObject);
    }


    /*public void OnCollisionEnter(Collision other)
    {
        PlayerData player = other.gameObject.GetComponent<PlayerData>();
        if (player != null)
        {
            player.TakeDmg(damage);
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            Vector3 knockBackDir = playerController.transform.position - transform.position;
            playerController.ApplyKnockBack(knockBackDir, damage);
        }
    }
    

    IEnumerator FollowToPosition(Transform targetTransform, Vector3 targetPosition, bool keepFollowing)
    {
        float currentSpeed = startSpeed;

        while (true)
        {
            _animator.SetBool(movingAnimation, true);
            Vector3 currentTarget = targetTransform ? targetTransform.position : targetPosition;
            float distance = Vector3.Distance(transform.position, currentTarget);

            // Exit if not keepFollowing and we’ve reached the destination
            if (!keepFollowing && distance <= stopDistance)
            {
                _animator.SetBool(movingAnimation, false);
                break;
            }
            
            // Accelerate
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

            // Move
            Vector3 direction = (currentTarget - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * (currentSpeed * Time.deltaTime);
            
            if (direction.sqrMagnitude > 0.0001f)
            {
                float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;       // 0 = forward (z)
                Quaternion targetRotation = Quaternion.Euler(0f, yaw + 180f, 0f);        // add 180° to flip
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            yield return null; 
        }
        yield return null;
    }

 
    
}
