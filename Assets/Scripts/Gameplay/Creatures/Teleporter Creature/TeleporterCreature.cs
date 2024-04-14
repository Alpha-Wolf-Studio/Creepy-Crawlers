using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures
{
    [RequireComponent(typeof(Collider2D))]
    public class TeleporterCreature : MonoBehaviour
    {
        [Header("Teleporter Data")]
        [SerializeField] private GameObject teleporterSpawnPoint;

        [Header("Teleporter Visual")]
        [SerializeField] private GameObject teleporterFromVisual;
        [SerializeField] private GameObject teleporterToVisual;

        private TeleporterType _teleporterType;
        private TeleporterCreature _linkedCreature;

        private readonly List<Collider2D> _receivedCreatures = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_teleporterType != TeleporterType.From) return;

            if (!_receivedCreatures.Contains(collision))
                _linkedCreature?.ReceiveGnome(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_receivedCreatures.Contains(collision))
                _receivedCreatures.Remove(collision);
        }

        public void ReceiveGnome(Collider2D collision) 
        {
            collision.transform.position = teleporterSpawnPoint.transform.position;
            _receivedCreatures.Add(collision);
        }

        public void SetTeleport(TeleporterCreature newLinkedCreature, TeleporterType newTeleporterType)
        {
            _linkedCreature = newLinkedCreature;
            _teleporterType = newTeleporterType;

            teleporterFromVisual.SetActive(newTeleporterType == TeleporterType.From);
            teleporterToVisual.SetActive(newTeleporterType == TeleporterType.To);
        }

        public enum TeleporterType 
        {
            From,
            To
        }
    }
}
