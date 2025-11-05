using NUnit.Framework;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;

using System;
using UnityEngine.UI;


namespace View {
    public class HeroCardUI : MonoBehaviour
    {

        #region Fields
        /// <summary>
        /// needed for GameStateUI to find the card easily
        /// </summary>
        private int _id;
        private string _heroName;
        private string _heroClass;
        private string _heroDescription;
        private int _rollRequirement;
        private bool _dataReceived;

        //specific children of the border that contain the text components
        /// <summary>
        /// ONLY CHANGE THIS LIST IF THE UI ELEMENTS ARE RENAMED
        /// </summary>
        private List<string> _textComponentNames = new List<string> { "Name", "CardType", "Description", "RollReq" };
        private const string _buttonLocation = "Overlay";
        private const string _borderLocation = "Border";
        private const string _cardTypePrefix = "Hős:";
        private Transform _border;
        private List<Transform> _textComponents;
        #endregion

        //
        //might need to change to private later
        /// <summary>
        /// needed for GameStateUI to find the card easily
        /// </summary>
        public int Id { get => _id; }
        public string HeroName { get => _heroName; }
        public string HeroClass { get => _heroClass; }
        //public string ClassImagePath;
        //public string HeroImagePath;
        public string HeroDescription { get => _heroDescription; }
        public string RollRequirement { get => _rollRequirement.ToString(); }




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        /// <summary>
        /// Might need to disable it if performance is an issue. It only needs 1 update after all
        /// </summary>
        void Update()
        {
            if (_dataReceived)
            {
                this.enabled = false;
            }
        }

        #region Public methods
        /// <summary>
        /// Public method to set the data of the hero card
        /// </summary>
        /// <param name="heroName"></param>
        /// <param name="heroClass"></param>
        /// <param name="heroDescription"></param>
        /// <param name="rollRequirement"></param>
        public void SetHeroData(int id, string heroName, string heroClass, string heroDescription, int rollRequirement)
        {
            _id = id;
            _heroName = heroName;
            _heroClass = heroClass;
            _heroDescription = heroDescription;
            _rollRequirement = rollRequirement;
            Debug.Log($"HeroCardUI.SetHeroData: Hero name: {heroName}, Hero class: {heroClass}, Hero description: {heroDescription}, Roll requirement: {rollRequirement}");
            SetData();
            _dataReceived = true;
        }
        #endregion
        #region Private methods

        /// <summary>
        /// Gets the text components of the border
        /// </summary>
        /// <param name="border"></param>
        /// <returns>list containing NameText, CardTypeText, DescriptionText, RollReqText</returns>
        private List<Transform> GetTextComponents(Transform border)
        {
            List<Transform> textComponents = new List<Transform>();

            foreach (string textComponentName in _textComponentNames)
            {
                foreach (Transform child in border)
                {
                    if (child.name.Contains(textComponentName))
                    {
                        Transform grandchild = child.GetChild(0);
                        textComponents.Add(grandchild);
                    }
                }
            }

            return textComponents;
        }
        /// <summary>
        /// Private method that sets the data of the UI elements
        /// </summary>
        private void SetData()
        {
            GetComponents();
            /*_textComponents[0].GetComponent<TextMeshProUGUI>().text = HeroName;
            _textComponents[1].GetComponent<TextMeshProUGUI>().text = $"{_cardTypePrefix} {HeroClass}";
            _textComponents[2].GetComponent<TextMeshProUGUI>().text = HeroDescription;
            _textComponents[3].GetComponent<TextMeshProUGUI>().text = RollRequirement;*/
            Debug.Log("HeroCardUI.SetData: Data set");

        }
        /// <summary>
        /// Private method that gets the components of the hero card
        /// Needed because other Monobehaviour objects might call SetHeroData before the components are initialized
        /// </summary>
        private void GetComponents()
        {
            _dataReceived = false;
            //get the border and text components
            _border = transform.Find(_borderLocation);
            //Debug.Log($"HeroCardUI: {_border.name}");
            _textComponents = GetTextComponents(_border);
            //foreach (Transform textComponent in _textComponents)
            //{
            //    Debug.Log($"HeroCardUI: text component names: {textComponent.name}");
            //}
        }
        #endregion
    }
}

