using UnityEngine;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoKiller : MonoBehaviour
    {
        [Header("Target Container")]
        public Transform _targetContainer;

        [Header("Disable Settings")]
        public bool _disableAutoRotation;
        public bool _disableAutoScale;
        public bool _disableAutoMove;
        public bool _disableAutoAlpha;

        private void Start()
        {
            if (_targetContainer != null)
            {
                DisableAutoAnimationsInChildren(_targetContainer);
            }
            else
            {
                Debug.LogWarning("No target container assigned!");
            }
        }

        private void DisableAutoAnimationsInChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                DisableAutoAnimations(child);
                DisableAutoAnimationsInChildren(child);
            }
        }

        private void DisableAutoAnimations(Transform item)
        {
            if (_disableAutoRotation)
            {
                AutoRotation rotationScript = item.GetComponent<AutoRotation>();
                if (rotationScript != null) 
                {
                    rotationScript.enabled = false;
                }
            }

            if (_disableAutoScale)
            {
                AutoScale scaleScript = item.GetComponent<AutoScale>();
                if (scaleScript != null) 
                {
                    scaleScript.enabled = false;
                }
            }

            if (_disableAutoMove)
            {
                AutoMove moveScript = item.GetComponent<AutoMove>();
                if (moveScript != null) 
                {
                    moveScript.enabled = false;
                }
            }

            if (_disableAutoAlpha)
            {
                AutoAlpha alphaScript = item.GetComponent<AutoAlpha>();
                if (alphaScript != null)
                {
                    alphaScript.enabled = false;
                    SetAlphaToOne(item);
                }
            }
        }

        private void SetAlphaToOne(Transform item)
        {
            CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
    }
}
