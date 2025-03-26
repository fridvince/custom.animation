using TMPro;
using UnityEngine;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoTextCounter : MonoBehaviour
    {
        [Header("Counting Range")]
        [SerializeField] private int _from = 0;
        [SerializeField] private int _to = 10;

        [Header("Animation Settings")]
        [SerializeField] private float _duration = 2f; 
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private bool _looping = false;

        [Header("Target Text")]
        [SerializeField] private TMP_Text _tmpText;

        [SerializeField] private bool _startOnAwake = true;

        private float _elapsedTime;
        private bool _isCounting;

        private void Awake()
        {
            if (_tmpText == null)
                _tmpText = GetComponent<TMP_Text>();

            if (_startOnAwake)
                StartCounting();
        }

        private void Update()
        {
            if (_isCounting)
                AnimateCount();
        }

        public void StartCounting()
        {
            _isCounting = true;
            _elapsedTime = 0f;
        }

        public void StopCounting()
        {
            _isCounting = false;
        }

        private void AnimateCount()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _duration)
            {
                if (_looping)
                {
                    _elapsedTime = 0f;
                }
                else
                {
                    StopCounting();
                    return;
                }
            }

            float progress = _elapsedTime / _duration;
            float curvedProgress = _curve.Evaluate(progress);

            int value = Mathf.RoundToInt(Mathf.Lerp(_from, _to, curvedProgress));
            _tmpText.text = value.ToString();
        }
    }
}
