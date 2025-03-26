using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoColor : MonoBehaviour
    {
        [Header("Color Settings")]
        [SerializeField] private Gradient _colorGradient = new Gradient();

        [Header("Animation Settings")]
        [Tooltip("The duration of one color change cycle in seconds.")]
        [SerializeField, Min(0.01f)] private float _animationDuration = 1.0f;

        [Tooltip("If true, the color animation will loop indefinitely.")]
        [SerializeField] private bool _loop;

        [Tooltip("If true, color animation will start automatically on Awake.")]
        [SerializeField] private bool _startOnAwake;

        [Tooltip("If true, color animation will start automatically on Enable.")]
        [SerializeField] private bool _startOnEnable;

        [Header("Target Settings")]
        [Tooltip("The list of Image components to animate. Leave empty to animate this object's Image component only.")]
        [SerializeField] private List<Image> _targetImages;

        private float _timeElapsed;
        private bool _isAnimating;
        private Color _originalColor;

        public bool IsAnimating => _isAnimating;

        private void Awake()
        {
            if (_targetImages.Count == 0)
            {
               Image image = GetComponent<Image>();
                if (image != null)
                {
                    _originalColor = image.color;
                    _targetImages.Add(image);
                }
                else
                {
                    Debug.LogWarning("No Image component found on the object, and no targets assigned.");
                }
            }

            if (_startOnAwake)
            {
                StartColorAnimation();
            }
        }

        private void OnEnable()
        {
            if (_startOnEnable)
            {
                ResetColor();
                StartColorAnimation();
            }
        }

        private void Update()
        {
            if (_isAnimating)
            {
                AnimateColor();
            }
        }
    
        [UsedImplicitly]
        public void StartColorAnimation()
        {
            _isAnimating = true;
            _timeElapsed = 0f;
        }

        [UsedImplicitly]
        public void StopColorAnimation()
        {
            _isAnimating = false;
            EvaluateAndApplyColor(1);
        }

        [UsedImplicitly]
        public void ResetColor()
        {
            StopColorAnimation();
            foreach (Image target in _targetImages)
            {
                if (target != null)
                {
                    target.color = _originalColor;
                }
            }
        }

        private void AnimateColor()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _animationDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopColorAnimation();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _animationDuration;
            EvaluateAndApplyColor(normalizedTime);
        }

        private void EvaluateAndApplyColor(float normalizedTime)
        {
            Color colorValue = _colorGradient.Evaluate(normalizedTime);

            foreach (Image target in _targetImages)
            {
                if (target != null)
                {
                    target.color = colorValue;
                }
            }
        }
    }
}
