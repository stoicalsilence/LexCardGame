using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public static PlayerDeck instance;
    public List<Card> cardsInDeck;
    public int avgDeckDmg;
    public int avgDeckDefense;
    public int deckSize;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        deckSize = 40;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fillDeck()
    {
        for (int i = 0; i < deckSize; i++)
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