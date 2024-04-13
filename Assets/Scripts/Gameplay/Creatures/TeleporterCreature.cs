using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class TeleporterCreature : MonoBehaviour
    {
        [SerializeField] private TeleporterCreature linkedCreature;
        [SerializeField] private TeleporterType teleporterType;

        private readonly List<Collider2D> _receivedCreatures = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (teleporterType != TeleporterType.Connected) return;

            if (!_receivedCreatures.Contains(collision))
                linkedCreature?.ReceiveGnome(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_receivedCreatures.Contains(collision))
                _receivedCreatures.Remove(collision);
        }

        public void ReceiveGnome(Collider2D collision) 
        {
            if (collision.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.MovePosition(transform.position);
                _receivedCreatures.Add(collision);
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
