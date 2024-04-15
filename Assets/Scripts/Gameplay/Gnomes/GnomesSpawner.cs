#if UNITY_EDITOR
using Gameplay.Levels;
using System.Collections;
using UnityEditor;
#endif
using UnityEngine;
using Utils;

namespace Gameplay.Gnomes
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GnomesSpawner))]
    public class GnomesSpawnerEditor : Editor
    {
        SerializedProperty spawnAllInOneRowProp;
        SerializedProperty objectPoolProp;
        SerializedProperty spawnIntervalProp;
        SerializedProperty spawnPositionProp;
        SerializedProperty rowsProp;
        SerializedProperty rowSpawnIntervalProp;

        SerializedProperty audioClipProp;
        SerializedProperty audioMixerGroupProp;

        private void OnEnable()
        {
            spawnAllInOneRowProp = serializedObject.FindProperty("spawnInOneRow");
            objectPoolProp = serializedObject.FindProperty("gnomesPool");
            spawnIntervalProp = serializedObject.FindProperty("spawnInterval");
            spawnPositionProp = serializedObject.FindProperty("spawnPosition");
            rowsProp = serializedObject.FindProperty("rows");
            rowSpawnIntervalProp = serializedObject.FindProperty("rowSpawnInterval");
            
            audioClipProp = serializedObject.FindProperty("audioClip");
            audioMixerGroupProp = serializedObject.FindProperty("audioMixerGroup");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(spawnAllInOneRowProp);
            EditorGUILayout.PropertyField(objectPoolProp);


            EditorGUILayout.PropertyField(spawnIntervalProp);
            EditorGUILayout.PropertyField(spawnPositionProp);

            if (!spawnAllInOneRowProp.boolValue)
            {
                EditorGUILayout.PropertyField(rowsProp);
                EditorGUILayout.PropertyField(rowSpawnIntervalProp);
            }

            EditorGUILayout.PropertyField(audioClipProp);
            EditorGUILayout.PropertyField(audioMixerGroupProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    public class GnomesSpawner : MonoBehaviour
    {
        [SerializeField] private bool spawnInOneRow = true;

        [SerializeField] private ObjectPool gnomesPool;
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private Vector3 spawnPosition;

        private int spawnQuantity = 0;

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
        }

        private void OnDestroy()
        {
            LevelSystem.OnLevelStarted -= SetMaxGnomes;
            LevelSystem.OnGnomesReleased -= SpawnAllGnomes;
        }

        private void SetMaxGnomes(int max)
        {
            spawnQuantity = max;
        }

        public void SpawnAllGnomes()
        {
            if (spawnInOneRow)
            {
                StartCoroutine(SpawnAllGnomes_CT());
            }
            else
            {
                StartCoroutine(SpawnAllGnomesInRows_CT());
            }
        }

        private IEnumerator SpawnAllGnomes_CT()
        {
            while (spawnQuantity > 0)
            {
                SpawnGnome();
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return null;
        }

        private IEnumerator SpawnAllGnomesInRows_CT()
        {
            int gnomesPerRow = spawnQuantity / rows;
            int extraGnomes = spawnQuantity % rows;

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
            gnomesPool.GetObjectFromPool().transform.position = this.transform.position + spawnPosition;
            
            audioSource.Play();

            spawnQuantity--;
        }
    }
}
