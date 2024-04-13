using UnityEngine;

namespace Gameplay.Creatures
{
    public class WindCreature : MonoBehaviour
    {
        [SerializeField] private Bounds slowDownbound;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + slowDownbound.center, slowDownbound.size);
        }
    }
}