using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    [RequireComponent(typeof(ParentConstraint))]
    public class AutoParent : MonoBehaviour, IAutoAnimatable
    {
        [Header("Weight Curve")]
        [SerializeField] private AnimationCurve _weightCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Header("Animation Settings")]
        [SerializeField, Min(0.01f)] public float _weightDuration = 1.0f;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _startOnAwake;
        [SerializeField] private bool _startOnEnable;

        private ParentConstraint _constraint;
        private float _timeElapsed;
        private bool _isAnimating;
        private float _originalWeight;

        public bool IsAnimating => _isAnimating;

        private void Awake()
        {
            _constraint = GetComponent<ParentConstraint>();

            if (_constraint == null)
            {
                Debug.LogWarning("No ParentConstraint found.");
                enabled = false;
                return;
            }

            _originalWeight = _constraint.weight;

            if (_startOnAwake)
            {
                StartAnimation();
            }
        }

        private void OnEnable()
        {
            if (_startOnEnable)
            {
                ResetWeight();
                StartAnimation();
            }
        }

        private void Update()
        {
            if (_isAnimating)
            {
                AnimateWeight();
            }
        }

        public void StartAnimation()
        {
            if (_constraint == null)
                return;

            _isAnimating = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        [UsedImplicitly]
        public void StopAnimation()
        {
            _isAnimating = false;
        }

        [UsedImplicitly]
        public void ResetWeight()
        {
            StopAnimation();
            ApplyWeightValue(_originalWeight);
        }

        private void AnimateWeight()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _weightDuration)
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

            float normalizedTime = _timeElapsed / _weightDuration;
            float weightValue = Mathf.Clamp01(_weightCurve.Evaluate(normalizedTime));
            ApplyWeightValue(weightValue);
        }

        private void ApplyWeightValue(float value)
        {
            if (_constraint == null || !_constraint.isActiveAndEnabled)
                return;

            _constraint.weight = Mathf.Clamp01(value);
        }
    }
}