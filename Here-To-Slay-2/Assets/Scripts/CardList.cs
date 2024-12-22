using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CardList : MonoBehaviour
{
    //If debugging is needed, change to public
    private List<GameObject> _cardList;

    public List<GameObject> Cards
    {
        get { return _cardList; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cardList = new List<GameObject>();
    }

    public void AddCard(GameObject card)
    {
        _cardList.Add(card);
        Debug.Log($"CardList: Added card: {card.GetComponent<HeroCardUI>().HeroName}");
    }


}
