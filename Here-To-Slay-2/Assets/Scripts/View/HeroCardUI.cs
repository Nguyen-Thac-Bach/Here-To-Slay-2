using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using System;



public class HeroCardUI : MonoBehaviour
{

    #region Fields
    private string _heroName;
    private string _heroClass;
    private string _heroDescription;
    private int _rollRequirement;

    //specific children of the border that contain the text components
    /// <summary>
    /// ONLY CHANGE THIS LIST IF THE UI ELEMENTS ARE RENAMED
    /// </summary>
    private List<string> _textComponentNames = new List<string> { "Name", "CardType", "Description", "RollReq" };
    private const string _cardTypePrefix = "Hős:";
    private Transform _border;
    private List<Transform> _textComponents;
    #endregion

    //
    //might need to change to private later
    public string HeroName { get => _heroName; } 
    public string HeroClass { get => _heroClass; }
    //public string ClassImagePath;
    //public string HeroImagePath;
    public string HeroDescription { get => _heroDescription; }
    public string RollRequirement { get => _rollRequirement.ToString(); }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get the border and text components
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
        Debug.Log("HeroCardUI: Starting SetData()");
        SetData();
        Debug.Log($"HeroCardUI: Data set for card: {HeroName}");
        this.enabled = false;
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
    /// <summary>
    /// sets the data of the UI elements
    /// </summary>
    private void SetData()
    {
        
        _textComponents[0].GetComponent<TextMeshProUGUI>().text = HeroName;
        _textComponents[1].GetComponent<TextMeshProUGUI>().text = $"{_cardTypePrefix} {HeroClass}";
        _textComponents[2].GetComponent<TextMeshProUGUI>().text = HeroDescription;
        _textComponents[3].GetComponent<TextMeshProUGUI>().text = RollRequirement;


    }
}
