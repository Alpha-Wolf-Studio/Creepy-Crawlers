using System.Collections;
using UnityEngine;
using Gameplay.Levels;

namespace Gameplay.Gnomes
{
    public class GnomesSpawner : MonoBehaviour
    {
        private Transform content;
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private GameObject prefabGnome;
        [SerializeField] private int spawnAmount = 10;

        [SerializeField] private int rows;
        [SerializeField] private float rowSpawnInterval = 5f;

        [Header("Audio Config")]
        [SerializeField] private AudioClip audioClip = null;
        [SerializeField] private UnityEngine.Audio.AudioMixerGroup audioMixerGroup = null;
        private AudioSource audioSource = null;

        private void Awake()
        {
            LevelSystem.OnLevelStarted += SetMaxGnomes;
            LevelSystem.OnGnomesReleased += SpawnAllGnomes;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = false;
            content = new GameObject().transform;
            content.name = "Content";
        }

        private void OnDestroy()
        {
            LevelSystem.OnLevelStarted -= SetMaxGnomes;
            LevelSystem.OnGnomesReleased -= SpawnAllGnomes;
        }

        private void SetMaxGnomes(int level, int gnomesInLevel)
        {
            spawnAmount = gnomesInLevel;
        }

        public void SpawnAllGnomes()
        {
            StartCoroutine(SpawnAllGnomes_CT());
            //StartCoroutine(SpawnAllGnomesInRows_CT());
        }

        private IEnumerator SpawnAllGnomes_CT()
        {
            while (spawnAmount > 0)
            {
                SpawnGnome();
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return null;
        }

        private IEnumerator SpawnAllGnomesInRows_CT()
        {
            int gnomesPerRow = spawnAmount / rows;
            int extraGnomes = spawnAmount % rows;

            for (int i = 0; i < rows; i++)
            {
                int gnomesToSpawn = gnomesPerRow + (i < extraGnomes ? 1 : 0);

                for (int j = 0; j < gnomesToSpawn; j++)
                {
                    SpawnGnome();
                    yield return new WaitForSeconds(spawnInterval);
                }

                if (i < rows - 1)
                {
                    yield return new WaitForSeconds(rowSpawnInterval);
                }
            }
        }

        private void SpawnGnome()
        {
            GameObject go = Instantiate(prefabGnome, transform.position, Quaternion.identity, content);
            audioSource.Play();
            spawnAmount--;
        }
    }
}
