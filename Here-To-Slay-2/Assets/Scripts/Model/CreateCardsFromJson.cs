using UnityEngine;
using Util;
namespace Model
{
    public class CreateCardsFromJson : MonoBehaviour
    {
        public GameObject cardParent;
        public GameObject HeroCardPrefab;
        public TextAsset HeroJSONFile;


        private void Start()
        {
            CreateHeroes();
        }

        private void CreateHeroes()
        {
            Heroes heroesFromJSON = JsonUtility.FromJson<Heroes>(HeroJSONFile.text); //this runs correctly, if in doubt uncomment next line
            //Debug.Log(heroesFromJSON.heroes[0].name);
            foreach (HeroJSON hero in heroesFromJSON.heroes)
            {

                GameObject HeroCard = Instantiate(HeroCardPrefab, cardParent.transform);
                HeroCard.GetComponent<HeroCardUI>().SetHeroData(hero.name, hero.heroClass, hero.description, hero.minRoll);
                Debug.Log($"CreateCardsFromJson: Created hero card: {hero.name}");
            }

        }
    }

}