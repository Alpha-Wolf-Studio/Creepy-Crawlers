using System;
using UnityEngine;

namespace Gameplay.Creatures 
{
    public abstract class CreatureSpawner : MonoBehaviour
    {
        public Action<CreatureData> CreatureSpawnedEvent = delegate { };

        protected CreatureData Data;
        public void SetCreatureData(CreatureData creatureData) => Data = creatureData;

        private Camera _mainCam;

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        protected Vector3 GetPointerPositionInWorldPosition() 
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -_mainCam.transform.position.z;
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(mousePos);
            return worldPos;
        }

        protected bool IsSpawnButtonDown() 
        {
            return Input.GetMouseButtonDown(0);
        }

        public virtual void Dispose() => Destroy(gameObject);
    }
}
