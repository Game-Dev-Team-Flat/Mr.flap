using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Normal
{
    public class Dotsite : MonoBehaviour
    {
        public bool targetLockOn;

        private void OnTriggerEnter(Collider other)
        {
            targetLockOn = other.CompareTag("Player");
        }

        private void OnTriggerExit(Collider other)
        {
            targetLockOn = false;
        }
    }
}