using UnityEngine;
using UnityEngine.UI;
using Components.Enums;
using Components.CustomEventArgs;
using System;
namespace View
{
    /// <summary>
    /// Tests if button click can move a given card to a given deck
    /// </summary>
    public class TestClick : MonoBehaviour
    {
        public int CardId;
        public Deck Deck;
        public Button Button;

        public event EventHandler<TestButtonClickedEventArgs> TestButtonClicked;
        
        void Start()
        {
            Button.onClick.AddListener(OnTestButtonClicked);
            CardId = 21;
            Deck = Deck.AttackableMonsters;
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnTestButtonClicked()
        {
            TestButtonClicked?.Invoke(Deck, new TestButtonClickedEventArgs { CardId = CardId, Deck = Deck });
            CardId++;
            Debug.Log("TestClick: TestButtonClicked: Button clicked");
        }
    }
}

