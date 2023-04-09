using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public PlayerDeck playerDeck;
    public GameObject allCardsPanel;
    public List<Card> allCards;
    public List<Card> playerCards;
    public PlayerDeck tempDeck;

    public GameObject buttonTemplatePrefab;
    // Start is called before the first frame update
    void Start()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();
        allCards = Database.instance.cards.cardList;
        spawnButtonsAllCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnButtonsAllCards()
    {
        foreach(Card card in allCards)
        {
            GameObject button = Instantiate(buttonTemplatePrefab);
            button.transform.SetParent(allCardsPanel.transform, false);
            button.transform.parent = allCardsPanel.transform;
            button.GetComponent<CardButton>().SetStats(card);
            button.GetComponent<CardButton>().SetTexts();
        }
    }

    public void createDeck()
    {
        playerDeck.cardsInDeck = tempDeck.cardsInDeck;
    }
}
