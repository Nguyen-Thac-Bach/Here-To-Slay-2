using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Components.CustomEventArgs;
using Components.Enums;

namespace Model
{
    /// <summary>
    /// Has all the game data, model classes. Responds to click events coming from GameView by relaying the "commands" to the appropriate model classes
    /// </summary>
    /// <remarks>Order of script execution (check Script Execution Order settings): CreateCardsFromJson (once) -> GameModel->GameView </remarks>
    public class GameModel:MonoBehaviour
    {
        #region Events
        public event EventHandler<CardMovedEventArgs> CardMoved;
        #endregion
        private CreateCardsFromJson _createCardsFromJson;
        
        private void Start()
        {
            Debug.Log("GameModel: Start: GameModel started");
            _createCardsFromJson = GetComponent<CreateCardsFromJson>();
            _createCardsFromJson.CardsCreated += OnCardsCreated;

        }
        #region Public Methods
        public void MoveCard(int cardID, Deck destination)
        {
            Deck origin = GameState.Instance.MoveCard(cardID, destination);
            if(origin == Deck.None)
            {
                Debug.Log($"GameModel: MoveCard: Could not move card {cardID} to {destination}");
                return;
            }
            int newPosition = GameState.Instance.GetCard(cardID).CardPosition;
            if(origin in new List<Deck>() { Deck.Player1Hand, Deck.Player2Hand, Deck.Player1Field, Deck.Player2Field, Deck.AttackableMonsters})
                       {
                GameState.Instance.AdjustHandPositions(origin);
            })
            List<int> idsToAdjust = GameState.Instance.GetCardsFromDeck(destination).Select(card => card.CardId).ToList();
            List<int> adjustedPositions = GameState.Instance.GetCardsFromDeck(destination).Select(card => card.CardPosition).ToList();
            CardMoved?.Invoke(this, new CardMovedEventArgs() {Origin = origin , CardId = cardID, NewDeck = destination, NewPosition = newPosition, AdjustedPositions = adjustedPositions});
        }
        #endregion

        #region Event Handlers
        private void OnCardsCreated(object sender, CardsCreatedEventArgs e)
        {
            Debug.Log("GameModel: OnCardsCreated: Cards created event received, probably from CreateCardsFromJson");
        }
        #endregion
    }
}
