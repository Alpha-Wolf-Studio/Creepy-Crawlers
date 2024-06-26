using System.Collections.Generic;
using UnityEngine;
using Gnomes;

namespace Gameplay.Creatures
{
    [RequireComponent(typeof(Collider2D))]
    public class TeleporterCreature : MonoBehaviour
    {
        [Header("Teleporter Data")]
        [SerializeField] private GameObject teleporterSpawnPoint;
        [SerializeField] private BoxCollider2D teleporterSpawnCheckCollider;

        [Header("Teleporter Visual")]
        [SerializeField] private GameObject teleporterFromVisual;
        [SerializeField] private GameObject teleporterToVisual;

        [Header("Teleporter Manual Linking")]
        [SerializeField] private TeleporterType teleporterType;
        [SerializeField] private TeleporterCreature linkedCreature;

        private readonly List<Collider2D> _receivedCreatures = new List<Collider2D>();

        private void Start()
        {
            if (teleporterType != TeleporterType.Disabled)
                SetTeleport(linkedCreature, teleporterType);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (teleporterType != TeleporterType.From) return;

            if (!_receivedCreatures.Contains(collision)) 
            {
                if (collision.GetComponent<Gnome>())
                {
                    if(linkedCreature)
                        linkedCreature.ReceiveGnome(collision);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_receivedCreatures.Contains(collision))
                _receivedCreatures.Remove(collision);
        }

        public void ReceiveGnome(Collider2D gnomeCollider) 
        {
            gnomeCollider.transform.position = teleporterSpawnPoint.transform.position;
            _receivedCreatures.Add(gnomeCollider);
        }

        public void SetTeleport(TeleporterCreature newLinkedCreature, TeleporterType newTeleporterType)
        {
            linkedCreature = newLinkedCreature;
            teleporterType = newTeleporterType;

            teleporterFromVisual.SetActive(newTeleporterType == TeleporterType.From);
            teleporterToVisual.SetActive(newTeleporterType == TeleporterType.To);
        }

        public Bounds GetCreatureColliderBounds()
        {
            Vector3 size = teleporterSpawnCheckCollider.size * transform.localScale;
            return new Bounds(teleporterSpawnCheckCollider.offset, size);
        }

        public enum TeleporterType 
        {
            Disabled,
            From,
            To
        }
    }
}
