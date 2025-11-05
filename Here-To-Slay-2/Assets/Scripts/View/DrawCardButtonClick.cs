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
    public class DrawCardButtonClick : MonoBehaviour
    {
        public Button Button;

        public event EventHandler DrawCardButtonClicked;

        void Start()
        {
            Button.onClick.AddListener(OnDrawCardButtonClicked);
        }

        void Update()
        {

        }

        void OnDrawCardButtonClicked()
        {
            DrawCardButtonClicked?.Invoke(this, new EventArgs());
            Debug.Log("DrawCardButtonClick.OnDrawCardButtonClicked: Button clicked");
        }
    }
}
