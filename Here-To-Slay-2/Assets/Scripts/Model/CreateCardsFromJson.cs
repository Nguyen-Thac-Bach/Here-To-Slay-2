using UnityEngine;
using Util;
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
            this.enabled = false;
        }

        private void CreateHeroes()
        {
            Heroes heroesFromJSON = JsonUtility.FromJson<Heroes>(HeroJSONFile.text); //this runs correctly, if in doubt uncomment next line
            //Debug.Log(heroesFromJSON.heroes[0].name);
            foreach (HeroJSON hero in heroesFromJSON.heroes)
            {

                GameObject HeroCard = Instantiate(HeroCardPrefab, CardParent.transform);
                HeroCard.name = hero.name;
                HeroCard.GetComponent<HeroCardUI>().SetHeroData(hero.name, hero.heroClass, hero.description, hero.minRoll);
                Debug.Log($"CreateCardsFromJson: Created hero card: {hero.name}");
                CardList.GetComponent<CardList>().AddCard(HeroCard);
                Debug.Log("CreateCardsFromJson: Added hero card to CardList");
            }

        }
    }

}