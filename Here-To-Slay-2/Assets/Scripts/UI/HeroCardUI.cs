using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

using System;

using Systems.Initialization;

public class HeroCardUI : MonoBehaviour
{
    //there are 2 ways to use UI with ECS:
    //1. Monobehaviour listens to events from ECS
    //2. Monobehaviour reads data from ECS
    //we will use the first method
    public Action<bool> OnHeroCardUIFinished;
    
    //Data that will be read from ECS
    //might need to change to private later
    public string HeroName;
    public string HeroClass;
    //public string ClassImagePath;
    //public string HeroImagePath;
    public string HeroDescription;
    public string RollRequirement;

    public bool IsActive;

    /// <summary>
    /// ONLY CHANGE THIS LIST IF THE UI ELEMENTS ARE RENAMED
    /// </summary>
    private List<string> _textComponentNames = new List<string> { "Name", "CardType", "Description", "RollReq" };
    private const string _cardTypePrefix = "Hős:";
    private Transform _border;
    private List<Transform> _textComponents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //subscribe to the event
        var cardCreatorSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CardCreatorSystem>();
        cardCreatorSystem.OnHeroCardCreated += ReceiveHeroData;

        //get the border and text components
        IsActive = false;
        _border = GetBorder(transform);
        Debug.Log($"HeroCardUI: {_border.name}");
        _textComponents = GetTextComponents(_border);
        foreach (Transform textComponent in _textComponents)
        {
            Debug.Log($"HeroCardUI: text component names: {textComponent.name}");
        }
    }

    /// <summary>
    /// Might need to disable it if performance is an issue. It only needs 1 update after all
    /// </summary>
    void Update()
    {
        if (IsActive)
        {
            SetData();
            //answer back to CardCreatorSystem
            OnHeroCardUIFinished?.Invoke(true);
            IsActive = false;
        }
    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if (World.DefaultGameObjectInjectionWorld == null) return;
        var cardCreatorSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<CardCreatorSystem>();
        cardCreatorSystem.OnHeroCardCreated -= ReceiveHeroData;
    }

    private void ReceiveHeroData(List<string> heroCardData)
    {
        HeroName = heroCardData[0];
        HeroClass = heroCardData[1];
        HeroDescription = heroCardData[2];
        RollRequirement = heroCardData[3];
        IsActive = true;
    }

    private Transform GetBorder(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        return children[0];
    }
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
    private void SetData()
    {
        //sets the data of the UI elements
        _textComponents[0].GetComponent<TextMeshProUGUI>().text = HeroName;
        _textComponents[1].GetComponent<TextMeshProUGUI>().text = $"{_cardTypePrefix} {HeroClass}";
        _textComponents[2].GetComponent<TextMeshProUGUI>().text = HeroDescription;
        _textComponents[3].GetComponent<TextMeshProUGUI>().text = RollRequirement;


    }
}
