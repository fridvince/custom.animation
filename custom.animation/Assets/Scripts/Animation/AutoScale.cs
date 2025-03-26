using System.Collections.Generic;
using UnityEngine;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoScale : MonoBehaviour, IAutoAnimatable
    {
        [Header("Scale Curves")]
        [SerializeField] private AnimationCurve _xScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        [SerializeField] private AnimationCurve _yScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
        [SerializeField] private AnimationCurve _zScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Header("Scale Settings")]
        [SerializeField, Min(0.01f)] public float _scaleDuration = 1.0f;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _startOnAwake;

        [Header("Target Settings")]
        [SerializeField] private List<Transform> _targetTransforms = new List<Transform>();

        private float _timeElapsed;
        private bool _isScaling;
        private Vector3 _originalScale;

        public bool IsScaling => _isScaling;

        private void Awake()
        {
            _originalScale = transform.localScale;

            if (_targetTransforms.Count == 0)
            {
                _targetTransforms.Add(transform);
            }

            if (_startOnAwake)
            {
                StartScaling();
            }
        }

        private void Update()
        {
            if (_isScaling)
            {
                AnimateScale();
            }
        }

        public void StartAnimation()
        {
            StartScaling();
        }

        public void StartScaling()
        {
            _isScaling = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        public void StopScaling()
        {
            _isScaling = false;
            enabled = false;
        }

        public void ResetScale()
        {
            StopScaling();
            foreach (var target in _targetTransforms)
            {
                if (target != null)
                {
                    target.localScale = _originalScale;
                }
            }
        }

        private void AnimateScale()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _scaleDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopScaling();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _scaleDuration;
            float xScale = _xScaleCurve.Evaluate(normalizedTime);
            float yScale = _yScaleCurve.Evaluate(normalizedTime);
            float zScale = _zScaleCurve.Evaluate(normalizedTime);

            Vector3 newScale = new Vector3(xScale, yScale, zScale);

            foreach (var target in _targetTransforms)
            {
                if (target != null)
                {
                    target.localScale = newScale;
                }
            }
        }
    }
}
