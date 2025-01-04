using UnityEngine;
using UnityEngine.UI;
using Components.Enums;
using Components.CustomEventArgs;
using System;

namespace View
{
    /// <summary>
    /// Tests if card draw is working
    /// </summary>
    public class ButtonGetterButtonClick : MonoBehaviour
    {
        public Button Button;
        public int Id;
        public event EventHandler<ButtonGetterButtonGlickedEventArgs> ButtonGetterButtonClicked;

        void Start()
        {
            Button.onClick.AddListener(OnButtonGetterButtonClicked);
            Id = 21;
        }

        void Update()
        {

        }

        void OnButtonGetterButtonClicked()
        {
            ButtonGetterButtonClicked?.Invoke(this, new ButtonGetterButtonGlickedEventArgs { Id = Id });
            Debug.Log($"ButtonGetterButtonClicked: {Id}");
        }
    }
}
