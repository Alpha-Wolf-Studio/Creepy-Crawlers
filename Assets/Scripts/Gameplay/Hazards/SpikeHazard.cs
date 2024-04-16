using Gnomes;
using UnityEngine;

namespace Gameplay.Hazards
{
    public class SpikeHazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Gnome gnome)) 
            {
                gnome.Kill();
            }
        }
    }
}
