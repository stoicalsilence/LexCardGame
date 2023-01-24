using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string duelistName; //player fill this in at start
    public PlayerHand handGO;
    public List<PlayingCard> hand;

    public Camera canvasCamera; 

    public CardDataBase cardDataBase;
    public List<Card> deck;
    public int deckSize;

    public PlayingCard cardPrefab;

    public PlayingCard cardToPlace;

    public PlayingCard UICardToDelete;
    public enum ACTION { CHOOSING, CONFIRMING, BOARDVIEW, PLACINGCARD}
    public ACTION currentAction;

    public bool playedCard;
    public int lifepoints;
    public TextMeshProUGUI lifepointText;

    public bool drawingCards;
    bool bool1;
    bool bool2;
    bool bool3;
    bool bool4;
    bool bool5;

    //chose card to play? track chosen card place it down, player has hand, chosen card gets removed from hand and added to gamegrid
    //deck
    //cards get moved from deck and into hand
    void Start()
    {
        lifepoints = 8000;
        deckSize = 40;
        fillDeck();
        StartCoroutine(drawCards());
    }

    // Update is called once per frame
    void Update()
    {
        if (!playedCard) { 

        if (Input.GetKeyDown(KeyCode.W) && currentAction != ACTION.CONFIRMING)
        {
            currentAction = ACTION.BOARDVIEW;
            UnrenderCards();
        }
        if (currentAction == ACTION.BOARDVIEW && Input.GetKeyDown(KeyCode.S))
        {
            currentAction = ACTION.CHOOSING;
            ResetLayers();
        }

        if (currentAction == ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.S)) 
            {
            currentAction = ACTION.CHOOSING;
            //hand.Add(cardToPlace);
            cardToPlace.selected = false;
            cardToPlace.transform.position = cardToPlace.originalPos;
            ResetLayers();
        }
        }

        if (bool1)
        {
            hand[0].transform.position = Vector3.Lerp(hand[0].transform.position, handGO.slot1.position, Time.deltaTime * 17);
        }
        if (bool2)
        {
            hand[1].transform.position = Vector3.Lerp(hand[1].transform.position, handGO.slot2.position, Time.deltaTime * 17);
        }
        if (bool3)
        {
            hand[2].transform.position = Vector3.Lerp(hand[2].transform.position, handGO.slot3.position, Time.deltaTime * 17);
        }
        if (bool4)
        {
            hand[3].transform.position = Vector3.Lerp(hand[3].transform.position, handGO.slot4.position, Time.deltaTime * 17);
        }
        if (bool5)
        {
            hand[4].transform.position = Vector3.Lerp(hand[4].transform.position, handGO.slot5.position, Time.deltaTime * 17);
        }

    }

    public void fillDeck()
    {   
        for (int i = 0; i < deckSize; i++)
        {
            deck.Add(Database.GetRandomCard());
        }
    }

    public IEnumerator drawCards()
    {
        drawingCards = true;
        while(hand.Count < 5 && deck.Count > 0)
        {
            var playingCard = Instantiate(cardPrefab, handGO.spawnSpot.position, Quaternion.identity);
            playingCard.SetUI(deck[0]);
            playingCard.SetStats(deck[0]);
            hand.Add(playingCard);
            //hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        yield return new WaitForSeconds(0.20f);
        bool1 = true;
        yield return new WaitForSeconds(0.20f);
        bool1 = false;
        bool2 = true;
        yield return new WaitForSeconds(0.20f);
        bool2 = false;
        bool3 = true;
        yield return new WaitForSeconds(0.20f);
        bool3 = false;
        bool4 = true;
        yield return new WaitForSeconds(0.20f);
        bool4 = false;
        bool5 = true;
        yield return new WaitForSeconds(0.20f);
        bool5 = false;

        drawingCards = false;
    }
    public void prepareToPlace(PlayingCard playingCard)
    {
        UICardToDelete = playingCard;
        cardToPlace = playingCard;
        currentAction = ACTION.PLACINGCARD;
        UnrenderCards();
        SetLayerAllChildren(playingCard.transform, LayerMask.NameToLayer("DontRender"));
    }
    public void placeCard(PlayingCard playingCard)
    {
        hand[0].transform.position = handGO.slot1.position;
        hand[1].transform.position = handGO.slot2.position;
        hand[2].transform.position = handGO.slot3.position;
        hand[3].transform.position = handGO.slot4.position;
        hand[4].transform.position = handGO.slot5.position;
        hand.Remove(playingCard);
    }
    

    public void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //            Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }

    public void ResetLayers()
    {
        foreach(PlayingCard card in hand)
        {
            if(card != null)
            {
                SetLayerAllChildren(card.gameObject.transform, LayerMask.NameToLayer("Default"));
            }
        }
        if (cardToPlace != null)
        {
            SetLayerAllChildren(cardToPlace.transform, LayerMask.NameToLayer("Default"));
        }
    }
    public void UnrenderCards()
    {
        foreach(PlayingCard card in hand)
        {
            if(card != null)
            {
                SetLayerAllChildren(card.gameObject.transform, LayerMask.NameToLayer("DontRender"));
            }
        }
    }
}
