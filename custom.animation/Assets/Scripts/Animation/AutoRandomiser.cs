using UnityEngine;
using System.Collections.Generic;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoRandomizer : MonoBehaviour
    {
        [Header("Random Duration Range")]
        [SerializeField] private float _minDuration = 2f;
        [SerializeField] private float _maxDuration = 10f;
        [SerializeField] private float _durationStep = 0.2f;

        private readonly List<IAutoAnimatable> _autoScripts = new();
        private readonly List<float> _uniqueDurations = new();

        private void Start()
        {
            TrackAutoAnimatableScripts();
            GenerateUniqueDurations();
            AssignDurations();
        }

        private void TrackAutoAnimatableScripts()
        {
            _autoScripts.Clear();
            FindAutoScriptsRecursively(transform);
            Debug.Log($"Found {_autoScripts.Count} auto-animation scripts.");
        }

        private void FindAutoScriptsRecursively(Transform parent)
        {
            foreach (MonoBehaviour script in parent.GetComponents<MonoBehaviour>())
            {
                if (script is IAutoAnimatable autoScript)
                {
                    _autoScripts.Add(autoScript);
                }
            }

            foreach (Transform child in parent)
            {
                FindAutoScriptsRecursively(child);
            }
        }

        private void GenerateUniqueDurations()
        {
            _uniqueDurations.Clear();

            for (float duration = _minDuration; duration <= _maxDuration; duration += _durationStep)
            {
                _uniqueDurations.Add(duration);
            }

            for (int i = 0; i < _uniqueDurations.Count; i++)
            {
                int randomIndex = Random.Range(0, _uniqueDurations.Count);
                (_uniqueDurations[i], _uniqueDurations[randomIndex]) = (_uniqueDurations[randomIndex], _uniqueDurations[i]);
            }
        }

        private void AssignDurations()
        {
            int durationIndex = 0;

            foreach (IAutoAnimatable autoScript in _autoScripts)
            {
                if (autoScript is MonoBehaviour script)
                {
                    float assignedDuration = _uniqueDurations[durationIndex];

                    var durationField = script.GetType().GetField("_sizeDuration") ?? // AutoSize
                                        script.GetType().GetField("_rotationDuration") ?? // AutoRotation
                                        script.GetType().GetField("_scaleDuration") ?? // AutoScale
                                        script.GetType().GetField("_moveDuration") ?? // AutoMove
                                        script.GetType().GetField("_fadeDuration") ?? // AutoAlpha
                                        script.GetType().GetField("_weightDuration"); // AutoParent

                    if (durationField != null)
                    {
                        durationField.SetValue(script, assignedDuration);
                        Debug.Log($"Assigned unique duration {assignedDuration:F1} to {script.gameObject.name} ({script.GetType().Name})");
                    }

                    durationIndex = (durationIndex + 1) % _uniqueDurations.Count;
                }
            }
        }
    }
}
