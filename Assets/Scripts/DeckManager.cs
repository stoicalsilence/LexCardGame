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
        if (deck.cardsInDeck.Count == 40)
        {
            PlayerPrefsExtra.SetList("deck", deck.cardsInDeck);
            Debug.Log("Saved Deck!" + " Amount of cards in deck:" + deck.cardsInDeck.Count);
        }
        else
        {
            Debug.Log("Couldnt save deck. Deck needs to have 40 cards.");
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
}
