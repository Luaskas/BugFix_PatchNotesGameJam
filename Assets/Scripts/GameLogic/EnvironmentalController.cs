using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace GameLogic
{
    public class EnvironmentalController : MonoBehaviour
    {
        public static EnvironmentalController Instance;
        
        private Collider _currentWallInReach;
        public bool m_isWallInReach = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SelectWallInReach(Collider wallInReach)
        {
            _currentWallInReach = wallInReach;
            m_isWallInReach = true;
        }

        public void DeselectWallInReach()
        {
            _currentWallInReach = null;
            m_isWallInReach = false;
        }
        
        public void DeactivateWallTemp()
        {
            _currentWallInReach.enabled = false;
            StartCoroutine(ReActivateCollider(_currentWallInReach));
        }
        
        public void DeactivateColliderTemp(Collider pColliderToDeactivate)
        {
            pColliderToDeactivate.enabled = false;
            StartCoroutine(ReActivateCollider(pColliderToDeactivate));
        }
        
        IEnumerator ReActivateCollider(Collider pColliderToReactivate)
        {
            yield return new WaitForSeconds(1f);
            pColliderToReactivate.enabled = true;
        }
    }
}
