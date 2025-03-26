using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    [RequireComponent(typeof(CanvasGroup))]
    public class AutoAlpha : MonoBehaviour, IAutoAnimatable
    {
        [Header("Alpha Curve")]
        [SerializeField] private AnimationCurve _alphaCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Header("Alpha Settings")]
        [SerializeField, Min(0.01f)] internal float _fadeDuration = 1.0f;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _startOnAwake;
        [SerializeField] private bool _startOnEnable;

        [Header("Target Settings")]
        [SerializeField] private List<CanvasGroup> _targetCanvasGroups;

        private float _timeElapsed;
        private bool _isFading;
        private float _originalAlpha;

        public bool IsFading => _isFading;

        private void Awake()
        {
            if (_targetCanvasGroups.Count == 0)
            {
                CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    _originalAlpha = canvasGroup.alpha;
                    _targetCanvasGroups.Add(canvasGroup);
                }
                else
                {
                    Debug.LogWarning("No CanvasGroup found on the object, and no targets assigned.");
                }
            }

            if (_startOnAwake)
            {
                StartFading();
            }
        }

        private void OnEnable()
        {
            if (_startOnEnable)
            {
                ResetAlpha();
                StartFading();
            }
        }

        private void Update()
        {
            if (_isFading)
            {
                AnimateAlpha();
            }
        }

        public void StartAnimation()
        {
            StartFading();
        }

        [UsedImplicitly]
        public void StartFading()
        {
            _isFading = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        [UsedImplicitly]
        public void StopFading()
        {
            _isFading = false;
        }

        [UsedImplicitly]
        public void ResetAlpha()
        {
            StopFading();
            ApplyAlphaValue(_originalAlpha);
        }

        private void AnimateAlpha()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _fadeDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopFading();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _fadeDuration;
            float alphaValue = _alphaCurve.Evaluate(normalizedTime);
            
            ApplyAlphaValue(alphaValue);
        }

        private void ApplyAlphaValue(float value)
        {
            foreach (CanvasGroup target in _targetCanvasGroups)
            {
                if (target != null)
                {
                    target.alpha = value;
                }
            }
        }
    }
}
