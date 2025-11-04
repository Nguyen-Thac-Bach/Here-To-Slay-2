using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Components;
using Components.Enums;
using Util;

namespace Model.Services
{
    public interface ICardJsonLoader
    {
        List<HeroCard> LoadHeroCards();
    }
    /// <summary>
    /// Loads hero cards from a JSON file
    /// </summary>
    public class CardJsonLoader : ICardJsonLoader
    {
        private readonly TextAsset _heroJsonFile;

        public CardJsonLoader(TextAsset heroJsonFile)
        {
            _heroJsonFile = heroJsonFile;
        }

        public List<HeroCard> LoadHeroCards()
        {
            var heroesContainer = JsonUtility.FromJson<HeroesContainer>(_heroJsonFile.text);
            return heroesContainer.heroes.Select(hero => new HeroCard(
                cardId: hero.id, 
                name: hero.name, 
                description: hero.description,
                isFaceUp: false, 
                deck: Deck.DrawDeck,
                minRoll: hero.minRoll, 
                effects: new List<Effect>(), 
                baseHeroClass: ParseHeroClass(hero.heroClass),
                currentHeroClass: ParseHeroClass(hero.heroClass),
                isActivatable: false
            )).ToList();
        }

        private HeroClass ParseHeroClass(string heroClassString)
        {
            return Enum.TryParse<HeroClass>(heroClassString, out HeroClass heroClass) 
                ? heroClass 
                : HeroClass.none;
        }
    }

    [System.Serializable]
    public class HeroesContainer
    {
        public List<HeroJSON> heroes;
    }
}
