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
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        CardIdList cardIdList = new CardIdList();
        cardIdList.ids = JsonUtility.FromJson<List<int>>(json);

        List<Card> cardList = new List<Card>();

        foreach(int i in cardIdList.ids)
        {
            Card card = Database.GetCardById(i);
            cardList.Add(card);
        }

        return cardList;
    }

    public void loadPlayerAllCollectedCards()
    {
        string json = PlayerPrefs.GetString("PlayerAllCollectedCards");
        if (!string.IsNullOrEmpty(json))
        {
            playerAllCollectedCards = JsonConvert.DeserializeObject<List<Card>>(json);
        }
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
