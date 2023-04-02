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
    public bool replacingCard;
    public enum ACTION { CHOOSING, CONFIRMING, BOARDVIEW, PLACINGCARD }
    public ACTION currentAction;

    public int lifepoints;
    public TextMeshProUGUI lifepointText;
    bool bool1;
    bool bool2;
    bool bool3;
    bool bool4;
    bool bool5;
    public bool lastActionWasAttack;


    public void Start()
    {
        lifepoints = 8000;
        gameEvaluation = FindObjectOfType<GameEvaluation>();
        deckSize = 40;
        fillDeck();
    }

    public void Update()
    {
        lifepointText.text = lifepoints.ToString();
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

  public IEnumerator prepareCardToPlace()
    {
        replacingCard = false;
        chosenTile = null;
        FieldCard strongestPlayerCard = gameEvaluation.findStrongestPlayerCard();   //TODO REALLY: check if facedown or not
        FieldCard strongestPlayerCardAttackMode = gameEvaluation.findStrongestPlayerCardOnlyAttackMode();

        PlayingCard strongestEnemyCardInHand = gameEvaluation.FindStrongestAttackEnemyCardInHand();
        PlayingCard strongestEnemyDefenseCardInHand = gameEvaluation.FindStrongestDefenseEnemyCardInHand();

        chosenTile = gameEvaluation.findEmptyEnemyTile();
        if (chosenTile == null)
        {
            replacingCard = true;
            chosenTile = gameEvaluation.FindTileWithWeakestEnemyCard();
        }

        if (strongestPlayerCard == null)
        {
            StartCoroutine(placeCard(strongestEnemyCardInHand));
            Debug.Log("placed card: strongest attack: " + strongestEnemyCardInHand.attack + "defense: " + strongestEnemyCardInHand.defense);
        }

        if (strongestPlayerCard != null)
        {
            if (strongestEnemyCardInHand.attack > strongestPlayerCard.attack || strongestEnemyCardInHand.attack == strongestPlayerCard.attack)
            {
                StartCoroutine(placeCard(strongestEnemyCardInHand));
                Debug.Log("placed card: strongest attack: " + strongestEnemyCardInHand.attack + "defense: " + strongestEnemyCardInHand.defense);
            }
            else
            {
                StartCoroutine(placeCard(strongestEnemyDefenseCardInHand));
                Debug.Log("placed card: strongest defense: " + strongestEnemyDefenseCardInHand.attack + "defense: " + strongestEnemyDefenseCardInHand.defense);
            }
        }
        yield return new WaitForSeconds(4f);


        foreach (FieldCard card in gameEvaluation.enemyFieldCards)
        {
            StartCoroutine(performCardAction(card));
            if (lastActionWasAttack)
            {
                yield return new WaitForSeconds(6);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }

        foreach(FieldCard card in gameEvaluation.enemyFieldCards)
        {
            if (!card.attackedThisTurn)
            {
                StartCoroutine(performCardAction(card));
            }
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FindObjectOfType<CameraMovement>().nextTurn());
    }

    //SHOULD PROBABLY DO THIS, I THINK ITS INEVTIABLE: REWORK DECISION MAKING SO THE AI EXAMINES EVERY CARD ON FIELD FOR EVERY ONE OF HIS CARD, AND CHECKS FOR STRONGEST ATTACK, STRONGEST DEFENSE, SAME ATTACK ETC ETC
    //BECAUSE WITH THIS IMPLEMENTATION, IT CAN ONLY CHECK FOR STRONGEST OR WEAKEST CARD. NOTHING IN BETWEEN IF THERE IS. THIS IS BAD.
    public IEnumerator performCardAction(FieldCard card)
    {
        FieldCard strongestPlayerCard = gameEvaluation.findStrongestPlayerCard();
        FieldCard strongestPlayerCardAttackMode = gameEvaluation.findStrongestPlayerCardOnlyAttackMode();

        if (!card.attackedThisTurn)
        {


            if (gameEvaluation.playerFieldCards.Count > 0)
            {
                if (strongestPlayerCardAttackMode)
                {
                    if (card.attack > strongestPlayerCardAttackMode.attack && !card.attackedThisTurn)
                    {
                        lastActionWasAttack = true;
                        if (card.inDefenseMode)
                        {
                            card.inDefenseMode = false;
                            yield return new WaitForSeconds(1);
                        }
                        card.declaringAttack = true;
                        strongestPlayerCardAttackMode.targeted = true;

                        yield return new WaitForSeconds(3);
                        card.attackedThisTurn = true;
                    }
                }
                if (card.attack > strongestPlayerCard.defense && !card.attackedThisTurn && strongestPlayerCard.inDefenseMode)
                {
                    lastActionWasAttack = true;
                    if (card.inDefenseMode)
                    {
                        card.inDefenseMode = false;
                        yield return new WaitForSeconds(1);
                    }
                    card.declaringAttack = true;
                    gameEvaluation.findStrongestPlayerCard().targeted = true;

                    yield return new WaitForSeconds(3);
                    card.attackedThisTurn = true;
                }

                if (gameEvaluation.findPlayerCardWithLowerAttack(card) && !card.attackedThisTurn)
                {
                    if (gameEvaluation.findPlayerCardWithLowerAttack(card).inDefenseMode && card.attack > gameEvaluation.findPlayerCardWithLowerAttack(card).defense && !card.attackedThisTurn)
                    {
                        lastActionWasAttack = true;
                        if (card.inDefenseMode)
                        {
                            card.inDefenseMode = false;
                            yield return new WaitForSeconds(1);
                        }
                        card.declaringAttack = true;
                        gameEvaluation.findPlayerCardWithLowerAttack(card).targeted = true;

                        yield return new WaitForSeconds(3);
                        card.attackedThisTurn = true;
                    }
                    if (gameEvaluation.findPlayerCardWithLowerAttack(card).inDefenseMode == false && card.attack > gameEvaluation.findPlayerCardWithLowerAttack(card).attack && !card.attackedThisTurn)
                    {
                        lastActionWasAttack = true;
                        if (card.inDefenseMode)
                        {
                            card.inDefenseMode = false;
                            yield return new WaitForSeconds(1);
                        }
                        card.declaringAttack = true;
                        gameEvaluation.findPlayerCardWithLowerAttack(card).targeted = true;

                        yield return new WaitForSeconds(3);
                        card.attackedThisTurn = true;
                    }
                }
                if (gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card)){
                    if (gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).inDefenseMode && card.attack > gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).defense && !card.attackedThisTurn)
                    {
                        //! CHECK FOR ATTACK & DEFENSE MODE
                        lastActionWasAttack = true;
                        if (card.inDefenseMode)
                        {
                            card.inDefenseMode = false;
                            yield return new WaitForSeconds(1);
                        }
                        card.declaringAttack = true;
                        gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).targeted = true;

                        yield return new WaitForSeconds(3);
                        card.attackedThisTurn = true;
                    }
                }
                if (gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card))
                {
                    if (gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).inDefenseMode == false && card.attack > gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).attack && !card.attackedThisTurn)
                    {
                        //! CHECK FOR ATTACK & DEFENSE MODE
                        lastActionWasAttack = true;
                        if (card.inDefenseMode)
                        {
                            card.inDefenseMode = false;
                            yield return new WaitForSeconds(1);
                        }
                        card.declaringAttack = true;
                        gameEvaluation.findPlayerCardWithLowerDefenseThanEnemyAttack(card).targeted = true;

                        yield return new WaitForSeconds(3);
                        card.attackedThisTurn = true;
                    }
                }
                if (gameEvaluation.findWeakestPlayerCard(true))
                {
                    if (gameEvaluation.findWeakestPlayerCard(true).inDefenseMode == false && card.attack == gameEvaluation.findWeakestPlayerCard(true).attack && !card.attackedThisTurn)
                    {
                        float random = Random.Range(0, 1);
                        if (random <= 0.5f)
                        {
                            card.inDefenseMode = true;
                            //card.attackedThisTurn = true;
                            lastActionWasAttack = false;
                            yield return new WaitForSeconds(0.5f);
                        }
                        else
                        {
                            lastActionWasAttack = true;
                            if (card.inDefenseMode)
                            {
                                card.inDefenseMode = false;
                                yield return new WaitForSeconds(1);
                            }
                            card.declaringAttack = true;
                            gameEvaluation.findWeakestPlayerCard(true).targeted = true;

                            yield return new WaitForSeconds(3);
                            card.attackedThisTurn = true;
                        }
                    }
                }
                if (gameEvaluation.findWeakestPlayerCard(true))
                {
                    if (gameEvaluation.findWeakestPlayerCard(true).inDefenseMode && card.attack == gameEvaluation.findWeakestPlayerCard(true).defense && !card.attackedThisTurn)
                    {
                        card.inDefenseMode = true;
                        //card.attackedThisTurn = true;
                        lastActionWasAttack = false;
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                if (gameEvaluation.findWeakestPlayerCard(true))
                {
                    if (card.attack < gameEvaluation.findWeakestPlayerCard(true).attack && !card.attackedThisTurn)
                    {
                        card.inDefenseMode = true;
                        //card.attackedThisTurn = true;
                        lastActionWasAttack = false;
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                if (strongestPlayerCardAttackMode)
                {
                    if (card.attack == strongestPlayerCardAttackMode.attack && !card.attackedThisTurn) //should be ok, cuz if there would be more cards an attack shouldve happened earlier
                    {
                        float random = Random.Range(0, 1);
                        if (random <= 0.5f)
                        {
                            card.inDefenseMode = true;
                            //card.attackedThisTurn = true;
                            lastActionWasAttack = false;
                            yield return new WaitForSeconds(0.5f);
                        }
                        else
                        {
                            lastActionWasAttack = true;
                            if (card.inDefenseMode)
                            {
                                card.inDefenseMode = false;
                                yield return new WaitForSeconds(1);
                            }
                            card.declaringAttack = true;
                            strongestPlayerCardAttackMode.targeted = true;

                            yield return new WaitForSeconds(3);
                            card.attackedThisTurn = true;
                        }
                    }
                }
            }
            else
            {
                lastActionWasAttack = true;
                if (card.inDefenseMode)
                {
                    card.inDefenseMode = false;
                    yield return new WaitForSeconds(1);
                }
                //Debug.Log("TODO: ATTACK LP");
                card.declaringAttack = true;
                lastActionWasAttack = true;
                if (!card.attackedThisTurn)
                {
                    card.attackedThisTurn = true;
                    StartCoroutine(gameEvaluation.enemyAttackEnemyLP());
                }
                yield return new WaitForSeconds(3f);
                
                //attack lifepoints
            }
        }
    
    }
    public IEnumerator placeCard(PlayingCard cardToPlace)
    {
        cardToPlace.selected = true;
        cardToPlace.faceDown = true;
        currentAction = ACTION.CONFIRMING;
        yield return new WaitForSeconds(1f);
        UnrenderCards();
        currentAction = ACTION.PLACINGCARD;
        if (replacingCard)
        {
            gameEvaluation.enemyFieldCards.Remove(chosenTile.fieldCardOnTile);
            Destroy(chosenTile.fieldCardOnTile.gameObject);
            chosenTile.fieldCardOnTile = null;
        }
        yield return new WaitForSeconds(1);
        chosenTile.cardOnTile = cardToPlace;
        chosenTile.enemyDropCard();
        hand[0].transform.position = handGO.slot1.position;
        hand[1].transform.position = handGO.slot2.position;
        hand[2].transform.position = handGO.slot3.position;
        hand[3].transform.position = handGO.slot4.position;
        hand[4].transform.position = handGO.slot5.position;
        currentAction = ACTION.BOARDVIEW;
        hand.Remove(cardToPlace);
    }

    public IEnumerator drawCards()
    {
        while (hand.Count < 5 && deck.Count > 0)
        {
            var playingCard = Instantiate(cardPrefab, handGO.spawnSpot.position, Quaternion.identity);
            playingCard.SetUI(deck[0]);
            playingCard.SetStats(deck[0]);
            hand.Add(playingCard);
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
            child.gameObject.layer = layer;
        }
    }
}

//RANDOMNESS

//if(strongestPlayerCard == null)
//{
//    float randomNumber = Random.Range(0f, 1f);

//    if (randomNumber < 0.5f)
//    {
//        StartCoroutine(placeCard(strongestEnemyCardInHand));
//        Debug.Log("randomly placed card: strongest attack: " + strongestEnemyCardInHand.attack + "defense: " + strongestEnemyCardInHand.defense);
//    }
//    else
//    {
//        StartCoroutine(placeCard(strongestEnemyDefenseCardInHand));
//        Debug.Log("randomly placed card: strongest defense: " + strongestEnemyDefenseCardInHand.attack + "defense: " + strongestEnemyDefenseCardInHand.defense);
//    }
//}