using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Model;
using Components.CustomEventArgs;
using Components.Enums;
namespace View
{
    public class GameView : MonoBehaviour
    {
        /// <summary>
        /// To make it easier to find the model which is inside the model manager
        /// </summary>
        public GameObject ModelManager;
        #region Deck GameObjects
        public GameObject DrawDeck;
        public GameObject DiscardDeck;
        public GameObject MonsterDeck;
        public GameObject AttackableMonsters;
        public GameObject Player1Hand;
        public GameObject Player1Field;
        public GameObject Player1SlainMonsters;
        public GameObject Player2Hand;
        public GameObject Player2Field;
        public GameObject Player2SlainMonsters;


        #endregion
        //test button
        public GameObject TestButton;
        private GameModel _gameModel;


        #region Events
        //public event EventHandler<TestButtonClickedEventArgs> TestButtonClicked;
        #endregion
        private void Start()
        {
            _gameModel = ModelManager.GetComponent<GameModel>();
            _gameModel.CardMoved += OnCardMoved;
            TestButton.GetComponent<TestClick>().TestButtonClicked += OnTestButtonClicked;

        }

        private GameObject GetDeckObject(Deck deck)
        {
            switch (deck)
            {
                case Deck.DrawDeck:
                    return DrawDeck;
                case Deck.DiscardDeck:
                    return DiscardDeck;
                case Deck.MonsterDeck:
                    return MonsterDeck;
                case Deck.AttackableMonsters:
                    return AttackableMonsters;
                case Deck.Player1Hand:
                    return Player1Hand;
                case Deck.Player1Field:
                    return Player1Field;
                case Deck.Player1SlainMonsters:
                    return Player1SlainMonsters;
                case Deck.Player2Hand:
                    return Player2Hand;
                case Deck.Player2Field:
                    return Player2Field;
                case Deck.Player2SlainMonsters:
                    return Player2SlainMonsters;
                default:
                    throw new Exception($"GameView: GetDeckObject: Deck enum \"{deck}\" not parseable");
            }
        }
        private void OnCardMoved(object sender, CardMovedEventArgs e)
        {
            GameObject card = GetComponent<GameStateUI>().GetCard(e.CardId);
            //1. move the card to the new deck
            //for deck with no limit size
            if (e.NewPosition == -1)
            {
                card.transform.SetParent(GetDeckObject(e.NewDeck).transform);
                Debug.Log($"GameView: OnCardMoved: Card {e.CardId} moved to {e.NewDeck}");
            }
            //for deck with a limit size
            else
            {
                
                GameObject deckObject = GetDeckObject(e.NewDeck);
                GameObject cardPositionObject = deckObject.transform.GetChild(e.NewPosition).gameObject;
                card.transform.SetParent(cardPositionObject.transform);
                Debug.Log($"GameView: OnCardMoved: Card {e.CardId} moved to {e.NewDeck}: {e.NewPosition}");

            }


            //reposition the other cards in the original deck if needed
            if (e.OriginNeedsAdjustment)
            {
                AdjustOriginDeckUI(e);
            }


        }

        private void AdjustOriginDeckUI(CardMovedEventArgs e)
        {
            Debug.Log($"GameView: OnCardMoved: Readjusting origin deck");
            Debug.Log($"GameView: OnCardMoved: CardMovedEventArgs values:");
            Debug.Log($"GameView: OnCardMoved: IdsToAdjust: {string.Join(",", e.IdsToAdjust)}");
            Debug.Log($"GameView: OnCardMoved: AdjustedPositions: {string.Join(",", e.AdjustedPositions)}");
            for (int i = 0; i < e.AdjustedPositions.Count(); i++)
            {
                int id = e.IdsToAdjust[i];
                GameObject cardToReposition = GetComponent<GameStateUI>().GetCard(id);
                GameObject deckObject = GetDeckObject(e.Origin);
                GameObject cardPositionObject = deckObject.transform.GetChild(e.AdjustedPositions[i]).gameObject;
                cardToReposition.transform.SetParent(cardPositionObject.transform);
                Debug.Log($"GameView: OnCardMoved: Card {id} repositioned to {e.Origin}: {e.AdjustedPositions[i]}");
            }
        }

        private void OnTestButtonClicked(object sender, TestButtonClickedEventArgs e)
        {
            _gameModel.MoveCard(e.CardId, e.Deck);
        }
    }
}
