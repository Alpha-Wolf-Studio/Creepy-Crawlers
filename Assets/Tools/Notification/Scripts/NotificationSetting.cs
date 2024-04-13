using UnityEngine;

namespace NotificationSystem
{
    [System.Serializable]
    public struct NotificationSetting
    {
        [Header("Settings")]
        public float duration;
        public Vector3 showPosition;
        public Vector3 hidePosition;

        [Header("Spawn")]
        public AnimationCurve animationSpawn;
        public float spawningTime;

        [Header("Despawn")]
        public AnimationCurve animationDespawn;
        public float despawningTime;

        public NotificationSetting (float duration, Vector3 showPosition, Vector3 hidePosition, AnimationCurve animationSpawn, float spawningTime, AnimationCurve animationDespawn, float despawningTime)
        {
            this.duration = duration;
            this.showPosition = showPosition;
            this.hidePosition = hidePosition;
            this.animationSpawn = animationSpawn;
            this.spawningTime = spawningTime;
            this.animationDespawn = animationDespawn;
            this.despawningTime = despawningTime;
        }
    }
}