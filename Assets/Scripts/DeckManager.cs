using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    public PlayerDeck deck;

    [SerializeField]
    public List<Card> playerAllCollectedCards;
    // Start is called before the first frame update

    private string path = "";
    private string persistentPath = "";

    List<int> modifiedDeck = new List<int>();
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        setPath();
    }
    void Start()
    {
        deck = FindObjectOfType<PlayerDeck>();
        modifiedDeck = loadCollectedCardsIds();
        if (modifiedDeck.Count == 0)
        {
            modifiedDeck = new List<int>();

            // Create a random number generator
            System.Random random = new System.Random();

            // Fill modifiedDeck with 40 random integers
            for (int i = 0; i < 40; i++)
            {
                int randomNumber = random.Next(1, 61); // Generates a random integer between 1 and 60
                modifiedDeck.Add(randomNumber);
            }

            foreach(int i in modifiedDeck)
            {
                saveCollectedCard(i);
            }
            SubmitChanges();
        }
    }

    public List<Card> loadDeck()
    {
        //return PlayerPrefsExtra.GetList<Card>("deck");
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Debug.Log("loadjson: " + json);
        CardIdList cardIdList = JsonUtility.FromJson<CardIdList>(json);

        List<Card> cardList = new List<Card>();

        foreach(int i in cardIdList.ids)
        {
            Card card = Database.GetCardById(i);
            cardList.Add(card);
        }

        return cardList;
    }

    [System.Serializable]
    public class CardIdList
    {
        public List<int> ids;
    }

    public void saveDeck()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        List<int> idList = new List<int>();

        foreach (Card card in deck.cardsInDeck)
        {
            idList.Add(card.id);
        }

        CardIdList cardIdList = new CardIdList();
        cardIdList.ids = idList;

        string json = JsonUtility.ToJson(cardIdList);
        Debug.Log("Saving data at: " + path);
        Debug.Log(json);

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
        //overwrite allcollected cards with modified deck
    }

    public bool checkIfAllCollectedCardListExists()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public void createCollectedCardsList()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";

        List<int> idList = new List<int>();
        CardIdList cardIdList = new CardIdList();
        cardIdList.ids = idList;

        foreach(Card card in loadDeck())
        {
            idList.Add(card.id);
        }

        string json = JsonUtility.ToJson(cardIdList);
        Debug.Log("Saving data at: " + path);
        Debug.Log(json);
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
    }

    public void saveCollectedCard(int cardId)
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";

        string json2;
        using (StreamReader reader = new StreamReader(path))
        {
            string json = reader.ReadToEnd();
            Debug.Log("loadjson: " + json);
            CardIdList cardIdList = JsonUtility.FromJson<CardIdList>(json);
            cardIdList.ids.Add(cardId);

            json2 = JsonUtility.ToJson(cardIdList);
            Debug.Log("Saving data at: " + path);
            Debug.Log(json2);

        }
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json2);
            }
    }

    public List<Card> loadCollectedCards()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";

        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Debug.Log("loadjson: " + json);
        CardIdList cardIdList = JsonUtility.FromJson<CardIdList>(json);

        List<Card> cardList = new List<Card>();

        foreach (int i in cardIdList.ids)
        {
            Card card = Database.GetCardById(i);
            cardList.Add(card);
        }

        return cardList;
    }

    public List<int> loadCollectedCardsIds()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";

        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Debug.Log("loadjson: " + json);
        CardIdList cardIdList = JsonUtility.FromJson<CardIdList>(json);
        return cardIdList.ids;
    }
    //logic here would be: GAME WON -> check if file exists, if not, create the list and save it. if it does exist, savecollectedcard for each card won. can also be used for purchasing cards in shop.

    //this can be used to remove cards when doing Add To Deck, but gotta find a way to appy changes and not immediately remove from json.
    public void RemoveFirstInstanceOfNumber(int numberToRemove)
    {
        if (modifiedDeck.Contains(numberToRemove))
        {
            modifiedDeck.Remove(numberToRemove);
        }
    }

    public void AddIdToModifiedDeck(int idToAdd)
    {
        modifiedDeck.Add(idToAdd);
    }

    public void SubmitChanges()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";
        string json = JsonUtility.ToJson(modifiedDeck);
        File.WriteAllText(path, json);
        //modifiedDeck.Clear();
    }
    public List<Card> ShuffleDeck(List<Card> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card card = list[k];
            list[k] = list[n];
            list[n] = card;
        }
        return list;
    }

    private void setPath()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }
}
