using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using System;

using Components.Enums;
namespace View {
    /// <summary>
    /// used to find out which deck the card belongs to. Added to all card prefabs, not just hero cards
    /// </summary>
    public class DeckTagUI : MonoBehaviour
    {

        #region Fields
        /// <summary>
        /// needed for GameStateUI to find the card easily
        /// </summary>
        private Deck _deck;

        #endregion
        public Deck Deck { get => _deck; }



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
            Debug.Log($"DeckTagUI: SetDeck: Deck set to {_deck}");
        }
        #endregion
        #region Private methods
        
        #endregion
    }
}

