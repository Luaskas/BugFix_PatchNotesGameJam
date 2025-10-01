using System;
using Controller;
using GameLogic;
using UnityEngine;

namespace Environment
{
    public class BreachableWallTarget : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EnvironmentalController.Instance.SelectWallInReach(transform.parent.gameObject.GetComponent<Collider>());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EnvironmentalController.Instance.DeselectWallInReach();
            }
        }
    }
}