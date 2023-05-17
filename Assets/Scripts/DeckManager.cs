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
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        setPath();
    }
    void Start()
    {
        deck = FindObjectOfType<PlayerDeck>();
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

        //Debug.Log("loaded deck:");
        //foreach(Card car in cardList) { Debug.Log(car.cardName); }
        return cardList;
    }

    //public void loadPlayerAllCollectedCards()
    //{
    //    string json = PlayerPrefs.GetString("PlayerAllCollectedCards");
    //    if (!string.IsNullOrEmpty(json))
    //    {
    //        playerAllCollectedCards = JsonConvert.DeserializeObject<List<Card>>(json);
    //    }
    //}

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
    }

    public void saveCollectedCard(int cardId)
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "AllCollectedCards.json";

        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Debug.Log("loadjson: " + json);
        CardIdList cardIdList = JsonUtility.FromJson<CardIdList>(json);
        cardIdList.ids.Add(cardId);

        string json2 = JsonUtility.ToJson(cardIdList);
        Debug.Log("Saving data at: " + path);
        Debug.Log(json2);

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
    }
    //logic here would be: GAME WON -> check if file exists, if not, create the list and save it. if it does exist, savecollectedcard for each card won. can also be used for purchasing cards in shop.

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
    public void savePlayerAllCollectedCards()
    {
        string json = JsonConvert.SerializeObject(playerAllCollectedCards);
        PlayerPrefs.SetString("PlayerAllCollectedCards", json);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        //saveDeck();
        savePlayerAllCollectedCards();
    }

    private void setPath()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }
}
