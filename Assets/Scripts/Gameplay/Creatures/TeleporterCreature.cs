using UnityEngine;

namespace Gameplay.Creatures
{
    public class TeleporterCreature : MonoBehaviour
    {
        [SerializeField] private TeleporterCreature linkedCreature;
        [SerializeField] private TeleporterType teleporterType;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (teleporterType != TeleporterType.Connected) return;

            if (collision.TryGetComponent(out Rigidbody2D rigidbody2D)) 
            {
                rigidbody2D.MovePosition(linkedCreature.transform.position);
            }
        }

        public void SetTeleport(TeleporterCreature newLinkedCreature, TeleporterType newTeleporterType)
        {
            linkedCreature = newLinkedCreature;
            teleporterType = newTeleporterType;
        }

        public enum TeleporterType 
        {
            Disconnected,
            Connected
        }
    }
}
