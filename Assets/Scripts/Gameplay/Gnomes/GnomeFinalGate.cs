using System;
using UnityEngine;
using Gameplay.Levels;
using Gnomes;

namespace Gameplay.Gnomes
{
    public class GnomeFinalGate : MonoBehaviour
    {
        [SerializeField] private bool isOpen = false;

        public static Action<Gnome> OnGnomeEntered;

        private void Start()
        {
            LevelSystem.OnKeysObjectiveReached += EnableDoor;
        }

        private void OnDestroy()
        {
            LevelSystem.OnKeysObjectiveReached -= EnableDoor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Gnome gnome = other.GetComponent<Gnome>();
            if (gnome != null)
            {
                Destroy(gnome.gameObject);
                OnGnomeEntered?.Invoke(gnome);
            }
        }

        private void EnableDoor()
        {
            isOpen = true;
        }
    }
}