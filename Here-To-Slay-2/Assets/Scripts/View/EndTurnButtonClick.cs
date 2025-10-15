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
    public class EndTurnButtonClick : MonoBehaviour
    {
        public Button Button;

        public event EventHandler EndTurnButtonClicked;

        void Start()
        {
            Button.onClick.AddListener(OnEndTurnButtonClicked);
        }

        void Update()
        {

        }

        void OnEndTurnButtonClicked()
        {
            EndTurnButtonClicked?.Invoke(this, new EventArgs());
            Debug.Log("EndTurnButtonClick.OnEndTurnButtonClicked: Button clicked");
        }
    }
}
