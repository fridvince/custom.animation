using System.Collections.Generic;
using UnityEngine;

namespace fridvince.Game.Common
{
    [RequireComponent(typeof(AutoHeader))]
    public class BaseButtonsGroup : MonoBehaviour
    {
        private List<BaseButton> _buttons = new();

        private void Awake()
        {
            FindAllBaseButtons();
        }

        private void FindAllBaseButtons()
        {
            _buttons.Clear();
            foreach (Transform child in transform)
            {
                BaseButton[] buttonsInContainer = child.GetComponentsInChildren<BaseButton>();
        
                foreach (var button in buttonsInContainer)
                {
                    _buttons.Add(button);
                    Debug.Log($"Button found: {button.name}");
                    button.OnClick.AddListener(() => OnButtonClicked(button));
                }

                if (buttonsInContainer.Length == 0)
                {
                    Debug.LogWarning($"No BaseButton found under container: {child.name}");
                }
            }
        }


        private void OnButtonClicked(BaseButton clickedButton)
        {
            Debug.Log($"Button clicked: {clickedButton.name}");

            foreach (var button in _buttons)
            {
                if (button != clickedButton)
                {
                    button.DeselectButton();
                }
            }

            clickedButton.IsSelected = true;
            clickedButton.SetToggleState(true);
        }
    }
}
