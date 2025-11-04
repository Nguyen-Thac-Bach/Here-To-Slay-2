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
                hero.id, 
                hero.name, 
                hero.description,
                false, 
                Deck.DrawDeck,
                hero.minRoll, 
                new List<Effect>(), 
                ParseHeroClass(hero.heroClass),
                ParseHeroClass(hero.heroClass),
                false
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
