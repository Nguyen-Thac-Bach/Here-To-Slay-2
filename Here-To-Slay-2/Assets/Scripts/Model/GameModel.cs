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
        private GameState _gameState;
        
        private void Start()
        {
            _gameState = GameState.Instance;
            Debug.Log("GameModel: Start: GameModel started");
            _createCardsFromJson = GetComponent<CreateCardsFromJson>();
            _createCardsFromJson.CardsCreated += OnCardsCreated;

        }
        #region Public Methods
        public void MoveCard(int cardID, Deck destination)
        {
            Deck origin = _gameState.MoveCard(cardID, destination);
            if(origin == Deck.None)
            {
                Debug.Log($"GameModel: MoveCard: Could not move card {cardID} to {destination}");
                return;
            }
            int newPosition = _gameState.GetCard(cardID).CardPosition;

            if(_gameState.IsDeckWithLimit(origin))
            {
                bool originNeedsAdjustment = true;
                List<int> idsToAdjust = _gameState.GetCardsFromDeck(origin).Select(card => card.CardId).ToList();
                List<int> adjustedPositions = _gameState.GetCardsFromDeck(origin).Select(card => card.CardPosition).ToList();
                Debug.Log($"GameModel: MoveCard: idsToAdjust: {string.Join(",", idsToAdjust)}");
                Debug.Log($"GameModel: MoveCard: adjustedPositions: {string.Join(",", adjustedPositions)}");
                CardMoved?.Invoke(this, new CardMovedEventArgs() { Origin = origin, CardId = cardID, NewDeck = destination, NewPosition = newPosition,OriginNeedsAdjustment = originNeedsAdjustment,IdsToAdjust = idsToAdjust, AdjustedPositions = adjustedPositions });
            }
            else
            {
                bool originNeedsAdjustment = false;
                CardMoved?.Invoke(this, new CardMovedEventArgs() { Origin = origin, CardId = cardID, NewDeck = destination, NewPosition = newPosition,  OriginNeedsAdjustment = originNeedsAdjustment});
            }
            
            
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
