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
        private GameViewModel _gameViewModel;
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
            _gameViewModel = new GameViewModel(cardJsonLoader, _gameState);
            
            // Optional: Subscribe to cards initialized event if needed
            _gameViewModel.CardsInitialized += OnCardsInitialized;
        }

        private void Start()
        {
            // Initialize cards
            Debug.Log("App.Start: Starting card initialization");
            //testing card drawing
            
            _gameViewModel.StartGame();
            TestCardPlaying();
        }
        private void TestCardDrawing()
        {
            _gameViewModel.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: true);
            _gameViewModel.DrawFromDrawDeck(Player.Player2, outOfOwnTurnAllowed: true);
            _gameViewModel.EndTurn();
            _gameViewModel.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: false); //should fail because it is player 2's turn
            _gameViewModel.EndTurn();
            _gameViewModel.DrawFromDrawDeck(Player.Player2, outOfOwnTurnAllowed: false); //should fail because it is player 1's turn
            _gameViewModel.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: false); 
            _gameViewModel.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: false); 
            _gameViewModel.DrawFromDrawDeck(Player.Player1, outOfOwnTurnAllowed: false); //should fail because hand is full
        }
        private void TestCardPlaying()
        {
            //at start of game it is player 1's turn, and he has 5 cards in his hand (currently all hero cards)
            HeroCard heroCard1 = (HeroCard)_gameState.GetCardsFromDeck(Deck.Player1Hand)[0];
            HeroCard heroCard2 = (HeroCard)_gameState.GetCardsFromDeck(Deck.Player1Hand)[1];
            HeroCard heroCard3 = (HeroCard)_gameState.GetCardsFromDeck(Deck.Player1Hand)[2];
            HeroCard heroCard4 = (HeroCard)_gameState.GetCardsFromDeck(Deck.Player1Hand)[3];
            HeroCard heroCard5 = (HeroCard)_gameState.GetCardsFromDeck(Deck.Player1Hand)[4];
            //he plays the third and 5th card from his hand to his field
            _gameViewModel.PlayHeroCardFromHand(heroCard3.CardId, Player.Player1);
            _gameViewModel.PlayHeroCardFromHand(heroCard5.CardId, Player.Player1);
        }


        private void OnCardsInitialized(object sender, System.EventArgs e)
        {
            Debug.Log("App.OnCardsInitialized: Cards have been successfully initialized");
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            _gameViewModel.CardsInitialized -= OnCardsInitialized;
        }
    }
}
