using UnityEngine;

namespace Gameplay.Creatures
{
    public class WallCreature : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D col;

        public Bounds GetCreatureColliderBounds() 
        {
            Vector3 size = col.size * transform.localScale;
            return new Bounds(col.offset, size);
        }
        
    }
}