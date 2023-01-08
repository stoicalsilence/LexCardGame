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
    public enum ACTION { CHOOSING, CONFIRMING, BOARDVIEW, PLACINGCARD}
    public ACTION currentAction;

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
        }
        if (currentAction == ACTION.BOARDVIEW && Input.GetKeyDown(KeyCode.S))
        {
            currentAction = ACTION.CHOOSING;
        }

        if (currentAction == ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.S))
        {
            currentAction = ACTION.CONFIRMING;
            hand.Add(cardToPlace);
            cardToPlace.selected = true;
            SetLayerAllChildren(hand[0].gameObject.transform, LayerMask.NameToLayer("Default"));
            SetLayerAllChildren(hand[1].gameObject.transform, LayerMask.NameToLayer("Default"));
            SetLayerAllChildren(hand[2].gameObject.transform, LayerMask.NameToLayer("Default"));
            SetLayerAllChildren(hand[3].gameObject.transform, LayerMask.NameToLayer("Default"));
            SetLayerAllChildren(cardToPlace.transform, LayerMask.NameToLayer("Default"));
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
        cardToPlace = playingCard;
        hand.Remove(playingCard);
        currentAction = ACTION.PLACINGCARD;
        //canvasCamera.gameObject.SetActive(false);
        //handGO.gameObject.SetActive(false);
        SetLayerAllChildren(hand[0].gameObject.transform, LayerMask.NameToLayer("DontRender"));
        SetLayerAllChildren(hand[1].gameObject.transform, LayerMask.NameToLayer("DontRender"));
        SetLayerAllChildren(hand[2].gameObject.transform, LayerMask.NameToLayer("DontRender"));
        SetLayerAllChildren(hand[3].gameObject.transform, LayerMask.NameToLayer("DontRender"));
        SetLayerAllChildren(playingCard.transform, LayerMask.NameToLayer("DontRender"));
        //hand[4].gameObject.layer = LayerMask.NameToLayer("DontRender");
        //playingCard.gameObject.layer = LayerMask.NameToLayer("DontRender");
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //            Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }
}
