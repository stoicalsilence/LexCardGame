using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string duelistName;
    public List<Card> deck;
    public int deckSize;
    public List<PlayingCard> hand;
    public GameEvaluation gameEvaluation;

    public PlayingCard cardPrefab;
    public PlayerHand handGO;

    public Tile chosenTile;
    public enum ACTION { CHOOSING, CONFIRMING, BOARDVIEW, PLACINGCARD }
    public ACTION currentAction;

    bool bool1;
    bool bool2;
    bool bool3;
    bool bool4;
    bool bool5;

    public void Start()
    {
        gameEvaluation = FindObjectOfType<GameEvaluation>();
        deckSize = 40;
        fillDeck();
    }

    public void Update()
    {
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

  public void prepareCardToPlace()
    {
        chosenTile = null;
        FieldCard strongestPlayerCard = gameEvaluation.findStrongestPlayerCard();

        PlayingCard strongestEnemyCardInHand = gameEvaluation.FindStrongestAttackEnemyCardInHand();
        PlayingCard strongestEnemyDefenseCardInHand = gameEvaluation.FindStrongestDefenseEnemyCardInHand();

        chosenTile = gameEvaluation.findEmptyEnemyTile();
        if (chosenTile == null)
        {
            chosenTile = gameEvaluation.FindTileWithWeakestEnemyCard();
        }

        if (strongestEnemyCardInHand.attack > strongestPlayerCard.attack)
        {
            StartCoroutine(placeCard(strongestEnemyCardInHand));
        }
        else
        {
            StartCoroutine(placeCard(strongestEnemyDefenseCardInHand));
        }
    }

    public IEnumerator placeCard(PlayingCard cardToPlace)
    {
        cardToPlace.selected = true;
        yield return new WaitForSeconds(1f);


        chosenTile.cardOnTile = cardToPlace;
        chosenTile.enemyDropCard();
    }

    public IEnumerator drawCards()
    {
        while (hand.Count < 5 && deck.Count > 0)
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
    }

    public void ResetLayers()
    {
        foreach (PlayingCard card in hand)
        {
            if (card != null)
            {
                SetLayerAllChildren(card.gameObject.transform, LayerMask.NameToLayer("Default"));
            }
        }
    }
    public void UnrenderCards()
    {
        foreach (PlayingCard card in hand)
        {
            if (card != null)
            {
                SetLayerAllChildren(card.gameObject.transform, LayerMask.NameToLayer("DontRender"));
            }
        }
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
}