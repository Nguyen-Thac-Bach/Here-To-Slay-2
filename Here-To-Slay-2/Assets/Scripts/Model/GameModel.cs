using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Components.CustomEventArgs;
using Components.Enums;
using Components;

namespace Model
{
    /// <summary>
    /// Has all the game data, model classes. Responds to click events coming from GameView by relaying the "commands" to the appropriate model classes
    /// </summary>
    /// <remarks>Order of script execution (check Script Execution Order settings): CreateCardsFromJson (once) -> GameModel->GameView </remarks>
    public class GameModel:MonoBehaviour
    {
        
        private CreateCardsFromJson _createCardsFromJson;
        public GameState _gameState;
        
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
            _gameState.MoveCard(cardID, destination);
        }
        /// <summary>
        /// Executes 1 atomic card effect.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="effectSourceCardID">which card the effect originates from. Needed because the owner of the card is often also affected by the AtomicCardEffect</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ExecuteAtomicCardEffect(AtomicCardEffect effect, int effectSourceCardID)
        {
            Player player = GetPlayer(effectSourceCardID);
            switch (effect)
            {
                
                //actorRelevant
                case AtomicCardEffect.Draw:
                    
                    ExecuteDrawEffect(player);
                    break;
                //needsChoosingCard
                case AtomicCardEffect.Recall:
                    ExecuteRecallEffect();
                    break;
                //needsChoosingCard, actorRelevant
                case AtomicCardEffect.Slay:
                    ExecuteSlayEffect();
                    break;
                //needsChoosingCard, needsChoosingDestination, actorRelevant
                default:
                    throw new NotImplementedException($"GameModel: ExecuteAtomicCardEffect: Effect {effect} not implemented");

            }
        }

        #endregion
        #region Private Methods
        private void ExecuteDrawEffect(Player player)
        {
            int drawCardID = _gameState.GetTopCardFromDrawDeck().CardId;
            Debug.Log($"GameModel: ExecuteDrawEffect: Player {player} drew card {drawCardID}");
            MoveCard(drawCardID, player == Player.Player1 ? Deck.Player1Hand : Deck.Player2Hand);
        }
        private void ExecuteRecallEffect()
        {
            //TODO: implement when items are implemented
        }
        private void ExecuteDiscardEffect(Player player)
        {
            BaseCard cardToDiscard = ChooseCardFromDeck(player == Player.Player1 ? Deck.Player1Hand : Deck.Player2Hand);
            MoveCard(cardToDiscard.CardId, Deck.DiscardDeck);
        }
        private void ExecuteSlayEffect()
        {
            //execute slay effect
            //replenish attackable monsters
        }
        /// <summary>
        /// Prompts the player to choose a card from a given deck. Will probably need to be async
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        private BaseCard ChooseCardFromDeck(Deck deck)
        {
            //if user prompted the card effect, send an event to the view to prompt the user to choose a card
            //also send which deck the card should be chosen from
            //return the card chosen by the user

            //if AI prompted the card effect, tell AI to choose a card
            //return the card chosen by the AI
            throw new NotImplementedException();
        }

        private Player GetPlayer(int cardID)
        {
            Deck deck = _gameState.GetCard(cardID).Deck;
            switch (deck)
            {
                case Deck.Player1Hand:
                case Deck.Player1Field:
                case Deck.Player1SlainMonsters:
                    return Player.Player1;
                case Deck.Player2Hand:
                case Deck.Player2Field:
                case Deck.Player2SlainMonsters:
                    return Player.Player2;
                default:
                    return Player.None;
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
