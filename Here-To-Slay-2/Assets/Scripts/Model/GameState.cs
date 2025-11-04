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
    /// Manages the state of the game, including cards, players, and game progression
    /// </summary>
    public class GameState
    {
        #region Fields
        private List<BaseCard> _cards;
        private const int _maxCardsInHand = 8;
        private const int _maxCardsInField = 5;
        private const int _maxCardsInAttackableMonsters = 3;
        private const int _maxCardsInSlainMonsters = 3;
        private const int _maxActions = 3;
        private const int _startingHandSize = 5;
        private Player _currentPlayer;
        private int _remainingActions;
        private GamePhase _currentPhase;
        #endregion
        #region Events
        public event EventHandler<CardMovedEventArgs> CardMoved;
        public event EventHandler<PlayerChangedEventArgs> PlayerChanged;
        public event EventHandler<ActionUsedEventArgs> ActionUsed;
        public event EventHandler<PhaseChangedEventArgs> PhaseChanged;
        public event EventHandler<TopCardInDrawDeckChangedEventArgs> TopCardInDrawDeckChanged;
        #endregion
        #region Constructors
  
        public GameState()
        {
            _cards = new List<BaseCard>();
        }
        #endregion
        #region Public methods
        public void StartGame()
        {
            RefreshActions();
            SetPlayer(Player.Player1);
            DrawStartingCards();
            SetPlayerPhase(GamePhase.ChoosingAction);
            Debug.Log("GameState.StartGame: New game started");
        }
        public void EndTurn()
        {
            SetToNextPlayer();
            RefreshActions();
        }
        /// <summary>
        /// Adds a card to the persistent data structure
        /// </summary>
        /// <param name="card"></param>
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
                //2. If the card was from the draw deck, notify the subscribers
                if (origin == Deck.DrawDeck)
                {
                    TopCardInDrawDeckChanged?.Invoke(this, new TopCardInDrawDeckChangedEventArgs() { NewTopCardInDrawDeckId = GetTopCardFromDrawDeck().CardId });
                }
                //3. Reposition cards in the origin deck if needed
                if (originNeedsAdjustment)
                {
                    RepositionCards(origin, oldCardPosition);
                    List<int> idsToAdjust = GetCardsFromDeck(origin).Select(cd => cd.CardId).ToList();
                    List<int> adjustedPositions = GetCardsFromDeck(origin).Select(cd => cd.CardPosition).ToList();
                    Debug.Log($"GameState.MoveCard: idsToAdjust: {string.Join(",", idsToAdjust)}");
                    Debug.Log($"GameState.MoveCard: adjustedPositions: {string.Join(",", adjustedPositions)}");
                    CardMoved?.Invoke(this, new CardMovedEventArgs() { 
                        Origin = origin,
                        CardId = cardID,
                        NewDeck = destination,
                        NewPosition = newPosition,
                        OriginNeedsAdjustment = originNeedsAdjustment,
                        IdsToAdjust = idsToAdjust,
                        AdjustedPositions = adjustedPositions });
                }
                else
                {
                    CardMoved?.Invoke(this, new CardMovedEventArgs() { Origin = origin, CardId = cardID, NewDeck = destination, NewPosition = newPosition, OriginNeedsAdjustment = originNeedsAdjustment });
                }
                Debug.Log($"GameState.MoveCard: {cardID} moved to {destination.ToString()}");
            }
            else {                 
                Debug.Log($"GameState.MoveCard: Deck {destination} is full");
            }
        }

        public Player GetCurrentPlayer()
        {
            Debug.Log($"GameState.GetCurrentPlayer: Current player: {_currentPlayer.ToString()}");
            return _currentPlayer;
            
        }
        /// <summary>
        /// Sets the current player to the next one and notifies subscribers (switches between Player1 and Player2)
        /// </summary>
        public void SetToNextPlayer()
        {
            if (_currentPlayer == Player.Player1)
            {
                SetPlayer(Player.Player2);
            }
            else
            {
                SetPlayer(Player.Player1);
            }
            Debug.Log($"GameState.SetToNextPlayer: Current player: {_currentPlayer.ToString()}");
            _currentPhase = GamePhase.ChoosingAction;
            PhaseChanged?.Invoke(this, new PhaseChangedEventArgs() { GamePhase = _currentPhase });
        }

        public int GetRemainingActions()
        {
            return _remainingActions;
        }
        /// <summary>
        /// Typically called at the start of a new turn
        /// </summary>
        private void RefreshActions()
        {
            _remainingActions = _maxActions;
            ActionUsed?.Invoke(this, new ActionUsedEventArgs() { RemainingActions = _remainingActions });
        }

        

        public void UseAction(int actionCost)
        {
            if(_remainingActions - actionCost >= 0)
            {
                _remainingActions -= actionCost;
            }
            else
            {
                Debug.Log("GameState.UseAction: Not enough actions");
                throw new System.Exception("Not enough actions");
                
            }
        }

        /// <summary>
        /// Returns true if the deck has a limit on the number of cards it can contain
        /// </summary>
        /// <param name="deck">deck</param>
        /// <returns>True if yes, no otherwise</returns>
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
        public GamePhase GetCurrentPhase()
        {
            return _currentPhase;
        }
        #endregion
        #region Private methods
        private void DrawStartingCards()
        {
            for (int i = 0; i < _startingHandSize; i++)
            {
                MoveCard(GetTopCardFromDrawDeck().CardId, Deck.Player1Hand);
                MoveCard(GetTopCardFromDrawDeck().CardId, Deck.Player2Hand);
            }
            Debug.Log("GameState.DrawStartingCards: Starting cards drawn");

        }
        /// <summary>
        /// Sets the current player and notifies subscribers
        /// </summary>
        /// <param name="player"></param>
        private void SetPlayer(Player player)
        {
            _currentPlayer = player;
            PlayerChanged?.Invoke(this, new PlayerChangedEventArgs() { Player = _currentPlayer });
        }
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
            Debug.Log($"GameState.RepositionCards: Repositioning cards in {origin} from position {fromWhichPosition}");
            List<BaseCard> cards = GetCardsFromDeck(origin);
            int cardsToReposition = cards.Count - fromWhichPosition;
            Debug.Log($"GameState.RepositionCards: Cards to reposition: {cardsToReposition}");
            for (int i = 0; i < cardsToReposition; i++)
            {
                cards[i + fromWhichPosition].SetCardPosition(i + fromWhichPosition);
                Debug.Log($"GameState.RepositionCards: Card {cards[i + fromWhichPosition].CardId} 's position value set to {i + fromWhichPosition}");
            }
        }
        /// <summary>
        /// Sets the CardPosition value of the card as the last in the destination deck. Sets to -1 if the deck has no limit
        /// </summary>
        /// <param name="card"></param>
        /// <param name="destination"></param>
        private void SetCardPosition(BaseCard card, Deck destination)
        {
            if (IsDeckWithLimit(destination))
            {
                card.SetCardPosition(GetCardsFromDeck(destination).Count - 1);
                return;
            }
            card.SetCardPosition(-1);
        }

        /// <summary>
        /// Determines if a card is selectable based on current game state
        /// </summary>
        public bool IsCardSelectable(int cardId, GamePhase currentPhase, Player currentPlayer)
        {
            BaseCard card = GetCard(cardId);
            
            switch (currentPhase)
            {
                case GamePhase.ChoosingAction:
                    return IsCardSelectableInChoosingActionPhase(card, currentPlayer);
                case GamePhase.RollingDice:
                case GamePhase.ChoosingTarget:
                    // Placeholder for future phase-specific logic
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines card selectability during the Choosing Action phase
        /// </summary>
        private bool IsCardSelectableInChoosingActionPhase(BaseCard card, Player currentPlayer)
        {
            Deck playerHand = currentPlayer == Player.Player1 ? Deck.Player1Hand : Deck.Player2Hand;
            Deck playerField = currentPlayer == Player.Player1 ? Deck.Player1Field : Deck.Player2Field;

            return card.Deck == playerHand ||
                   card.Deck == playerField ||
                   card.Deck == Deck.AttackableMonsters ||
                   card.Deck == Deck.DrawDeck;
        }

        private void SetPlayerPhase(GamePhase phase)
        {
            _currentPhase = phase;
            PhaseChanged?.Invoke(this, new PhaseChangedEventArgs() { GamePhase = _currentPhase });
        }

        /// <summary>
        /// Gets all selectable card IDs for the current game state
        /// </summary>
        public List<int> GetSelectableCardIds(GamePhase currentPhase, Player currentPlayer)
        {
            return _cards
                .Where(card => IsCardSelectable(card.CardId, currentPhase, currentPlayer))
                .Select(card => card.CardId)
                .ToList();
        }
        #endregion
    }
}

