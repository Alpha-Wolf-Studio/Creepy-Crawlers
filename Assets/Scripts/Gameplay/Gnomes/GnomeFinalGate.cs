using System;
using UnityEngine;
using Utils;

namespace Gameplay.Gnomes
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private ObjectPool gnomesPool;
        private int gnomeMaxCount = 5;
        private int gnomesDetected = 0;
        [Range(0f, 1f)]
        private float gnomesPercentageNeeded;

        public event Action<GameObject> OnGnomeEntered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gnome"))
            {
                gnomesPool.ReturnObjectToPool(other.transform.gameObject);
                gnomesDetected++;
            }
        }

        public bool PercentageReached()
        {
            return (gnomesDetected / gnomeMaxCount) > gnomesPercentageNeeded;
        }

    }
}
