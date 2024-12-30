using UnityEngine;
using System.Collections.Generic;
using Components;
using Components.Enums;
using Components.CustomEventArgs;
using System;
using System.Linq;

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
        private const int _maxCardsInSlainMonsters = 3;
        private const int _maxActions = 3;
        private Player _currentPlayer;
        private int _remainingActions;
        #endregion
        #region Events
        public event EventHandler<CardMovedEventArgs> CardMoved;
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
        public void NewGame()
        {
            _cards.Clear();
            _currentPlayer = Player.Player1;
            RefreshActions();
        }
        public void EndTurn()
        {
            SetToNextPlayer();
            RefreshActions();
        }
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
        public BaseCard GetTopCardFromDrawDeck()
        {
            return GetCardsFromDeck(Deck.DrawDeck)[0];
        }
        /// <summary>
        /// Moves card in the persistent data structure
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="destination"></param>
        /// <returns>None if unsuccessful, otherwise the original deck the card belonged to</returns>
        public void MoveCard(int cardID, Deck destination)
        {
            BaseCard card = _cards.Find(card => card.CardId == cardID);

            if (DeckNotFull(destination))
            {
                //1. Move card to destination
                Deck origin = card.Deck;
                card.SetDeck(destination);
                int oldCardPosition = card.CardPosition;
                SetCardPosition(card, destination);
                int newPosition = card.CardPosition;
                bool originNeedsAdjustment = IsDeckWithLimit(origin);
                //2. Reposition cards in the origin deck if needed
                if (originNeedsAdjustment)
                {
                    RepositionCards(origin, oldCardPosition);
                    List<int> idsToAdjust = GetCardsFromDeck(origin).Select(cd => cd.CardId).ToList();
                    List<int> adjustedPositions = GetCardsFromDeck(origin).Select(cd => cd.CardPosition).ToList();
                    Debug.Log($"GameState: MoveCard: idsToAdjust: {string.Join(",", idsToAdjust)}");
                    Debug.Log($"GameState: MoveCard: adjustedPositions: {string.Join(",", adjustedPositions)}");
                    CardMoved?.Invoke(this, new CardMovedEventArgs() { Origin = origin, CardId = cardID, NewDeck = destination, NewPosition = newPosition, OriginNeedsAdjustment = originNeedsAdjustment, IdsToAdjust = idsToAdjust, AdjustedPositions = adjustedPositions });
                }
                else
                {
                    CardMoved?.Invoke(this, new CardMovedEventArgs() { Origin = origin, CardId = cardID, NewDeck = destination, NewPosition = newPosition, OriginNeedsAdjustment = originNeedsAdjustment });
                }
                Debug.Log(cardID + " moved to " + destination.ToString());
            }
            else {                 
                Debug.Log("Deck is full");
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

        public bool IsDeckWithLimit(Deck deck) {             
            switch (deck)
            {
                case Deck.Player1Hand:
                case Deck.Player2Hand:
                case Deck.Player1Field:
                case Deck.Player2Field:
                case Deck.Player1SlainMonsters:
                case Deck.Player2SlainMonsters:
                case Deck.AttackableMonsters:
                    return true;
                default:
                    return false;
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
                case Deck.Player1SlainMonsters:
                    return GetCardsFromDeck(deck).Count < _maxCardsInSlainMonsters;
                case Deck.Player2SlainMonsters:
                    return GetCardsFromDeck(deck).Count < _maxCardsInSlainMonsters;

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
        private void RepositionCards(Deck origin, int fromWhichPosition)
        {
            Debug.Log($"GameState: RepositionCards: Repositioning cards in {origin} from position {fromWhichPosition}");
            List<BaseCard> cards = GetCardsFromDeck(origin);
            int cardsToReposition = cards.Count - fromWhichPosition;
            Debug.Log($"GameState: RepositionCards: Cards to reposition: {cardsToReposition}");
            for (int i = 0; i < cardsToReposition; i++)
            {
                cards[i + fromWhichPosition].SetCardPosition(i + fromWhichPosition);
                Debug.Log($"GameState: RepositionCards: Card {cards[i + fromWhichPosition].CardId} 's position value set to {i + fromWhichPosition}");
            }
        }

        private void SetCardPosition(BaseCard card, Deck destination)
        {
            switch (destination)
            {
                case Deck.Player1Hand:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count-1);
                    break;
                case Deck.Player2Hand:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                case Deck.Player1Field:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                case Deck.Player2Field:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                case Deck.AttackableMonsters:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                case Deck.Player1SlainMonsters:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                case Deck.Player2SlainMonsters:
                    card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                    break;
                default:
                    card.SetCardPosition(-1);
                    break;
            }
        }
        #endregion
    }
}

