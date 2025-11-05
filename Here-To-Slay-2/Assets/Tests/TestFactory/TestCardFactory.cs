using Components;
using Components.Enums;
using System.Collections.Generic;

namespace Tests.TestFactory
{
    /// <summary>
    /// Factory for creating test cards with sensible defaults.
    /// Allows easy customization for specific test scenarios.
    /// </summary>
    public static class TestCardFactory
    {
        private static int _autoIncrementCardId = 1000;

        /// <summary>
        /// Resets the auto-increment counter for card IDs (call in [SetUp])
        /// </summary>
        public static void ResetIdCounter()
        {
            _autoIncrementCardId = 1000;
        }

        /// <summary>
        /// Creates a BaseCard with default values
        /// </summary>
        public static BaseCard CreateBaseCard(
            int? cardId = null,
            string name = "TestCard",
            string description = "Test card description",
            bool isFaceUp = false,
            Deck deck = Deck.DrawDeck)
        {
            return new BaseCard(
                cardId: cardId ?? GetNextCardId(),
                name: name,
                description: description,
                isFaceUp: isFaceUp,
                deck: deck
            );
        }

        /// <summary>
        /// Creates a HeroCard with default values
        /// </summary>
        public static HeroCard CreateHeroCard(
            int? cardId = null,
            string name = "TestHero",
            string description = "Test hero card",
            bool isFaceUp = false,
            Deck deck = Deck.DrawDeck,
            int minRoll = 5,
            List<Effect> effects = null,
            HeroClass baseHeroClass = HeroClass.fighter,
            HeroClass currentHeroClass = HeroClass.fighter,
            bool isActivatable = false)
        {
            return new HeroCard(
                cardId: cardId ?? GetNextCardId(),
                name: name,
                description: description,
                isFaceUp: isFaceUp,
                deck: deck,
                minRoll: minRoll,
                effects: effects ?? new List<Effect>(),
                baseHeroClass: baseHeroClass,
                currentHeroClass: currentHeroClass,
                isActivatable: isActivatable
            );
        }

        /// <summary>
        /// Creates multiple HeroCards for populating a deck
        /// </summary>
        public static List<HeroCard> CreateMultipleHeroCards(int count, Deck deck = Deck.DrawDeck)
        {
            var cards = new List<HeroCard>();
            for (int i = 0; i < count; i++)
            {
                cards.Add(CreateHeroCard(deck: deck, name: $"Hero{i}"));
            }
            return cards;
        }

        /// <summary>
        /// Creates a complete game deck (draw deck) with the specified number of cards
        /// </summary>
        public static List<HeroCard> CreateDrawDeck(int cardCount = 30)
        {
            return CreateMultipleHeroCards(cardCount, Deck.DrawDeck);
        }

        /// <summary>
        /// Gets the next auto-incremented card ID
        /// </summary>
        private static int GetNextCardId()
        {
            return _autoIncrementCardId++;
        }
    }
}

