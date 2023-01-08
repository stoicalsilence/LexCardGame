using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
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

    //chose card to play? track chosen card place it down, player has hand, chosen card gets removed from hand and added to gamegrid
    //deck
    //cards get moved from deck and into hand
    void Start()
    {
        deckSize = 40;
        fillDeck();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            drawCards();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentAction = ACTION.BOARDVIEW;
            UnrenderCards();
        }
        if (currentAction == ACTION.BOARDVIEW && Input.GetKeyDown(KeyCode.S))
        {
            currentAction = ACTION.CHOOSING;
            ResetLayers();
        }

        if (currentAction == ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.S))  //after having played card i dont think should be able to return to confirm
        {//add bool "watchingcardplace", have all this stuff only work if watchingcardplace is false. if watchingcardplace then you gotta just wait at boardview. during which card drops down from above onto field
            currentAction = ACTION.CHOOSING;
            //hand.Add(cardToPlace);
            cardToPlace.selected = false;
            cardToPlace.transform.position = cardToPlace.originalPos;
            ResetLayers();
        }
    }

    public void fillDeck()
    {   
        for (int i = 0; i < deckSize; i++)
        {
            deck.Add(Database.GetRandomCard());
        }
    }

    public void drawCards()
    {
        while(hand.Count < 5 && deck.Count > 0)
        {
            var playingCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            playingCard.SetUI(deck[0]);
            playingCard.SetStats(deck[0]);
            hand.Add(playingCard);
            //hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        hand[0].transform.position = handGO.slot1.position;
        hand[1].transform.position = handGO.slot2.position;
        hand[2].transform.position = handGO.slot3.position;
        hand[3].transform.position = handGO.slot4.position;
        hand[4].transform.position = handGO.slot5.position;
    }

    public void placeCard(PlayingCard playingCard)
    {
        UICardToDelete = playingCard;
        cardToPlace = playingCard;
        hand.Remove(playingCard);
        currentAction = ACTION.PLACINGCARD;
        //canvasCamera.gameObject.SetActive(false);
        //handGO.gameObject.SetActive(false);
        UnrenderCards();
        SetLayerAllChildren(playingCard.transform, LayerMask.NameToLayer("DontRender"));
        //hand[4].gameObject.layer = LayerMask.NameToLayer("DontRender");
        //playingCard.gameObject.layer = LayerMask.NameToLayer("DontRender");
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
        if (cardToPlace.gameObject != null)
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
