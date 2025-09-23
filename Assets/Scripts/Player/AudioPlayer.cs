using System;
using Collectible;
using UnityEngine;

namespace Player
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip errorPickupSound;
        [SerializeField] private AudioClip abilityPickupSound;
        [SerializeField] private AudioClip playerAttackSound;
        [SerializeField] private AudioClip playerDamageSound;
        [SerializeField] private AudioClip playerDeathSound;
        
        
        public static AudioPlayer Instance { get; private set; }
        private AudioSource audioSource;
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }


        public void PlayAbilityPickupSound()
        {
            audioSource.PlayOneShot(abilityPickupSound);
        }

        public void PlayCollectibleSound(CollectibleType type)
        {
            switch (type)
            {
                case CollectibleType.ERROR:
                    audioSource.PlayOneShot(errorPickupSound);
                    break;
                case CollectibleType.HEALTH:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void PlayPlayerSound(PlayerSoundType sound)
        {
            switch (sound)
            {
                case PlayerSoundType.JUMP:
                    break;
                case PlayerSoundType.ATTACK:
                    audioSource.PlayOneShot(playerAttackSound);
                    break;
                case PlayerSoundType.DEATH:
                    audioSource.PlayOneShot(playerDeathSound);
                    break;
                case PlayerSoundType.SPAWN:
                    break;
                case PlayerSoundType.COLLIDER:
                    break;
                case PlayerSoundType.BACKJUMP:
                    break;
                case PlayerSoundType.DAMAGE:
                    audioSource.PlayOneShot(playerDamageSound);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
            }
        }
    }
    
    public enum PlayerSoundType
    {
        JUMP,
        ATTACK,
        DEATH,
        SPAWN,
        COLLIDER,
        BACKJUMP,
        DAMAGE,
        
    }
}
