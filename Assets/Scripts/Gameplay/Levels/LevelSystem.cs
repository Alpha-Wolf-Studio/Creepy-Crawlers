using UnityEngine;
using Gameplay.Gnomes;
using System;
using System.Collections;
using UnityEngine.Audio;
using Gnomes;

namespace Gameplay.Levels
{
    public class LevelSystem : MonoBehaviour
    {
        [Header("Level Config")]
        [Range(0f,1f)]
        [SerializeField] private float gnomesPercentageNeeded;
        [SerializeField] private int maxGnomesInLevel;
        [SerializeField] private int maxKeysInLevel;
        [SerializeField] private float secondsToStart = 2;

        [Header("Audio Config")]
        [SerializeField] private AudioClip audioClip = null;
        [SerializeField] private AudioMixerGroup audioMixerGroup = null;
        private AudioSource audioSource = null;

        private bool objectiveReached = false;
        private int keysObtained = 0;
        private int starsObtained = 0;
        private int gnomesDeleted = 0;

        public static Action<int> OnLevelStarted;
        public static Action OnGnomesReleased;
        public static Action OnKeysObjectiveReached;

        private void Awake()
        {
            Gnome.onDeleteGnome += Gnome_onDeleteGnome;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void Gnome_onDeleteGnome()
        {
            gnomesDeleted++;
        }

        private void Start()
        {
            OnLevelStarted?.Invoke(maxGnomesInLevel);
            GnomeFinalGate.OnGnomeEntered += AbsorbGnomeData;
            StartSummoning();
        }

        private void OnDestroy()
        {
            GnomeFinalGate.OnGnomeEntered -= AbsorbGnomeData;
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
            starsObtained += gnome.GnomeStats.starAmount;
        }
    }
}