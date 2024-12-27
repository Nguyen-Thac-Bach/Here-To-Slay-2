using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace View
{
    /// <summary>
    /// Tracks the OBJECTS representing the cards in the game. May also be used to track other game objects.
    /// </summary>
    public class GameStateUI : MonoBehaviour
    {
        //If debugging is needed, change to public
        private List<GameObject> _cardList;

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


    }
}

