using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Model;
using Components.Enums;
using Components.CustomEventArgs;
using TMPro;
using System;

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

        public event EventHandler<DrawCardEventArgs> DrawCardRequested;

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
            Debug.Log($"GameStateUI.AddCard: CardList: Added card: {card.GetComponent<BaseCardUI>().Id}");
        }
        /// <summary>
        /// gets the card object with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">if card with given id not found</exception>
        public GameObject GetCard(int id)
        {
            foreach (GameObject card in _cardList)
            {
                if (card.GetComponent<BaseCardUI>().Id == id)
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
                if (card.GetComponent<BaseCardUI>().Deck == deck)
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
            Debug.Log($"GameStateUI.SetCurrentPhaseUI: Phase set to {phase}");
            UpdatePhase_Player_ActionText();
        }
        public void SetTopCardInDrawDeckID(int id)
        {
            _topCardOfDrawDeckID = id;
            UpdateTopCardInDrawDeckCardEventListener(id);
            Debug.Log($"GameStateUI.SetTopCardInDrawDeckID: Top card in draw deck set to {id}");
        }
        public void UpdateDrawnCardEventListeners(int id)
        {
            GameObject card = GetCard(id);
            Button button = card.GetComponent<BaseCardUI>().GetButton();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => Debug.Log($"GameStateUI.UpdateDrawnCardEventListener: Card {id} clicked"));
            bool drawnDuringOwnTurn;
            if (_currentPlayerUI == Player.Player1)
            {
                drawnDuringOwnTurn = card.GetComponent<BaseCardUI>().Deck == Deck.Player1Hand;
            }
            else
            {
                drawnDuringOwnTurn = card.GetComponent<BaseCardUI>().Deck == Deck.Player2Hand;
            }
            bool isChoosingActionPhase = _currentPhaseUI == GamePhase.ChoosingAction;
            button.interactable = drawnDuringOwnTurn && isChoosingActionPhase;
            Debug.Log($"GameStateUI.UpdateDrawnCardEventListeners: Updated event listeners for card {id}. Interactable = drawnDuringOwnTurn({drawnDuringOwnTurn}) && isChooingActionPhase({isChoosingActionPhase})");
        }
        public Button GetButton(int id)
        {
            GameObject card = GetCard(id);
            return card.GetComponent<BaseCardUI>().GetButton();
        }
        public bool CanDrawCard()
        {
            return _currentPhaseUI == GamePhase.ChoosingAction;
        }
        /// <summary>
        /// Updates card interactivity based on a list of selectable card IDs
        /// </summary>
        public void UpdateCardInteractivity(List<int> selectableCardIds)
        {
            // Reset all card listeners and interactivity
            ResetCardEventListeners();

            // Update interactivity for selectable cards
            foreach (GameObject card in _cardList)
            {
                int cardId = card.GetComponent<BaseCardUI>().Id;
                Button button = card.GetComponent<BaseCardUI>().GetButton();
                
                // Set interactivity based on selectability
                button.interactable = selectableCardIds.Contains(cardId);
                
                // Add basic click handling
                if (button.interactable)
                {
                    button.onClick.AddListener(() => {
                        Debug.Log($"GameStateUI: Card {cardId} clicked");
                        // The actual click handling will be done by GameController
                    });
                }
            }
        }
        #region Private methods
        private void UpdatePhase_Player_ActionText()
        {
            Phase_Player_ActionText.GetComponent<TextMeshProUGUI>().text = 
                $"Player: {_currentPlayerUI}\n" +
                $"Actions: {_remainingActionsUI}\n" +
                $"Phase: {_currentPhaseUI}";
            Debug.Log($"GameStateUI.UpdatePlayerAndActionText: Updated text to: Player: {_currentPlayerUI}, Actions: {_remainingActionsUI}, Phase: {_currentPhaseUI}");
        }

        private void UpdateTopCardInDrawDeckCardEventListener(int id)
        {
            GameObject card = GetCard(id);
            Button button = card.GetComponent<BaseCardUI>().GetButton();

            bool isChoosingActionPhase = _currentPhaseUI == GamePhase.ChoosingAction;
            button.interactable = isChoosingActionPhase;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (CanDrawCard())
                {
                    // Raise an event to request drawing a card
                    DrawCardRequested?.Invoke(this, new DrawCardEventArgs { activePlayerDraws = true });
                }
                Debug.Log($"GameStateUI.UpdateTopCardInDrawDeckCardEventListener: Card {id} clicked");
            });
            Debug.Log($"GameStateUI.UpdateTopCardInDrawDeckCardEventListener: Updated event listeners for card {id}. Interactable = isChooingActionPhase({isChoosingActionPhase})");
        }

        private void ResetCardEventListeners()
        {
            foreach (GameObject card in _cardList)
            {
                Button button = card.GetComponent<BaseCardUI>().GetButton();
                button.onClick.RemoveAllListeners();
                button.interactable = false;
            }
            Debug.Log($"GameStateUI.ResetCardEventListeners: Removed all listeners and set interactable to false for all cards");
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
        /// Makes the top card of the draw deck clickable
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        private void ListenForDraw()
        {
            // Simplified draw logic
            if (_topCardOfDrawDeckID != -1)
            {
                Debug.Log($"GameStateUI: Top card in draw deck is {_topCardOfDrawDeckID}");
            }
        }

        #endregion

    }
}

