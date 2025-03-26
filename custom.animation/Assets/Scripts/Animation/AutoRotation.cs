using UnityEngine;
using System.Collections.Generic;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoRotation : MonoBehaviour, IAutoAnimatable
    {
        [Header("Rotation Curves")]
        [SerializeField] private AnimationCurve _xRotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _yRotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _zRotationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Rotation Settings")]
        [Tooltip("The duration of one rotation cycle in seconds.")]
        [SerializeField, Min(0.01f)] private float _rotationDuration = 1.0f;

        [Tooltip("If true, the rotation animation will loop indefinitely.")]
        [SerializeField] private bool _loop;

        [Tooltip("If true, rotation will start automatically on Awake.")]
        [SerializeField] private bool _startOnAwake;

        [Header("Target Settings")]
        [Tooltip("The list of transforms to rotate. Leave empty to rotate this object only.")]
        [SerializeField] private List<Transform> _targetTransforms = new();

        private float _timeElapsed;
        private bool _isRotating;
        private Dictionary<Transform, Quaternion> _originalRotations = new();

        public bool IsRotating => _isRotating;

        private void Awake()
        {
            if (_targetTransforms.Count == 0)
            {
                _targetTransforms.Add(transform);
            }

            foreach (var target in _targetTransforms)
            {
                if (target != null)
                {
                    _originalRotations.TryAdd(target, target.localRotation);
                }
            }

            if (_startOnAwake)
            {
                StartAnimation();
            }
        }

        private void Update()
        {
            if (_isRotating)
            {
                AnimateRotation();
            }
        }

        public void StartAnimation()
        {
            _isRotating = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        public void StopRotation()
        {
            _isRotating = false;
            enabled = false;
        }

        public void ResetRotation()
        {
            StopRotation();
            foreach (var target in _targetTransforms)
            {
                if (target != null && _originalRotations.TryGetValue(target, out Quaternion rotation))
                {
                    target.localRotation = rotation;
                }
            }
        }

        private void AnimateRotation()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _rotationDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopRotation();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _rotationDuration;
            float xRotation = _xRotationCurve.Evaluate(normalizedTime);
            float yRotation = _yRotationCurve.Evaluate(normalizedTime);
            float zRotation = _zRotationCurve.Evaluate(normalizedTime);

            Quaternion newRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            foreach (var target in _targetTransforms)
            {
                if (target != null)
                {
                    target.localRotation = newRotation;
                }
            }
        }
    }
}
