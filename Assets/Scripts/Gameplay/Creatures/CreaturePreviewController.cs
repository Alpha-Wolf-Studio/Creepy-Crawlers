using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class CreaturePreviewController : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> previewSpriteRenderers;

        private readonly int _validPropertyHash = Shader.PropertyToID("_Valid_ID");

        public void SetPreviewState(PreviewState state) 
        {
            MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetInteger(_validPropertyHash, (int)state);

            foreach (var spriteRenderer in previewSpriteRenderers)
            {
                spriteRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public enum PreviewState
        {
            Valid,
            Invalid
        }
    }
}