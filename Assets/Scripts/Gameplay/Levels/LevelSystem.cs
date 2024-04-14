using UnityEngine;
using Gameplay.Gnomes;
using System;
using System.Collections;

namespace Gameplay.Levels
{
    public class LevelSystem : MonoBehaviour
    {
        [Range(0f,1f)]
        [SerializeField] private float gnomesPercentageNeeded;
        [SerializeField] private int maxGnomesInLevel;
        [SerializeField] private int maxKeysInLevel;
        [SerializeField] private float secondsToStart = 2;

        private bool objectiveReached = false;
        private int keysObtained = 0;
        private int starsObtained = 0;

        public static Action<int> OnLevelStarted;
        public static Action OnGnomesReleased;
        public static Action OnKeysObjectiveReached;

        private void Start()
        {
            Gnome.Gnome.OnKeyPickUp += CheckKeys;
            OnLevelStarted?.Invoke(maxGnomesInLevel);
            GnomeFinalGate.OnGnomeEntered += AbsorbGnomeData;
            StartSummoning();
        }

        private void StartSummoning()
        {
            StartCoroutine(RealeaseTheGnomes());
        }

        private IEnumerator RealeaseTheGnomes()
        {
            yield return new WaitForSeconds(secondsToStart);
            OnGnomesReleased?.Invoke();
            yield return null;
        }

        private void CheckKeys()
        {
            keysObtained++;
            if (keysObtained >= maxKeysInLevel)
            {
                OnKeysObjectiveReached?.Invoke();
            }
        }

        private void PercentageReached(int gnomesDetected)
        {
            if((gnomesDetected / maxGnomesInLevel) > gnomesPercentageNeeded)
            {
                objectiveReached = true;
            }
        }

        private void AbsorbGnomeData(Gnome.Gnome gnome)
        {
            starsObtained += gnome.GnomeStats.starAmount;
        }
    }

}
