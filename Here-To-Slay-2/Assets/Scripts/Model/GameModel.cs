using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Components.CustomEventArgs;
using Components.Enums;
using Components;
using Model.Services;
using ViewModel;

namespace Model
{
    /// <summary>
    /// Lightweight coordinator for game initialization and card loading
    /// Implements ICardRepository to manage card addition
    /// </summary>
    public class GameModel : MonoBehaviour, ICardRepository
    {
        private CardViewModel _cardViewModel;
        private GameState _gameState;
        
        private void Awake()
        {
            // Create a new GameState instance
            _gameState = new GameState();
            
            // Load hero JSON from Resources folder
            var heroJsonFile = Resources.Load<TextAsset>("JSON/heroes");
            
            // Initialize dependencies
            var cardJsonLoader = new CardJsonLoader(heroJsonFile);
            _cardViewModel = new CardViewModel(cardJsonLoader, this);
            
            // Optional: Subscribe to cards initialized event if needed
            _cardViewModel.CardsInitialized += OnCardsInitialized;
        }

        private void Start()
        {
            // Initialize cards
            _cardViewModel.InitializeCards();
            
            // Start the game
            _gameState.StartGame();
        }

        /// <summary>
        /// Implementation of ICardRepository to add cards to GameState
        /// </summary>
        public void AddCard(object card)
        {
            // Cast to BaseCard and add to GameState
            if (card is Components.BaseCard baseCard)
            {
                _gameState.AddCard(baseCard);
            }
            else
            {
                Debug.LogWarning($"GameModel: Attempted to add non-BaseCard object of type {card.GetType()}");
            }
        }

        private void OnCardsInitialized(object sender, System.EventArgs e)
        {
            Debug.Log("GameModel: Cards have been successfully initialized");
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            _cardViewModel.CardsInitialized -= OnCardsInitialized;
        }

        // Expose GameState methods if needed
        public GameState GetGameState() => _gameState;
    }
}
