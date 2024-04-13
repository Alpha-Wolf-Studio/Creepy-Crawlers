using UnityEngine;

namespace Gameplay.Creatures 
{
    [CreateAssetMenu(fileName = "Creature Data", menuName = "Creatures/Creature Data", order = 1)]
    public class CreatureData : ScriptableObject
    {
        public string creatureName;
        public Sprite creatureThumbnail;
        public CreatureSpawner creatureSpawnerPrefab;
    }
}
