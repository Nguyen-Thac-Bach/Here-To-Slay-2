using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Model;
using Components.Enums;
using TMPro;

namespace View
{
    /// <summary>
    /// Tracks the OBJECTS representing the cards in the game. May also be used to track other game objects.
    /// </summary>
    public class GameStateUI : MonoBehaviour
    {
        public GameObject Phase_Player_ActionText;
        //If debugging is needed, change to public
        private List<GameObject> _cardList;
        private Player _currentPlayerUI;
        private int _remainingActionsUI;
        private GamePhase _currentPhaseUI;
        private int _topCardOfDrawDeckID;

        public List<GameObject> CardList
        {
            get { return _cardList; }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _cardList = new List<GameObject>();
        }

        public void AddCard(GameObject card)
        {
            _cardList.Add(card);
            Debug.Log($"CardList: Added card: {card.GetComponent<HeroCardUI>().HeroName}");
        }

        public GameObject GetCard(int id)
        {
            foreach (GameObject card in _cardList)
            {
                if (card.GetComponent<HeroCardUI>().Id == id)
                {
                    return card;
                }
            }
            throw new System.Exception($"GameStateUI: GetCard: Card with id {id} not found");
        }

        public List<GameObject> GetCardsInDeck(Deck deck)
        {
            List<GameObject> cardsInDeck = new List<GameObject>();
            foreach (GameObject card in _cardList)
            {
                if (card.GetComponent<DeckTagUI>().Deck == deck)
                {
                    cardsInDeck.Add(card);
                }
            }
            return cardsInDeck;
        }
        public void SetCurrentPlayerUI(Player player)
        {
            _currentPlayerUI = player;
            UpdatePhase_Player_ActionText();
        }
        public void SetRemainingActionsUI(int actions)
        {
            _remainingActionsUI = actions;
            UpdatePhase_Player_ActionText();
        }
        public void SetCurrentPhaseUI(GamePhase phase)
        {
            _currentPhaseUI = phase;
            UpdateCardEventListeners();
            Debug.Log($"GameStateUI: SetCurrentPhaseUI: Phase set to {phase}");
            UpdatePhase_Player_ActionText();
        }
        public Button GetButton(int id)
        {
            GameObject card = GetCard(id);
            if (card.GetComponent<HeroCardUI>() != null)
            {
                Debug.Log($"GameStateUI: GetButton: Card with id {id} is a HeroCardUI, getting its button");
                return card.GetComponent<HeroCardUI>().GetButton();
            }
            //if no card type is matching, throw an exception
            throw new System.Exception($"GameStateUI: GetButton: Card with id {id} is not a HeroCardUI");

        }
        private void UpdatePhase_Player_ActionText()
        {
            Phase_Player_ActionText.GetComponent<TextMeshProUGUI>().text = 
                $"Player: {_currentPlayerUI}\n" +
                $"Actions: {_remainingActionsUI}\n" +
                $"Phase: {_currentPhaseUI}";
            Debug.Log($"GameStateUI: UpdatePlayerAndActionText: Updated text to: Player: {_currentPlayerUI}, Actions: {_remainingActionsUI}, Phase: {_currentPhaseUI}");
        }
        private void UpdateCardEventListeners()
        {
            ResetCardEventListeners();
            switch (_currentPhaseUI)
            {
                case GamePhase.ChoosingAction:
                    //these decks are clickable: own hand, field; attackablemonsters, drawdeck
                    //own hand, field
                    Deck deck = _currentPlayerUI == Player.Player1 ? Deck.Player1Hand : Deck.Player2Hand;
                    ListenForActivation(deck);
                    deck = _currentPlayerUI == Player.Player1 ? Deck.Player1Field : Deck.Player2Field;
                    ListenForActivation(deck);
                    //attackablemonsters
                    ListenForAttack();
                    //drawdeck
                    ListenForDraw();
                    break;
                case GamePhase.RollingDice:
                    throw new System.Exception($"GameStateUI: UpdateCardEventListeners: not implemented for phase {_currentPhaseUI}");
                    break;
                case GamePhase.ChoosingTarget:
                    throw new System.Exception($"GameStateUI: UpdateCardEventListeners: not implemented for phase {_currentPhaseUI}");
                    break;
            }
        }
        private void ResetCardEventListeners()
        {
            foreach (GameObject card in _cardList)
            {
                if(card.GetComponent<HeroCardUI>() != null)
                {
                    Button button = card.GetComponent<HeroCardUI>().GetButton();
                    button.onClick.RemoveAllListeners();
                    button.interactable = false;
                }
                else throw new System.Exception($"GameStateUI: ResetCardEventListeners: not implemented for card type {card.GetType()}");
            }
        }
        private void ListenForActivation(Deck deck)
        {
            List<GameObject> cardsInDeck = GetCardsInDeck(deck);
            foreach (GameObject card in cardsInDeck)
            {
                if(card.GetComponent<HeroCardUI>() != null)
                {
                    Button button = card.GetComponent<HeroCardUI>().GetButton();
                    button.onClick.AddListener(() => Debug.Log($"GameStateUI: ListenForActivation: Card {card.GetComponent<HeroCardUI>().HeroName} clicked"));
                    button.interactable = true;
                }
                else throw new System.Exception($"GameStateUI: ListenForActivation: not implemented for card type {card.GetType()}");
            }
        }
        /// <summary>
        /// Only has monster type cards
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        private void ListenForAttack()
        {
            List<GameObject> attackableMonsters = GetCardsInDeck(Deck.AttackableMonsters);
            foreach (GameObject monster in attackableMonsters)
            {
                //implement for monster type cards

            }
            //throw new System.Exception($"GameStateUI: ListenForAttack: not implemented");
        }
        /// <summary>
        /// Puts top card of draw deck in hand
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        private void ListenForDraw()
        {
            List<GameObject> drawDeck = GetCardsInDeck(Deck.DrawDeck);
            foreach (GameObject card in drawDeck)
            {
                if (card.GetComponent<HeroCardUI>() != null)
                {
                    Button button = card.GetComponent<HeroCardUI>().GetButton();
                    button.onClick.AddListener(() => Debug.Log($"GameStateUI: ListenForActivation: Card {card.GetComponent<HeroCardUI>().HeroName} clicked"));
                    button.interactable = true;
                }
                else throw new System.Exception($"GameStateUI: ListenForActivation: not implemented for card type {card.GetType()}");
            }
        }

        

    }
}

