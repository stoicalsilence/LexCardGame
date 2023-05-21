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
        //Invoke("orderAllCardsById",1);
        //Invoke("orderAllPlayerCardsById",1);
    }

    // Update is called once per frame
    void Update()
    {
        avgDmgText.text = "Average Dmg: " + playerDeck.avgDeckDmg.ToString();
        avgDfsText.text = "Average Dfs: " + playerDeck.avgDeckDefense.ToString();
        cardCountInDeck.text = FindObjectOfType<DeckManager>().modifiedDeck.Count.ToString();

        if (Input.anyKeyDown)
        {
            switch (Input.inputString)
            {
                case "1":
                    OrderCardsByType(Card.TYPE.THUNDER);
                    break;
                case "2":
                    OrderCardsByType(Card.TYPE.MACHINE);
                    break;
                case "3":
                    OrderCardsByType(Card.TYPE.ROCK);
                    break;
                case "4":
                    OrderCardsByType(Card.TYPE.FIRE);
                    break;
                case "5":
                    OrderCardsByType(Card.TYPE.WATER);
                    break;
                case "6":
                    OrderCardsByType(Card.TYPE.DRAGON);
                    break;
                case "7":
                    OrderCardsByType(Card.TYPE.WARRIOR);
                    break;
                case "8":
                    OrderCardsByType(Card.TYPE.FAIRY);
                    break;
                case "[": // Keypad number 1
                    OrderCardsByType(Card.TYPE.INSECT);
                    break;
                case "]": // Keypad number 2
                    OrderCardsByType(Card.TYPE.ZOMBIE);
                    break;
                case "/": // Keypad number 3
                    OrderCardsByType(Card.TYPE.BEAST);
                    break;
                case "*": // Keypad number 4
                    OrderCardsByType(Card.TYPE.PLANT);
                    break;
                case "-": // Keypad number 5
                    OrderCardsByType(Card.TYPE.WINGEDBEAST);
                    break;
                case "+": // Keypad number 6
                    OrderCardsByType(Card.TYPE.SPELLCASTER);
                    break;
                case ".": // Keypad number 7
                    OrderCardsByType(Card.TYPE.FIEND);
                    break;
                    // Add more cases for other card types as needed
            }
        }
    }

    public void spawnButtonsAllCards()
    {
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
        foreach (Card card in ToCardList(FindObjectOfType<DeckManager>().modifiedDeck))
        {
            GameObject button = Instantiate(buttonTemplatePrefab);
            button.transform.SetParent(playerDeckCardsPanel.transform, false);
            button.transform.parent = playerDeckCardsPanel.transform;
            button.GetComponent<CardButton>().SetStats(card);
            button.GetComponent<CardButton>().SetTexts();
        }
    }
    public List<Card> ToCardList(List<int> idlist)
    {
        List<Card> cardList = new List<Card>();

        foreach (int i in idlist)
        {
            Card card = Database.GetCardById(i);
            cardList.Add(card);
        }

        return cardList;
    }

    public void orderAllCardsById()
    {
        allCards.Clear();

        // Get all the child Button components of the parent GameObject
        Button[] buttons = allCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));
        //allCards = Database.instance.cards.cardList.OrderBy(cards => cards.id).ToList();
        allCards = ToCardList(FindObjectOfType<DeckManager>().modifiedAllCards);
        allCards = allCards.OrderBy(cards => cards.id).ToList();
        //allCards = fixedModifiedAllACards.OrderBy(cards => cards.id).ToList(); //- REINSTATE THIS TO REMOVE CHEATS
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
        //allCards = Database.instance.cards.cardList.OrderByDescending(cards => cards.attack).ToList();
        
        allCards = ToCardList(FindObjectOfType<DeckManager>().modifiedAllCards).OrderByDescending(cards => cards.attack).ToList();
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
        //ToCardList(FindObjectOfType<DeckManager>().modifiedDeck).OrderBy(cards => cards.id).ToList();
        //playerDeck.cardsInDeck = playerDeck.cardsInDeck.OrderBy(cards => cards.id).ToList();
        List<Card> tempCardList = ToCardList(FindObjectOfType<DeckManager>().modifiedDeck);
        tempCardList = tempCardList.OrderBy(cards => cards.id).ToList();
        FindObjectOfType<DeckManager>().modifiedDeck = FindObjectOfType<DeckManager>().ToIdList(tempCardList); ;
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
        //playerDeck.cardsInDeck = playerDeck.cardsInDeck.OrderByDescending(cards => cards.attack).ToList();
        List<Card> tempCardList = ToCardList(FindObjectOfType<DeckManager>().modifiedDeck);
        tempCardList = tempCardList.OrderByDescending(cards => cards.attack).ToList();
        FindObjectOfType<DeckManager>().modifiedDeck = FindObjectOfType<DeckManager>().ToIdList(tempCardList);
        playerCardsOrderingByAttack = true;
        playerCardsOrderingById = false;
        spawnButtonsPlayerCards();
    }

    public void OrderCardsByType(Card.TYPE type)
    {
        allCards.Clear();

        // Get all the child Button components of the parent GameObject
        Button[] buttons = allCardsPanel.transform.GetComponentsInChildren<Button>();

        // Destroy each Button
        buttons.ToList().ForEach(button => Destroy(button.gameObject));

        // Filter cards based on the specified type
        allCards = ToCardList(FindObjectOfType<DeckManager>().modifiedAllCards)
            .Where(card => card.type == type)
            .OrderBy(card => card.id)
            .ToList();

        allCardsOrderingById = true;
        allCardsOrderingByAttack = false;
        spawnButtonsAllCards();
    }



    public void removeCardFromDeck()
    {
        CardButton[] buttons = playerDeckCardsPanel.transform.GetComponentsInChildren<CardButton>();
        foreach (CardButton button in buttons)
        {
            if (button.transform.parent.name.Equals("PlayerCards"))
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
                    FindObjectOfType<DeckManager>().AddIdToModifiedDeck(card.id); //reinsatte this to have real game logic. broken rn tho.
                    FindObjectOfType<DeckManager>().RemoveCardIDInDeck(card.id);
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
                    FindObjectOfType<DeckManager>().RemoveFirstInstanceOfNumber(card.id); //reinsatte this to have real game logic. broken rn tho.
                    FindObjectOfType<DeckManager>().AddIdToActualModifiedDeck(card.id);
                    reorderCards();
                }
            }
        }
    }

    public void saveDeck()
    {
        if (playerDeck.cardsInDeck.Count == 40)
        {
            //FindObjectOfType<DeckManager>().saveDeck();
            FindObjectOfType<DeckManager>().SubmitChanges();
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
