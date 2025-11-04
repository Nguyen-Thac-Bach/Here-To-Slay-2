using System;
using UnityEngine;
using Model;
using Model.Services;

namespace ViewModel
{
    /// <summary>
    /// Interface for card management to decouple from specific GameState implementation
    /// </summary>
    public interface ICardRepository
    {
        void AddCard(object card);
    }

    public class CardViewModel
    {
        private readonly ICardJsonLoader _cardJsonLoader;
        private readonly ICardRepository _cardRepository;
        
        public event EventHandler CardsInitialized;

        public CardViewModel(ICardJsonLoader cardJsonLoader, ICardRepository cardRepository)
        {
            _cardJsonLoader = cardJsonLoader ?? throw new ArgumentNullException(nameof(cardJsonLoader));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        public void InitializeCards()
        {
            try 
            {
                var heroCards = _cardJsonLoader.LoadHeroCards();
                foreach (var card in heroCards)
                {
                    _cardRepository.AddCard(card);
                }
                
                Debug.Log($"CardViewModel: Initialized {heroCards.Count} hero cards");
                
                // Trigger event to notify that cards are initialized
                CardsInitialized?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.LogError($"CardViewModel: Error initializing cards - {ex.Message}");
            }
        }
    }
}
