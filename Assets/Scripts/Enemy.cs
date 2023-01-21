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

    public Tile chosenTile;

    public void Start()
    {
        gameEvaluation = FindObjectOfType<GameEvaluation>();
        fillDeck();
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
            //place strongestEnemyCardInHand
        }
        else
        {
            //place strongestEnemyDefenseCardInHand
        }
    }

    public void placeCard(PlayingCard cardToPlace)
    {
        chosenTile.cardOnTile = cardToPlace;
    }
}