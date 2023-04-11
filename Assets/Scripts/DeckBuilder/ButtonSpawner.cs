using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
        orderAllCardsById();
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
        //deckmanager.savedeck
        playerDeck.cardsInDeck = tempDeck.cardsInDeck;
    }

    public void orderAllCardsById()
    {
        allCards.Clear();
        
        // Get all the child Button components of the parent GameObject
        Button[] buttons = allCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        allCards = Database.instance.cards.cardList.OrderBy(cards => cards.id).ToList();
        spawnButtonsAllCards();
    }

    public void orderAllCardsByAttack()
    {
        allCards.Clear();

        // Get all the child Button components of the parent GameObject
        Button[] buttons = allCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        allCards = Database.instance.cards.cardList.OrderByDescending(cards => cards.attack).ToList();
        spawnButtonsAllCards();
    }
}
