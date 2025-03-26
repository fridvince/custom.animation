using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoSprite : MonoBehaviour
    {
        [Header("Animation Settings")]
        [Tooltip("Animation speed as a value between 0 (0 FPS) and 1 (64 FPS).")]
        [SerializeField, Range(0f, 1f)] private float _animationSpeed = 0.5f;

        [Tooltip("If true, the animation will loop indefinitely.")]
        [SerializeField] private bool _loop;

        [Tooltip("If true, the animation will start automatically on Awake.")]
        [SerializeField] private bool _startOnAwake;

        [Tooltip("If true, the animation will start automatically on OnEnable.")]
        [SerializeField] private bool _startOnEnable;

        [Tooltip("The duration of one animation cycle in seconds. Overrides animation speed when used with randomizer.")]
        [SerializeField] public float _animationDuration;

        [Header("Sprite Folder Path")]
        [Tooltip("Path to the folder containing the sprite images, relative to Resources.")]
        [SerializeField] private string _spriteFolderPath = "Sprites/Fire";  // Example default path

        private List<Sprite> _frames = new List<Sprite>();
        private Image _imageComponent;
        private float _timePerFrame;
        private float _timeElapsed;
        private int _currentFrameIndex;
        private bool _isAnimating;

        public bool IsAnimating => _isAnimating;

        private void Awake()
        {
            _imageComponent = GetComponent<Image>();
            if (_imageComponent == null)
            {
                Debug.LogError("AutoSprite requires an Image component on the same GameObject.");
                enabled = false;
                return;
            }

            LoadFrames();

            if (_frames.Count == 0)
            {
                Debug.LogWarning("No frames loaded. Animation will not play.");
                enabled = false;
                return;
            }

            UpdateTimePerFrame();

            if (_startOnAwake)
            {
                StartAnimating();
            }
        }

        private void OnEnable()
        {
            if (_startOnEnable)
            {
                ResetAnimation();
                StartAnimating();
            }
        }

        private void Update()
        {
            if (_isAnimating)
            {
                AnimateFrame();
            }
        }

        public void StartAnimating()
        {
            _isAnimating = true;
            _timeElapsed = 0f;
            _currentFrameIndex = 0;
            enabled = true;
            if (_frames.Count > 0)
            {
                _imageComponent.sprite = _frames[0];
            }
        }

        public void StopAnimating()
        {
            _isAnimating = false;
        }

        public void ResetAnimation()
        {
            StopAnimating();
            _currentFrameIndex = 0;
            _timeElapsed = 0f;
            if (_frames.Count > 0)
            {
                _imageComponent.sprite = _frames[0];
            }
        }

        private void AnimateFrame()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed >= _timePerFrame)
            {
                _timeElapsed -= _timePerFrame;
                _currentFrameIndex++;

                if (_currentFrameIndex >= _frames.Count)
                {
                    if (_loop)
                    {
                        _currentFrameIndex = 0;
                    }
                    else
                    {
                        StopAnimating();
                        return;
                    }
                }

                if (_currentFrameIndex < _frames.Count)
                {
                    _imageComponent.sprite = _frames[_currentFrameIndex];
                }
            }
        }

        private void LoadFrames()
        {
            // Load sprites from the user-specified folder inside Resources
            if (string.IsNullOrEmpty(_spriteFolderPath))
            {
                Debug.LogWarning("Sprite folder path is not set.");
                return;
            }

            // Ensure the path is correctly formatted and load the sprites
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>(_spriteFolderPath);

            if (loadedSprites.Length > 0)
            {
                _frames.AddRange(loadedSprites);
            }
            else
            {
                Debug.LogWarning($"No sprites found in Resources/{_spriteFolderPath}. Animation will not play.");
            }
        }

        private void UpdateTimePerFrame()
        {
            if (_frames.Count == 0)
            {
                return;
            }

            if (_animationDuration > 0)
            {
                _timePerFrame = _animationDuration / _frames.Count;
            }
            else
            {
                float fps = Mathf.Lerp(0f, 64f, _animationSpeed);
                _timePerFrame = fps > 0 ? 1f / fps : float.MaxValue;
            }

            Debug.Log($"Time Per Frame updated: {_timePerFrame:F3} seconds per frame.");
        }

        private void OnValidate()
        {
            UpdateTimePerFrame();

            if (_frames.Count > 0 && _imageComponent != null)
            {
                ResetAnimation();
                _imageComponent.sprite = _frames[_currentFrameIndex];
            }
        }
    }
}
