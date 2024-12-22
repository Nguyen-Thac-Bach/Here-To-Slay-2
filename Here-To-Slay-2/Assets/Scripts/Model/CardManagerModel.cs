using UnityEngine;
using System.Collections.Generic;
using Components;
using Components.Enums;

namespace Model
{
    /// <summary>
    /// Singleton class that provides methods to access and move cards between decks
    /// </summary>
    public class CardManagerModel
    {
        #region Fields
        private static CardManagerModel _instance;
        /// <summary>
        /// Stores all the cards in the game
        /// </summary>
        /// <remarks>
        /// If performance is a concern, consider using a different data structure, such as a dictionary
        /// </remarks>
        private List<BaseCard> _cards;
        #endregion

        #region Properties
        public static CardManagerModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CardManagerModel();
                }
                return _instance;
            }
        }
        #endregion
        #region Constructors
        public CardManagerModel()
        {

        }
        #endregion
        #region Public methods
        public void AddCard(BaseCard card)
        {
            _cards.Add(card);
        }

        public BaseCard GetCard(int cardID)
        {
            return _cards.Find(card => card.CardId == cardID);
        }
        public void MoveCard(int cardID, Deck destination)
        {
            _cards.Find(card => card.CardId == cardID).SetDeck(destination);
            Debug.Log(cardID + " moved to " + destination.ToString());
        }
        #endregion

    }
}

