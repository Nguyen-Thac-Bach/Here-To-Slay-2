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
        public GameObject PlayerAndActionText;
        //If debugging is needed, change to public
        private List<GameObject> _cardList;
        private Player _currentPlayerUI;
        private int _remainingActionsUI;

        public List<GameObject> CardList
        {
            get { return _cardList; }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {


            _cardList = new List<GameObject>();
            UpdatePlayerAndActionText();
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

        public void SetCurrentPlayerUI(Player player)
        {
            _currentPlayerUI = player;
            UpdatePlayerAndActionText();
        }
        public void SetRemainingActionsUI(int actions)
        {
            _remainingActionsUI = actions;
            UpdatePlayerAndActionText();
        }
        private void UpdatePlayerAndActionText()
        {
            PlayerAndActionText.GetComponent<TextMeshProUGUI>().text = $"Player: {_currentPlayerUI}, Actions: {_remainingActionsUI}";
            Debug.Log($"GameStateUI: UpdatePlayerAndActionText: Updated player and action text to Player: {_currentPlayerUI}, Actions: {_remainingActionsUI}");
        }

    }
}

