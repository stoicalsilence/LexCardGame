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

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        deck = FindObjectOfType<PlayerDeck>();
    }

    public List<Card> loadDeck()
    {
        return PlayerPrefsExtra.GetList<Card>("deck");
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
        PlayerPrefsExtra.SetList("deck", deck.cardsInDeck);
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
