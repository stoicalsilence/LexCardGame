using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ButtonSpawner : MonoBehaviour
{
    public PlayerDeck playerDeck;
    public GameObject allCardsPanel;
    public List<Card> allCards;

    public GameObject playerDeckCardsPanel;

    public GameObject buttonTemplatePrefab;

    public GameObject addCardToDeckButton;
    public GameObject removeCardFromDeckButton;

    public TextMeshProUGUI avgDmgText;
    public TextMeshProUGUI avgDfsText;
    public TextMeshProUGUI cardCountInDeck;

    public bool allCardsOrderingById;
    public bool allCardsOrderingByAttack;
    public bool playerCardsOrderingById;
    public bool playerCardsOrderingByAttack;

    
    // Start is called before the first frame update
    void Start()
    {
        playerDeck = FindObjectOfType<Player>().deck;
        playerDeck.cardsInDeck = FindObjectOfType<DeckManager>().loadDeck();
        orderAllCardsById();
        orderAllPlayerCardsById();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnButtonsAllCards()
    {
        avgDmgText.text = "Average Dmg: " + playerDeck.avgDeckDmg.ToString();
        avgDfsText.text = "Average Dfs: " + playerDeck.avgDeckDefense.ToString();
        cardCountInDeck.text = playerDeck.cardsInDeck.Count.ToString();
        foreach (Card card in allCards)
        {
            GameObject button = Instantiate(buttonTemplatePrefab);
            button.transform.SetParent(allCardsPanel.transform, false);
            button.transform.parent = allCardsPanel.transform;
            button.GetComponent<CardButton>().SetStats(card);
            button.GetComponent<CardButton>().SetTexts();
        }
    }

    public void spawnButtonsPlayerCards()
    {
        foreach (Card card in playerDeck.cardsInDeck)
        {
            GameObject button = Instantiate(buttonTemplatePrefab);
            button.transform.SetParent(playerDeckCardsPanel.transform, false);
            button.transform.parent = playerDeckCardsPanel.transform;
            button.GetComponent<CardButton>().SetStats(card);
            button.GetComponent<CardButton>().SetTexts();
        }
    }

    public void orderAllCardsById()
    {
        allCards.Clear();

        // Get all the child Button components of the parent GameObject
        Button[] buttons = allCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        allCards = Database.instance.cards.cardList.OrderBy(cards => cards.id).ToList();
        //allCards = FindObjectOfType<DeckManager>().loadCollectedCards().OrderBy(cards => cards.id).ToList(); //- REINSTATE THIS TO REMOVE CHEATS
        allCardsOrderingById = true;
        allCardsOrderingByAttack = false;
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
        //allCards = FindObjectOfType<DeckManager>().loadCollectedCards().OrderByDescending(cards => cards.attack).ToList(); //<-- REINSTATE THIS TO REMOVE CHEATS
        allCardsOrderingById = false;
        allCardsOrderingByAttack = true;
        spawnButtonsAllCards();
    }

    public void orderAllPlayerCardsById()
    {
        // Get all the child Button components of the parent GameObject
        Button[] buttons = playerDeckCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        playerDeck.cardsInDeck = playerDeck.cardsInDeck.OrderBy(cards => cards.id).ToList();
        playerCardsOrderingById = true;
        playerCardsOrderingByAttack = false;
        spawnButtonsPlayerCards();
    }

    public void orderAllPlayerCardsByAttack()
    {
        // Get all the child Button components of the parent GameObject
        Button[] buttons = playerDeckCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        playerDeck.cardsInDeck = playerDeck.cardsInDeck.OrderByDescending(cards => cards.attack).ToList();
        playerCardsOrderingByAttack = true;
        playerCardsOrderingById = false;
        spawnButtonsPlayerCards();
    }

    public void removeCardFromDeck()
    {
        CardButton[] buttons = playerDeckCardsPanel.transform.GetComponentsInChildren<CardButton>();
        foreach (CardButton button in buttons)
        {
            if(button.transform.parent.name.Equals("PlayerCards"))
            {
                if (button.selected == true)
                {
                    Debug.Log("wee");
                    button.selected = false;
                    string targetCardName = button.cardName;
                    Card card = playerDeck.cardsInDeck.Find(x => x.cardName == targetCardName);
                    button.gameObject.transform.parent = null;
                    button.gameObject.transform.parent = allCardsPanel.transform;
                    allCards.Add(card);
                    //FindObjectOfType<DeckManager>().AddIdToModifiedDeck(card.id); //reinsatte this to have real game logic. broken rn tho.
                    playerDeck.cardsInDeck.Remove(card);
                    reorderCards();
                }
            }
        }
    }

    public void addCardToDeck()
    {
        CardButton[] buttons = allCardsPanel.transform.GetComponentsInChildren<CardButton>();

        foreach (CardButton button in buttons)
        {
            if (button.transform.parent.name == "AllCards")
            {
                if (button.selected)
                {
                    button.selected = false;
                    string targetCardName = button.cardName;
                    Card card = allCards.Find(x => x.cardName == targetCardName);
                    allCards.Remove(card);
                    //FindObjectOfType<DeckManager>().RemoveFirstInstanceOfNumber(card.id); //reinsatte this to have real game logic. broken rn tho.
                    playerDeck.cardsInDeck.Add(card);
                    reorderCards();
                }
            }
        }
    }

    public void saveDeck()
    {
        if (playerDeck.cardsInDeck.Count == 40)
        {
            FindObjectOfType<DeckManager>().saveDeck();
            //FindObjectOfType<DeckManager>().SubmitChanges();
        }
        else
        {
            Debug.Log("Could not save deck. Deck must contain exactly 40 cards.");
        }
    }

    public void reorderCards()
    {
        playerDeck.calculateAverageDamage();
        playerDeck.calculateAverageDefense();
        playerDeck.sumUpCardTypes();
        if (playerCardsOrderingById)
        {
            orderAllPlayerCardsById();
        }
        else
        {
            orderAllPlayerCardsByAttack();
        }
        if (allCardsOrderingById)
        {
            orderAllCardsById();
        }
        else
        {
            orderAllCardsByAttack();
        }
    }
}
