using UnityEngine;
using Unity.Entities;

using System;
using Components.Enums;
using Unity.Collections;
using System.Collections.Generic;

using Util;
using Components;
//commented out for now

namespace Systems.Initialization
{
    //SystemBase is needed, because UI can only reference systems that inherit from SystemBase
    //[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class CardCreatorSystem : SystemBase
    {
        #region Constants
        #endregion

        #region Fields
        public Action<List<string>> OnHeroCardCreated;
        #endregion

        protected override void OnCreate()
        {
            
            Debug.Log("HeroCardCreatorSystem.OnCreate");
            //wait for components to exist
            RequireForUpdate<HeroCardCreatorProperties>();
        }

        protected override void OnDestroy()
        {
        }
        protected override void OnUpdate()
        {
            //we only want to run this system once, so we disable it for future updates
            Enabled = false;
            //TODO: check if this way of making sure OnUpdate runs "only once" hurts performance
            Debug.Log("CardCreatorSystem.OnUpdate");

            //Hero cards
            CreateHeroesFromJSON();

            //check if cards are created
            int heroCardCount = 0;
            Debug.Log("CardCreatorSystem.OnUpdate: heroCardCount: " + heroCardCount);
                

            

        }

        #region Private Methods
        /// <summary>
        /// invokes OnHeroCardCreated event for the UI to display the hero card
        /// </summary>
        /// <param name="heroCardData"></param>
        private void HeroCardCreated(List<string> heroCardData)
        {
            OnHeroCardCreated?.Invoke(heroCardData);
        }
        private void CreateHeroesFromJSON()
        {
            //read json file
            Entity heroCardCreatorEntity = SystemAPI.GetSingletonEntity<HeroCardCreatorProperties>();
            CardCreatorAspect cardCreatorAspect = SystemAPI.GetAspect<CardCreatorAspect>(heroCardCreatorEntity);
            string Heroes_JSON_PATH = cardCreatorAspect.Heroes_JSON_PATH;
            Heroes heroesFromJSON = JsonUtility.FromJson<Heroes>(Resources.Load<TextAsset>(Heroes_JSON_PATH).text);
            Debug.Log("CardCreatorSystem.CreateHeroesFromJSON: loaded data from " + Heroes_JSON_PATH);
            //create hero card entities using data from HeroCardCreator and JSON

            //for now, assume there is only one hero card creator
            Debug.Log("CardCreatorSystem.CreateHeroCards");
            //since we make structural changes while iterating through entities, we need to use EntityCommandBuffer for "thread safety"

            //start by creating only 1 hero card
            //old version
            /*
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            foreach (HeroJSON heroData in heroesFromJSON.heroes)
            {
                CreateHeroCardFrom(ecb, heroData, cardCreatorAspect);
            }

            //actually create entities
            ecb.Playback(EntityManager);
            ecb.Dispose();
            */
            //new version
            CreateHeroCardFrom(heroesFromJSON.heroes[0], cardCreatorAspect);
        }
        private void CreateHeroCardFrom(HeroJSON heroData, CardCreatorAspect cardCreatorAspect)
        {
            Debug.Log("CardCreatorSystem.CreateHeroCardFrom: starting method using " + heroData.name);
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            Entity newHeroCardEntity = ecb.Instantiate(cardCreatorAspect.HeroCardPrefab);
            int mR = heroData.minRoll;
            HeroClass hC = Enum.TryParse(typeof(HeroClass), heroData.heroClass, out object heroClass) ? (HeroClass)heroClass : HeroClass.none;
            
            //TODO: how to access newHeroCardEntity's components?
            //the prefab will need to contain a script first that sets display texts. I need to access the script to set its values for card display
            /*
            ecb.AddComponent<HeroCard>(newHeroCardEntity);
            ecb.SetComponent<HeroCard>(newHeroCardEntity, new HeroCard
            {

                MinRoll = mR,
                HeroClass = hC,
                IsActivated = false,

            });
            Debug.Log($"CardCreatorSystem.CreateHeroCards: MinRoll: {mR}, HeroClass: {hC}");
            //TODO: check if every string data is transferred
            //if not, then increase byte size of strings in BaseCard
            int id = heroData.id;
            string name = heroData.name;
            string description = heroData.description;
            ecb.AddComponent<BaseCard>(newHeroCardEntity);
            ecb.SetComponent<BaseCard>(newHeroCardEntity, new BaseCard
            {
                CardId = id,
                Name = name,
                Description = description,
                IsViewable = true,
            });
            Debug.Log($"CardCreatorSystem.CreateHeroCards: CardId: {id}, Name: {name}, Description: {description}");
            */
            

            //Actually create hero card entity
            ecb.Playback(EntityManager);
            ecb.Dispose();
            //listen for UI event to display hero card
            //Entities.ForEach()
            MonoBehaviour UIScript = GameObject.Find("HeroCardPrefab").GetComponent<MonoBehaviour>();
            Debug.Log("CardCreatorSystem.CreateHeroCard: UIScript to subscribe to: " + UIScript);
            //invoke event for UI to display hero card
            HeroCardCreated(new List<string> { heroData.name, heroData.heroClass, heroData.description, heroData.minRoll.ToString() });
        }
        #endregion
    }
}

