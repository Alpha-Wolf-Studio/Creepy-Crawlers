using Gameplay.Levels;
using System;
using UnityEngine;
using Utils;
using Gnomes;

namespace Gameplay.Gnomes
{
    public class GnomeFinalGate : MonoBehaviour
    {
        [SerializeField] private ObjectPool gnomesPool;
        private bool isOpen = false;

        public static Action<Gnome> OnGnomeEntered;

        private void Start()
        {
            LevelSystem.OnKeysObjectiveReached += EnableDoor;
        }

        private void OnDestroy()
        {
            LevelSystem.OnKeysObjectiveReached -= EnableDoor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isOpen)
            {
                Gnome gnome = other.GetComponent<Gnome>();
                if (gnome != null)
                {
                    gnomesPool.ReturnObjectToPool(gnome.gameObject);
                    OnGnomeEntered?.Invoke(gnome);
                }
            }
        }

        private void EnableDoor()
        {
            isOpen = true;
        }
       

    }
}
