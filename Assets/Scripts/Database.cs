using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour
{
    public CardDataBase cards;
    public static Database instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Card GetCardById(int id)
    {
        return instance.cards.cardList.FirstOrDefault(c => c.id == id);

        //foreach(Card card in instance.cards.cardList)
        //{
        //    if(card.id == id)
        //    {
        //        return card;
        //    }
        //}
        //Debug.Log("Did not find card to return");
        //return null;
    }

    public static Card GetRandomCard()
    {
        return instance.cards.cardList[Random.Range(0, instance.cards.cardList.Count())];
    }
    public static CardDataBase GetCardDatabase()
    {
        return instance.cards;
    }
    }
