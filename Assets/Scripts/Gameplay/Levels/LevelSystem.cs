using UnityEngine;
using Gameplay.Gnomes;
using System;
using System.Collections;
using UnityEngine.Audio;
using Gnomes;
using UnityEngine.SceneManagement;
using UI.Gameplay;
using CustomSceneSwitcher.Switcher.Data;
using CustomSceneSwitcher.Switcher.External;
using CustomSceneSwitcher.Switcher;

namespace Gameplay.Levels
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private SceneChangeData sceneChangeData;
        [SerializeField] private SceneReference currentLevel;

        [SerializeField] private SceneReference nextLevel;

        [Header("Level Config")] public static int level = 0;
        [SerializeField] private int minGnomes = 16;
        [SerializeField] private int maxGnomesInLevel;
        [SerializeField] private float secondsToStart = 2;

        [Header("Audio Config")] [SerializeField]
        private AudioClip audioClip = null;

        [SerializeField] private AudioMixerGroup audioMixerGroup = null;
        private AudioSource audioSource = null;

        private bool objectiveReached = false;
        private int keysObtained = 0;
        private int starsObtained = 0;
        private int gnomesAbsorbed = 0;
        private int gnomesDeleted = 0;

        public static Action<int> OnLevelStarted;
        public static Action OnGnomesReleased;
        public static Action OnKeysObjectiveReached;

        private void Awake()
        {
            UIGameflowButtonsHolder.onFinishLevelPressed += UIGameflowButtonsHolder_onFinishLevelPressed;
            GnomeFinalGate.OnGnomeEntered += AbsorbGnomeData;
            Gnome.onDeleteGnome += Gnome_onDeleteGnome;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
            level = SceneManager.GetActiveScene().buildIndex;
        }

        private void Start()
        {
            OnLevelStarted?.Invoke(maxGnomesInLevel);
            StartSummoning();
        }

        private void OnDestroy()
        {
            UIGameflowButtonsHolder.onFinishLevelPressed -= UIGameflowButtonsHolder_onFinishLevelPressed;
            GnomeFinalGate.OnGnomeEntered -= AbsorbGnomeData;
            Gnome.onDeleteGnome -= Gnome_onDeleteGnome;
        }

        private void UIGameflowButtonsHolder_onFinishLevelPressed()
        {
            Gnome[] gnomes = FindObjectsOfType<Gnome>();
            for (int i = 0; i < gnomes.Length; i++)
                gnomes[i].Kill();
        }

        private void Gnome_onDeleteGnome()
        {
            gnomesDeleted++;
            SetResult();
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

        private void AbsorbGnomeData(Gnome gnome)
        {
            gnomesAbsorbed++;
            starsObtained += gnome.GnomeStats.starAmount;
        }

        private void SetResult()
        {
            if (gnomesDeleted != maxGnomesInLevel)
                return;

            if (gnomesAbsorbed >= minGnomes && starsObtained >= 1)
                SaveAndLoad.SaveLevel(level, 3);
            else if (gnomesAbsorbed >= minGnomes)
                SaveAndLoad.SaveLevel(level, 2);
            else
                SaveAndLoad.SaveLevel(level, 1);

            sceneChangeData.SetScene(gnomesAbsorbed > 0 ? nextLevel : currentLevel);
            SceneSwitcher.ChangeScene(sceneChangeData);
        }
    }
}