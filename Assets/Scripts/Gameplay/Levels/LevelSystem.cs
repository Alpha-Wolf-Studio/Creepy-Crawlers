using UnityEngine;
using Gameplay.Gnomes;
using System;
using System.Collections;
using UnityEngine.Audio;
using Gnomes;
using UnityEngine.SceneManagement;
using UI.Gameplay;
using CustomSceneSwitcher.Switcher.Data;

namespace Gameplay.Levels
{
    public class LevelSystem : MonoBehaviour
    {
        [Header("Level Config")] 
        [SerializeField] private int minGnomes = 16;
        [SerializeField] private int maxGnomesInLevel;
        [SerializeField] private float secondsToStart = 2;

        [Header("Audio Config")] [SerializeField]
        private AudioClip audioClip = null;

        [SerializeField] private AudioMixerGroup audioMixerGroup = null;
        private AudioSource audioSource = null;

        private int starsObtained = 0;
        private int gnomesAbsorbed = 0;
        private int gnomesDeleted = 0;
        private bool isLevelActive = false;
        private int currentlevel = 0;

        public static Action<int> OnLevelCleared;
        public static Action<int> OnLevelFailed;
        public static Action<int, int> OnLevelStarted;
        public static Action OnGnomeAbsorbed;
        public static Action OnGnomesReleased;
        public static Action OnKeysObjectiveReached;

        private void Awake()
        {
            UIGameflowButtonsHolder.onFinishLevelPressed += FinishLevel;
            GnomeFinalGate.OnGnomeEntered += AbsorbGnomeData;
            Gnome.onDeleteGnome += CheckLevelConditions;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();

            currentlevel = SceneManager.GetActiveScene().buildIndex;
        }

        private void Start()
        {
            OnLevelStarted?.Invoke(currentlevel, maxGnomesInLevel);
            StartSummoning();
        }

        private void OnDestroy()
        {
            UIGameflowButtonsHolder.onFinishLevelPressed -= FinishLevel;
            GnomeFinalGate.OnGnomeEntered -= AbsorbGnomeData;
            Gnome.onDeleteGnome -= CheckLevelConditions;
        }

        private void CheckLevelConditions()
        {
            gnomesDeleted++;

            if (gnomesDeleted == maxGnomesInLevel)
                FinishLevel();
        }

        private void FinishLevel()
        {
            if (!isLevelActive) return;

            isLevelActive = false;
            SetResult();
        }

        private void StartSummoning()
        {
            isLevelActive = true;
            StartCoroutine(RealeaseTheGnomes());
        }

        private IEnumerator RealeaseTheGnomes()
        {
            yield return new WaitForSeconds(secondsToStart);
            OnGnomesReleased?.Invoke();
            yield return null;
        }

        private void AbsorbGnomeData(Gnome gnome)
        {
            gnomesAbsorbed++;
            starsObtained += gnome.GnomeStats.starAmount;
            OnGnomeAbsorbed?.Invoke();
        }

        private void SetResult()
        {
            if (gnomesAbsorbed >= minGnomes && starsObtained >= 1)
                SaveAndLoad.SaveLevel(currentlevel, 3);
            else if (gnomesAbsorbed >= minGnomes)
                SaveAndLoad.SaveLevel(currentlevel, 2);
            else
                SaveAndLoad.SaveLevel(currentlevel, 1);

            SaveAndLoad.LoadAll();

            bool currentLevelCleared = SaveAndLoad.SaveGame.maxLevel >= currentlevel;

            if (currentLevelCleared) 
            {
                OnLevelCleared?.Invoke(currentlevel);

                if(currentlevel == SaveAndLoad.SaveGame.maxLevel)
                    SaveAndLoad.SaveLevel(currentlevel + 1, 0);
            }
            else 
            {
                OnLevelFailed?.Invoke(currentlevel);
            }
        }

        [ContextMenu("Test Auto Complete Level 1 Stars")]
        private void AutoWin1Stars() => AutoWin(1);

        [ContextMenu("Test Auto Complete Level 2 Stars")]
        private void AutoWin2Stars() => AutoWin(2);

        [ContextMenu("Test Auto Complete Level 3 Stars")]
        private void AutoWin3Stars() => AutoWin(3);


        private void AutoWin(int stars)
        {
            if (Application.isPlaying)
            {
                SaveAndLoad.SaveLevel(currentlevel, stars);

                SaveAndLoad.LoadAll();

                bool currentLevelCleared = SaveAndLoad.SaveGame.maxLevel >= currentlevel;

                if (currentLevelCleared)
                {
                    OnLevelCleared?.Invoke(currentlevel);

                    if (currentlevel == SaveAndLoad.SaveGame.maxLevel)
                        SaveAndLoad.SaveLevel(currentlevel + 1, 0);
                }
                else
                {
                    OnLevelFailed?.Invoke(currentlevel);
                }
            }
        }
    }
}