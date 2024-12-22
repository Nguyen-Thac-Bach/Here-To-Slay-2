using UnityEngine;
using System;
using Util;
using Components;
using View;
using System.Collections.Generic;
using Components.Enums;
namespace Model
{
    public class CreateCardsFromJson : MonoBehaviour
    {
        
        /// <summary>
        /// The object that will provide access to all cards
        /// </summary>
        public GameObject CardList;
        public GameObject CardParent;
        public GameObject HeroCardPrefab;
        public TextAsset HeroJSONFile;


        private void Start()
        {
            CreateHeroes();
            Debug.Log("CreateCardsFromJson: Start: Finished creating all cards");
            /*List<BaseCard> cards = CardManagerModel.Instance.GetCards();
            foreach (BaseCard card in cards)
            {
                Debug.Log($"CreateCardsFromJson: Start: Card: {card.Name} is in deck: {card.Deck}");
            }*/
            this.enabled = false;
        }

        private void CreateHeroes()
        {
            Heroes heroesFromJSON = JsonUtility.FromJson<Heroes>(HeroJSONFile.text); //this runs correctly, if in doubt uncomment next line
            //Debug.Log(heroesFromJSON.heroes[0].name);
            foreach (HeroJSON hero in heroesFromJSON.heroes)
            {
                //1. Create the card in the model
                //TODO: When effects are implemented, add them to the card (json, constructors etc.
                HeroCard heroCard = new HeroCard(
                    hero.id, hero.name, hero.description,
                    false, hero.minRoll, new List<Effect>(), 
                    Enum.TryParse<HeroClass>(hero.heroClass, out HeroClass baseHeroClass) ? baseHeroClass : HeroClass.none,
                    Enum.TryParse<HeroClass>(hero.heroClass, out HeroClass currentHeroClass) ? currentHeroClass : HeroClass.none,
                    false);
                CardManagerModel.Instance.AddCard(heroCard);
                Debug.Log($"CreateCardsFromJson: Added hero card: {hero.name} to CardManagerModel");

                //2. Create the card in the view
                GameObject HeroCardObject = Instantiate(HeroCardPrefab, CardParent.transform);
                HeroCardObject.name = hero.name;
                HeroCardObject.GetComponent<HeroCardUI>().SetHeroData(hero.name, hero.heroClass, hero.description, hero.minRoll);
                Debug.Log($"CreateCardsFromJson: Created hero card: {hero.name}");
                CardList.GetComponent<CardManagerView>().AddCard(HeroCardObject);
                Debug.Log("CreateCardsFromJson: Added hero card to CardList");
            }

        }
    }

}