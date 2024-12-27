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
        private const int _maxCardsInHand = 8;
        private const int _maxCardsInField = 5;
        private const int _maxCardsInAttackableMonsters = 3;
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
        public List<BaseCard> GetCardsFromDeck(Deck deck)
        {
            return _cards.FindAll(card => card.Deck == deck);
        }
        public Deck MoveCard(int cardID, Deck destination)
        {
            BaseCard card = _cards.Find(card => card.CardId == cardID);

            if (DeckNotFull(destination))
            {
                Deck origin = card.Deck;
                card.SetDeck(destination);
                RepositionCards(origin);
                Debug.Log(cardID + " moved to " + destination.ToString());
                return origin;
            }
            else {                 
                Debug.Log("Deck is full");
                return Deck.None;
            }
            
            
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
        #region Private methods
        private bool DeckNotFull(Deck deck)
        {
            switch (deck)
            {
                case Deck.Player1Hand:
                    return GetCardsFromDeck(deck).Count < _maxCardsInHand;
                case Deck.Player2Hand:
                    return GetCardsFromDeck(deck).Count < _maxCardsInHand;
                case Deck.Player1Field:
                    return GetCardsFromDeck(deck).Count < _maxCardsInField;
                case Deck.Player2Field:
                    return GetCardsFromDeck(deck).Count < _maxCardsInField;
                case Deck.AttackableMonsters:
                    return GetCardsFromDeck(deck).Count < _maxCardsInAttackableMonsters;
                //other decks don't have a limit
                default:
                    return true;
            }
        }
        /// <summary>
        /// Adjusts the CardPosition value of the cards in the given deck after a card has been moved from it
        /// </summary>
        /// <param name="origin">Which deck needs reindexing</param>
        /// <remarks> only relevant for decks with a limit on the number of cards: hand, field, attackableMonsters</remarks>
        private void RepositionCards(Deck origin)
        {
            List<BaseCard> cards = GetCardsFromDeck(origin);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].SetCardPosition(i);
            }
        }
        #endregion
    }
}

