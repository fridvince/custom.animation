using UnityEngine;
using UnityEngine.UI;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoScroll : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private int _desiredAmount = 4;
        [SerializeField] private int _previousChildCount;
        [SerializeField] private ScrollRect _scrollComponent;
        [SerializeField] private Transform _contentPanel;

        void Start()
        {
            UpdateScrollComponentState();
        }

        void Update()
        {
            if (_contentPanel.childCount != _previousChildCount)
            {
                UpdateScrollComponentState();
                _previousChildCount = _contentPanel.childCount;
            }
        }

        private void UpdateScrollComponentState()
        {
            bool shouldEnable = _contentPanel.childCount >= _desiredAmount;
            _scrollComponent.enabled = shouldEnable;
        }
    }
}
