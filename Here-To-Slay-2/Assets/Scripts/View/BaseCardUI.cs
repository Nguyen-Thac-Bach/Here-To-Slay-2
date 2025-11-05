using NUnit.Framework;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

using Components.Enums;
namespace View {
    /// <summary>
    /// used to find out which deck the card belongs to. Added to all card prefabs, not just hero cards
    /// </summary>
    public class BaseCardUI : MonoBehaviour
    {

        #region Fields
        private const string _buttonLocation = "Overlay";

        /// <summary>
        /// needed for GameStateUI to find the card easily
        /// </summary>
        private Deck _deck;
        private int _id;
        private bool _isFaceUp;
        private bool _isTopCardInDrawDeck;
        #endregion

        public Deck Deck { get => _deck; }
        public int Id { get => _id; }
        public bool IsFaceUp { get => _isFaceUp; }
        public bool IsTopCardInDrawDeck { get => _isTopCardInDrawDeck; }



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        /// <summary>
        /// Might need to disable it if performance is an issue. It only needs 1 update after all
        /// </summary>
        void Update()
        {
        }

        #region Public methods
        public void SetDeck(Deck deck)
        {
            _deck = deck;
            Debug.Log($"BaseCardUI.SetDeck: Deck set to {_deck}");
        }
        /// <summary>
        /// Only set it at the start of the game, when the card is created
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            _id = id;
            Debug.Log($"BaseCardUI.SetId: Id set to {_id}");
        }
        public void SetIsFaceUp(bool isFaceUp)
        {
            _isFaceUp = isFaceUp;
            Debug.Log($"BaseCardUI.SetIsFaceUp: IsFaceUp set to {_isFaceUp}");
        }
        public void SetIsTopCardInDrawDeck(bool isTopCardInDrawDeck)
        {
            _isTopCardInDrawDeck = isTopCardInDrawDeck;
            Debug.Log($"BaseCardUI.SetIsTopCardInDrawDeck: IsTopCardInDrawDeck set to {_isTopCardInDrawDeck}");
        }
        #endregion
        public Button GetButton()
        {
            GameObject overlay = transform.Find(_buttonLocation).gameObject;
            Debug.Log($"BaseCardUI.GetButton: Button will be found in: {overlay.name}");
            return overlay.GetComponent<Button>();
        }
        #region Private methods
        
        #endregion
    }
}

