using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace fridvince.Game.Common
{
    [RequireComponent(typeof(AutoHeader))]
    public class BaseButton : Selectable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Serializable]
        public class BaseButtonClickedEvent : UnityEvent { }

        [SerializeField] private BaseButtonClickedEvent _onClick = new();
        [SerializeField] private UnityEvent _onPointerDownEvent = new();
        [SerializeField] private UnityEvent _onPointerUpEvent = new();
        
        [Header("Toggle Elements (Multiple Allowed)")]
        [SerializeField] private List<GameObject> _toggleElements = new();
        
        [Header("Optional Scale on Press")]
        [SerializeField] private Transform _scalingContainer;
        [SerializeField] private float _pressedScale = 1.0f;
        private Vector3 _originalScale;

        [Header("Checkmark/Selection State")]
        public bool IsSelected = false;

        public UnityEvent OnPointerDownEvent => _onPointerDownEvent;
        public UnityEvent OnPointerUpEvent => _onPointerUpEvent;
        public BaseButtonClickedEvent OnClick => _onClick;

        protected override void OnEnable()
        {
            base.OnEnable();
            _originalScale = _scalingContainer != null ? _scalingContainer.localScale : transform.localScale;
            SetToggleState(IsSelected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            Press();
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            _onClick.Invoke();
            SetToggleState(true);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDownEvent?.Invoke();
            base.OnPointerDown(eventData);
            ScaleButton(true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUpEvent?.Invoke();
            base.OnPointerUp(eventData);
            ScaleButton(false);
        }

        private void ScaleButton(bool isPressed)
        {
            if (_scalingContainer == null)
            {
                _scalingContainer = transform;
            }
            _scalingContainer.localScale = isPressed ? _originalScale * _pressedScale : _originalScale;
        }

        public void SetToggleState(bool isActive)
        {
            foreach (var element in _toggleElements)
            {
                if (element != null)
                {
                    element.SetActive(isActive);
                }
            }
        }

        public void DeselectButton()
        {
            IsSelected = false;
            SetToggleState(false);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }
    }
}
