using UnityEngine;
using Util;
namespace Model
{
    public class CreateCardsFromJson : MonoBehaviour
    {
        public GameObject HeroCardPrefab;
        public GameObject cardParent;
        public string HeroesJSONPath;

        private void Start()
        {
            Heroes heroesFromJSON = JsonUtility.FromJson<Heroes>(Resources.Load<TextAsset>(Heroes_JSON_PATH).text);
            Debug.Log(heroesFromJSON.heroes[0].name);       
        }
    }

}