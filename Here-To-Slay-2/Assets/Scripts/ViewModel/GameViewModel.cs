using System;
using UnityEngine;
using Model;
using Model.Services;

namespace ViewModel
{


    public class GameViewModel
    {
        private readonly ICardJsonLoader _cardJsonLoader;
        private readonly GameState _gameState;
        
        public event EventHandler CardsInitialized;

        public GameViewModel(ICardJsonLoader cardJsonLoader, GameState cardRepository)
        {
            _cardJsonLoader = cardJsonLoader ?? throw new ArgumentNullException(nameof(cardJsonLoader));
            _gameState = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }
        /// <summary>
        /// Starts the game by initializing hero cards and updating game state
        /// </summary>
        public void StartGame()
        {
            Debug.Log("CardViewModel.StartGame: Starting game initialization");
            InitializeHeroCards();
            _gameState.StartGame();
        }

        private void InitializeHeroCards()
        {
            try 
            {
                var heroCards = _cardJsonLoader.LoadHeroCards();
                foreach (var card in heroCards)
                {
                    _gameState.AddCard(card);
                }
                
                Debug.Log($"CardViewModel: Initialized {heroCards.Count} hero cards");
                
                // Trigger event to notify that cards are initialized
                CardsInitialized?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.LogError($"CardViewModel: Error initializing cards - {ex.Message}");
            }
        }
    }
}
