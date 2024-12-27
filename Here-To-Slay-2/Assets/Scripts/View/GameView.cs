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
        public GameObject Player2Hand;
        public GameObject Player2Field;

        #endregion
        //test button
        public GameObject TestButton;
        private GameModel _gameModel;


        #region Events
        public event EventHandler<TestButtonClickedEventArgs> TestButtonClicked;
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
                case Deck.AttackAbleMonsters:
                    return AttackableMonsters;
                case Deck.Player1Hand:
                    return Player1Hand;
                case Deck.Player1Field:
                    return Player1Field;
                case Deck.Player2Hand:
                    return Player2Hand;
                case Deck.Player2Field:
                    return Player2Field;
                default:
                    throw new Exception($"GameView: GetDeckObject: Deck enum \"{deck}\" not parseable");
            }
        }
        private void OnCardMoved(object sender, CardMovedEventArgs e)
        {
            GameObject card = GetComponent<GameStateUI>().GetCard(e.CardId);
            card.transform.SetParent(GetDeckObject(e.NewPosition).transform);

            Debug.Log($"GameView: OnCardMoved: Card {e.CardId} moved to {e.NewPosition}");
        }

        private void OnTestButtonClicked(object sender, TestButtonClickedEventArgs e)
        {
            _gameModel.MoveCard(e.CardId, e.Deck);
        }
    }
}
