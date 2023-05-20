using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerDeck : MonoBehaviour
{
    public List<Card> cardsInDeck;
    public int avgDeckDmg;
    public int avgDeckDefense;
    public int maxDeckSize;
    public int deckSize;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        maxDeckSize = 40;
    }

    // Update is called once per frame
    void Update()
    {
        deckSize = cardsInDeck.Count;
        calculateAverageDamage();
        calculateAverageDefense();
    }

    public void fillDeck()
    {
        for (int i = 0; i < maxDeckSize; i++)
        {
            cardsInDeck.Add(Database.GetRandomCard());
        }
    }
    public void calculateAverageDamage()
    {
        avgDeckDmg = (int)cardsInDeck.Average(x => x.attack);
    }
    public void calculateAverageDefense()
    {
        avgDeckDefense = (int)cardsInDeck.Average(x => x.defense);
    }
    public void sumUpCardTypes()
    {
        //for each card in cardsindeck if
        //amountFiend //amountDragon //amountZombie //amountWarrior
        //++respective type
    }
}
