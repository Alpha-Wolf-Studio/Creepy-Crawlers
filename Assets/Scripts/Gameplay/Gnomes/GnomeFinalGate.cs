using Gameplay.Levels;
using System;
using UnityEngine;
using Utils;

namespace Gameplay.Gnomes
{
    public class GnomeFinalGate : MonoBehaviour
    {
        [SerializeField] private ObjectPool gnomesPool;
        private bool isOpen = false;

        public static Action<Gnome.Gnome> OnGnomeEntered;

        private void Start()
        {
            LevelSystem.OnKeysObjectiveReached += EnableDoor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isOpen)
            {
                if (other.CompareTag("Gnome"))
                {
                    gnomesPool.ReturnObjectToPool(other.transform.gameObject);
                    OnGnomeEntered?.Invoke(other.transform.gameObject.GetComponent<Gnome.Gnome>());
                }
            }
        }

        private void EnableDoor()
        {
            isOpen = true;
        }
       

    }
}
