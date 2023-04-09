using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    public PlayerDeck deck;

    [SerializeField]
    public List<Card> playerAllCollectedCards;
    // Start is called before the first frame update
    void Start()
    {
        loadDeck();
        loadPlayerAllCollectedCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadDeck()
    {
        string json = PlayerPrefs.GetString("Deck");
        if (!string.IsNullOrEmpty(json))
        {
            deck = JsonConvert.DeserializeObject<PlayerDeck>(json);
        }
    }

    public void loadPlayerAllCollectedCards()
    {
        string json = PlayerPrefs.GetString("PlayerAllCollectedCards");
        if (!string.IsNullOrEmpty(json))
        {
            playerAllCollectedCards = JsonConvert.DeserializeObject<List<Card>>(json);
        }
    }

    public void saveDeck()
    {
        string json = JsonConvert.SerializeObject(deck);
        PlayerPrefs.SetString("Deck", json);
        PlayerPrefs.Save();
    }

    public void savePlayerAllCollectedCards()
    {
        string json = JsonConvert.SerializeObject(playerAllCollectedCards);
        PlayerPrefs.SetString("PlayerAllCollectedCards", json);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        saveDeck();
        savePlayerAllCollectedCards();
    }
}
