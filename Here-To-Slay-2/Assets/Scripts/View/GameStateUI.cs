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
                if (card.GetComponent<HeroCardUI>().CurrentDeck == deck)
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
            Debug.Log($"GameStateUI: SetCurrentPhaseUI: Phase set to {phase}");
            UpdatePhase_Player_ActionText();
        }
        private void UpdatePhase_Player_ActionText()
        {
            Phase_Player_ActionText.GetComponent<TextMeshProUGUI>().text = 
                $"Player: {_currentPlayerUI}\n" +
                $"Actions: {_remainingActionsUI}\n" +
                $"Phase: {_currentPhaseUI}";
            Debug.Log($"GameStateUI: UpdatePlayerAndActionText: Updated text to: Player: {_currentPlayerUI}, Actions: {_remainingActionsUI}, Phase: {_currentPhaseUI}");
        }

    }
}

