using UnityEngine;
using System;
using Components.CustomEventArgs;

namespace Model
{
    /// <summary>
    /// [DEPRECATED] This class is kept for backwards compatibility and will be removed in future versions.
    /// Card initialization is now handled by CardViewModel in the MVVM architecture.
    /// </summary>
    [Obsolete("Use CardViewModel for card initialization instead.", false)]
    public class CreateCardsFromJson : MonoBehaviour
    {
        [Obsolete("Event is no longer used. Subscribe to CardViewModel.CardsInitialized instead.")]
        public event EventHandler<CardsCreatedEventArgs> CardsCreated;

        private void Start()
        {
            Debug.LogWarning("CreateCardsFromJson is deprecated. Please update your project to use the new MVVM architecture.");
            
            // Invoke the event for backwards compatibility
            CardsCreated?.Invoke(this, new CardsCreatedEventArgs());
            
            // Disable this component
            this.enabled = false;
        }
    }
}