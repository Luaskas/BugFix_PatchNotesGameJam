using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Collectible
{
    public class PickUp : MonoBehaviour
    {
        public CollectibleType type;
        
        private Rigidbody rb;
        
        public float startSpeed = 2f;       // speed when movement starts
        public float acceleration = 1f;     // units per second^2
        public float maxSpeed = 10f;
        public float stopDistance = 0.2f;
        private Coroutine moveCoroutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (moveCoroutine != null)
                    StopCoroutine(moveCoroutine);
                Debug.Log("Picked up");
                
                moveCoroutine = StartCoroutine(MoveTowardsPlayer(other.transform));
            }
        }
        
        
        private IEnumerator MoveTowardsPlayer(Transform player)
        {
            float currentSpeed = startSpeed;

            while (Vector3.Distance(transform.position, player.position) > stopDistance)
            {
                // Accelerate
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

                // Move
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * (currentSpeed * Time.deltaTime);

                yield return null; // wait until next frame
            }

            OnReachPlayer();
        }

        private void OnReachPlayer()
        {
            Debug.Log("Sphere reached the player via coroutine!");
            AudioPlayer.Instance.PlayCollectibleSound(type);

            switch (type)
            {
                case CollectibleType.ERROR:
                    PlayerData.Instance.IncreaseErrors();
                    break;
                case CollectibleType.HEALTH:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroy(gameObject);
        }
        
    }

    public enum CollectibleType
    {
        ERROR,
        HEALTH,
    }
}
