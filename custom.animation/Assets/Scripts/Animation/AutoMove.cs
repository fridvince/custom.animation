using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoMove : MonoBehaviour
    {
        [Header("Position Curves")]
        [SerializeField] private AnimationCurve _xPositionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _yPositionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _zPositionCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Position Settings")]
        [Tooltip("The duration of one movement cycle in seconds.")]
        [SerializeField, Min(0.01f)] public float _moveDuration = 1.0f;

        [Tooltip("If true, the movement animation will loop indefinitely.")]
        [SerializeField] private bool _loop;

        [Tooltip("If true, movement will start automatically on Awake.")]
        [SerializeField] private bool _startOnAwake;
    
        [Tooltip("If true, movement will start automatically on OnEnable.")]
        [SerializeField] private bool _startOnEnable;

        [Header("Target Settings")]
        [Tooltip("The list of transforms to animate. Leave empty to animate this object only.")]
        [SerializeField] private List<Transform> _targetTransforms;

        private float _timeElapsed;
        private bool _isMoving;
        private readonly Dictionary<Transform, Vector3> _originalPositions = new();

        public bool IsMoving => _isMoving;

        private void Awake()
        {
            if (_targetTransforms.Count == 0)
            {
                _targetTransforms.Add(transform);
            }

            foreach (Transform target in _targetTransforms)
            {
                if (target != null)
                {
                    _originalPositions.TryAdd(target, target.localPosition);
                }
            }

            if (_startOnAwake)
            {
                StartMoving();
            }
        }

        public void OnEnable()
        {
            if (_startOnEnable)
            {
                ResetPosition();
                StartMoving();
            }
        }

        private void Update()
        {
            if (_isMoving)
            {
                AnimatePosition();
            }
        }

        public void StartMoving()
        {
            _isMoving = true;
            _timeElapsed = 0f;
            enabled = true;
        }

        [UsedImplicitly]
        public void StopMoving()
        {
            _isMoving = false;
        }

        public void ResetPosition()
        {
            StopMoving();
            foreach (Transform target in _targetTransforms)
            {
                if (target != null && _originalPositions.TryGetValue(target, out Vector3 originalPosition))
                {
                    target.localPosition = originalPosition;
                }
            }
        }

        private void AnimatePosition()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _moveDuration)
            {
                if (_loop)
                {
                    _timeElapsed = 0f;
                }
                else
                {
                    StopMoving();
                    return;
                }
            }

            float normalizedTime = _timeElapsed / _moveDuration;
            float xPosition = _xPositionCurve.Evaluate(normalizedTime);
            float yPosition = _yPositionCurve.Evaluate(normalizedTime);
            float zPosition = _zPositionCurve.Evaluate(normalizedTime);

            Vector3 newPosition = new(xPosition, yPosition, zPosition);

            foreach (Transform target in _targetTransforms)
            {
                if (target != null)
                {
                    target.localPosition = newPosition;
                }
            }
        }
    }
}