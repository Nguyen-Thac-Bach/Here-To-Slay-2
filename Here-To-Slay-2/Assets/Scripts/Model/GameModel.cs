using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Components.CustomEventArgs;

namespace Model
{
    /// <summary>
    /// Has all the game data, model classes. Responds to click events coming from GameView by relaying the "commands" to the appropriate model classes
    /// </summary>
    /// <remarks>Order of script execution (check Script Execution Order settings): CreateCardsFromJson (once) -> GameModel->GameView </remarks>
    public class GameModel:MonoBehaviour
    {
        private CreateCardsFromJson _createCardsFromJson;
        
        private void Start()
        {
            Debug.Log("GameModel: Start: GameModel started");
            _createCardsFromJson = GetComponent<CreateCardsFromJson>();
            _createCardsFromJson.CardsCreated += OnCardsCreated;

        }

        private void OnCardsCreated(object sender, CardsCreatedEventArgs e)
        {
            Debug.Log("GameModel: OnCardsCreated: Cards created event received, probably from CreateCardsFromJson");
        }
    }
}
