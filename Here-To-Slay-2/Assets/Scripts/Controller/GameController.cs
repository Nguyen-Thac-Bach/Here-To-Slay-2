using UnityEngine;
using System;
using Model;
using Components.Enums;
using Components.CustomEventArgs;
using View;

namespace Controller
{
    /// <summary>
    /// Mediates interactions between GameModel and GameStateUI
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameModel _gameModel;
        [SerializeField] private GameStateUI _gameStateUI;

        // Custom event for card clicks
        public event EventHandler<CardClickedEventArgs> CardClicked;

        private void Start()
        {
            // Subscribe to view events
            _gameStateUI.DrawCardRequested += HandleDrawCardRequest;
            
            // Initial game state setup
            UpdateCardInteractivity();
        }

        /// <summary>
        /// Updates card interactivity based on current game state
        /// </summary>
        private void UpdateCardInteractivity()
        {
            // Get current game state from model
            GamePhase currentPhase = _gameModel._gameState.GetCurrentPhase();
            Player currentPlayer = _gameModel._gameState.GetCurrentPlayer();

            // Get selectable card IDs
            var selectableCardIds = _gameModel._gameState.GetSelectableCardIds(currentPhase, currentPlayer);

            // Update UI interactivity
            _gameStateUI.UpdateCardInteractivity(selectableCardIds);
        }

        /// <summary>
        /// Handles draw card requests from the UI
        /// </summary>
        private void HandleDrawCardRequest(object sender, DrawCardEventArgs e)
        {
            try 
            {
                // Execute draw effect for the current player
                _gameModel.ExecuteAtomicCardEffect(AtomicCardEffect.Draw, 
                    _gameModel._gameState.GetCurrentPlayer());
                
                // Update card interactivity after drawing
                UpdateCardInteractivity();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error drawing card: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles card clicks from the UI
        /// </summary>
        public void OnCardClicked(int cardId)
        {
            try 
            {
                // Validate card selection
                GamePhase currentPhase = _gameModel._gameState.GetCurrentPhase();
                Player currentPlayer = _gameModel._gameState.GetCurrentPlayer();

                if (_gameModel._gameState.IsCardSelectable(cardId, currentPhase, currentPlayer))
                {
                    // Raise event for card click
                    CardClicked?.Invoke(this, new CardClickedEventArgs { CardId = cardId });

                    // Update card interactivity
                    UpdateCardInteractivity();
                }
                else
                {
                    Debug.Log($"Card {cardId} is not selectable in current game state");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing card click: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Event arguments for card click events
    /// </summary>
    public class CardClickedEventArgs : EventArgs
    {
        public int CardId { get; set; }
    }
}
