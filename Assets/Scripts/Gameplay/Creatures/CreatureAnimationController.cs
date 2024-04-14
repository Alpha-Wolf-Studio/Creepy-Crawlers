using System.Collections;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class CreatureAnimationController : MonoBehaviour
    {
        [Header("Spawn Configurations")]
        [SerializeField] private GameObject visual;
        [SerializeField] private SpriteRenderer spawnParticlesRenderer;
        [SerializeField] private Sprite[] spawnParticlesAnimationFrames;
        [SerializeField] private float spawnTime;
        [SerializeField] private float spawnParticlesTime;

        private IEnumerator _spawnAnimationIEnumerator;
        private IEnumerator _spawnParticlesIEnumerator;

        private void OnEnable()
        {
            DoSpawnAnimation();
        }

        private void DoSpawnAnimation()
        {
            DoCreatureSpawnAnimation();
            DoParticlesSpawnAnimation();
        }

        private void DoCreatureSpawnAnimation()
        {
            if (_spawnAnimationIEnumerator != null)
                StopCoroutine(_spawnAnimationIEnumerator);

            _spawnAnimationIEnumerator = SpawningAnimationIEnumerator();

            StartCoroutine(_spawnAnimationIEnumerator);
        }

        private void DoParticlesSpawnAnimation()
        {
            if (_spawnParticlesIEnumerator != null)
                StopCoroutine(_spawnParticlesIEnumerator);

            _spawnParticlesIEnumerator = SpawningParticlesIEnumerator();

            StartCoroutine(_spawnParticlesIEnumerator);
        }

        private IEnumerator SpawningAnimationIEnumerator()
        {
            float t = 0;
            while (t < spawnTime)
            {
                visual.transform.localScale = Vector3.one * Mathf.InverseLerp(0, spawnTime, t);
                t += Time.deltaTime;
                yield return null;
            }
            visual.transform.localScale = Vector3.one;
        }

        private IEnumerator SpawningParticlesIEnumerator()
        {
            if (spawnParticlesRenderer != null && spawnParticlesAnimationFrames != null && spawnParticlesAnimationFrames.Length > 0)
            {
                float frameTime = spawnParticlesTime / spawnParticlesAnimationFrames.Length;
                int currentFrame = 0;

                WaitForSeconds waitTime = new WaitForSeconds(frameTime);

                while (currentFrame < spawnParticlesAnimationFrames.Length)
                {
                    spawnParticlesRenderer.sprite = spawnParticlesAnimationFrames[currentFrame];
                    currentFrame++;
                    yield return waitTime;
                }

                spawnParticlesRenderer.sprite = null;
            }
        }
    }
}