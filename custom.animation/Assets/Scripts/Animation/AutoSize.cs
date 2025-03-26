using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoSize : MonoBehaviour, IAutoAnimatable
    {
        [Header("Size Curves")]
        [SerializeField] private AnimationCurve _minWidthCurve = AnimationCurve.Linear(0, 1, 1, 1);
        [SerializeField] private AnimationCurve _minHeightCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Header("Size Settings")]
        [Tooltip("The duration of one sizing cycle in seconds.")]
        [SerializeField, Min(0.01f)] private float _sizeDuration = 1.0f;

        [Tooltip("If true, the size animation will loop indefinitely.")]
        [SerializeField] private bool _loop;

        [Tooltip("If true, sizing will start automatically on Awake.")]
        [SerializeField] private bool _startOnAwake;

        [Header("Target Objects")]
        [Tooltip("Objects to animate (must have LayoutElement).")]
        [SerializeField] private List<GameObject> _targetObjects = new();

        private float _timeElapsed;
        private bool _isSizing;
        private Dictionary<GameObject, (float, float)> _originalSizes = new();

        public bool IsSizing => _isSizing;

        private void Awake()
        {
            foreach (var obj in _targetObjects)
            {
                if (obj.TryGetComponent(out LayoutElement layout))
                {
                    _originalSizes[obj] = (layout.minWidth, layout.minHeight);
                }
            }

            if (_startOnAwake)
            {
                StartAnimation();
            }
        }

        private void Update()
        {
            if (_isSizing)
            {
                AnimateSize();
            }
        }

        public void StartAnimation()
        {
            _isSizing = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        public void StopAnimation()
        {
            _isSizing = false;
            enabled = false;
        }

        public void ResetSize()
        {
            StopAnimation();
            foreach (var obj in _targetObjects)
            {
                if (obj.TryGetComponent(out LayoutElement layout) && _originalSizes.ContainsKey(obj))
                {
                    (float originalWidth, float originalHeight) = _originalSizes[obj];
                    layout.minWidth = originalWidth;
                    layout.minHeight = originalHeight;
                }
            }
        }

        private void AnimateSize()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _sizeDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopAnimation();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _sizeDuration;
            float minWidth = _minWidthCurve.Evaluate(normalizedTime);
            float minHeight = _minHeightCurve.Evaluate(normalizedTime);

            foreach (var obj in _targetObjects)
            {
                if (obj.TryGetComponent(out LayoutElement layout))
                {
                    layout.minWidth = minWidth;
                    layout.minHeight = minHeight;
                }
            }
        }
    }
}
