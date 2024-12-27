using UnityEngine;
using System.Collections.Generic;
using Components;
using Components.Enums;

namespace Model
{
    /// <summary>
    /// Singleton class that provides methods to access and move cards between decks
    /// </summary>
    public sealed class GameState
    {
        #region Fields
        private static readonly GameState _instance = new GameState();
        /// <summary>
        /// Stores all the cards in the game
        /// </summary>
        /// <remarks>
        /// If performance is a concern, consider using a different data structure, such as a dictionary
        /// </remarks>
        private List<BaseCard> _cards;
        private const int _maxActions = 3;
        private Player _currentPlayer;
        private int _remainingActions;
        #endregion

        #region Properties
        public static GameState Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion
        #region Constructors
  
        private GameState()
        {
            _cards = new List<BaseCard>();
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

        public List<BaseCard> GetCards()
        {
            return _cards;
        }
        public void MoveCard(int cardID, Deck destination)
        {
            _cards.Find(card => card.CardId == cardID).SetDeck(destination);
            Debug.Log(cardID + " moved to " + destination.ToString());
        }

        public Player GetCurrentPlayer()
        {
            Debug.Log("Current player: " + _currentPlayer.ToString());
            return _currentPlayer;
            
        }

        public void SetToNextPlayer()
        {
            _currentPlayer = _currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            Debug.Log("Current player: " + _currentPlayer.ToString());
        }

        public int GetRemainingActions()
        {
            return _remainingActions;
        }
        /// <summary>
        /// Typically called at the start of a new turn
        /// </summary>
        public void RefreshActions()
        {
            _remainingActions = _maxActions;
        }

        public void UseAction(int actionCost)
        {
            if(_remainingActions - actionCost >= 0)
            {
                _remainingActions -= actionCost;
            }
            else
            {
                Debug.Log("Not enough actions");
                throw new System.Exception("Not enough actions");
                
            }
        }
        #endregion

    }
}

