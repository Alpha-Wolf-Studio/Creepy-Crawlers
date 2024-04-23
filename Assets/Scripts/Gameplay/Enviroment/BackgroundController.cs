using UnityEngine;

namespace Gameplay.Enviroment 
{
    public class BackgroundController : MonoBehaviour
    {
        Vector2Int screenResolution = Vector2Int.one;

        private void Update()
        {
            if(screenResolution.x != Screen.width || screenResolution.y != Screen.height) 
            {
                MatchBackgroundSizeToCamera();
            }
        }

        private void MatchBackgroundSizeToCamera() 
        {
            Camera mainCamera = Camera.main;
            screenResolution = new Vector2Int(Screen.width, Screen.height);

            float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

            Vector2 scale = new Vector2(1, 1);

            if (mainCamera.orthographic)
            {
                scale.y = mainCamera.orthographicSize / 5f;
            }
            else
                scale.y = 2.0f * Mathf.Tan(0.5f * mainCamera.fieldOfView * Mathf.Deg2Rad) * distanceToCamera / 10f;

            scale.x = scale.y * mainCamera.aspect;

            transform.localScale = new Vector2(scale.x, scale.y);
            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        }

#if UNITY_EDITOR
        [ContextMenu("Match background size to camera")]
        private void MatchBackgroundSizeToCameraEditor()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            MatchBackgroundSizeToCamera();
        }
#endif
    }
}
