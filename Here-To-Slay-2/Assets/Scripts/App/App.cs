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
using Model;
using ViewModel;

namespace App
{
    /// <summary>
    /// Lightweight coordinator for game initialization and card loading
    /// Implements ICardRepository to manage card addition
    /// </summary>
    public class App : MonoBehaviour
    {
        private GameViewModel _cardViewModel;
        private GameState _gameState;
        
        private void Awake()
        {
            Debug.Log("App.Awake: Initializing GameState");
            // Create a new GameState instance
            _gameState = new GameState();

            Debug.Log("App.Awake: Loading hero JSON and initializing GameViewModel");
            // Load hero JSON from Resources folder
            var heroJsonFile = Resources.Load<TextAsset>("JSON/heroes");
            
            // Initialize dependencies
            var cardJsonLoader = new CardJsonLoader(heroJsonFile);
            _cardViewModel = new GameViewModel(cardJsonLoader, _gameState);
            
            // Optional: Subscribe to cards initialized event if needed
            _cardViewModel.CardsInitialized += OnCardsInitialized;
        }

        private void Start()
        {
            // Initialize cards
            Debug.Log("App.Start: Starting card initialization");
            _cardViewModel.StartGame();
        }

        private void OnCardsInitialized(object sender, System.EventArgs e)
        {
            Debug.Log("App.OnCardsInitialized: Cards have been successfully initialized");
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            _cardViewModel.CardsInitialized -= OnCardsInitialized;
        }
    }
}
