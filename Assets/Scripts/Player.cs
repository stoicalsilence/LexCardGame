using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string duelistName; //player fill this in at start
    public PlayerHand handGO;
    public List<PlayingCard> hand;

    public Camera canvasCamera;

    public CardDataBase cardDataBase;

    public PlayerDeck deck;

    public PlayingCard cardPrefab;

    public PlayingCard cardToPlace;

    public PlayingCard UICardToDelete;
    public enum ACTION { CHOOSING, CONFIRMING, BOARDVIEW, PLACINGCARD }
    public ACTION currentAction;

    public bool playedCard;
    public int lifepoints;
    public TextMeshProUGUI lifepointText;

    public List<PlayingCard> fusionList;
    public List<PlayingCard> fusionList2;


    public bool drawingCards;
    bool bool1;
    bool bool2;
    bool bool3;
    bool bool4;
    bool bool5;
    public bool placingFusedCard;

    public FusionTable fusionTable;
    public PlayingCard dummyPrefab;

    public GameEvaluation gameEvaluation;

    //chose card to play? track chosen card place it down, player has hand, chosen card gets removed from hand and added to gamegrid
    //deck
    //cards get moved from deck and into hand

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        lifepoints = 8000;
        deck.fillDeck();
        deck.calculateAverageDamage();
        deck.calculateAverageDefense();
        FindObjectOfType<DeckManager>().saveDeck();
        StartCoroutine(drawCards());
        foreach(Card card in FindObjectOfType<DeckManager>().loadDeck())
        {
            Debug.Log(card.cardName);
        }
        fusionTable = FindObjectOfType<FusionTable>();
        gameEvaluation = FindObjectOfType<GameEvaluation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene("DeckBuilder");
        }
        if(deck.cardsInDeck.Count == 0)
        {
            Debug.Log("Lost by attrition!");
        }
        lifepointText.text = lifepoints.ToString();
        if (!playedCard)
        {

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

            if (currentAction == ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.S) && !placingFusedCard)
            {
                currentAction = ACTION.CHOOSING;
                cardToPlace.selected = false;
                cardToPlace.faceDown = false;
                cardToPlace.transform.position = cardToPlace.originalPos;
                ResetLayers();
            }
        }

        if (bool1)
        {
            if(hand[0])
            hand[0].transform.position = Vector3.Lerp(hand[0].transform.position, handGO.slot1.position, Time.deltaTime * 17);
        }
        if (bool2)
        {
            if (hand[1])
                hand[1].transform.position = Vector3.Lerp(hand[1].transform.position, handGO.slot2.position, Time.deltaTime * 17);
        }
        if (bool3)
        {
            if (hand[2])
                hand[2].transform.position = Vector3.Lerp(hand[2].transform.position, handGO.slot3.position, Time.deltaTime * 17);
        }
        if (bool4)
        {
            if (hand[3])
                hand[3].transform.position = Vector3.Lerp(hand[3].transform.position, handGO.slot4.position, Time.deltaTime * 17);
        }
        if (bool5)
        {
            if (hand[4])
                hand[4].transform.position = Vector3.Lerp(hand[4].transform.position, handGO.slot5.position, Time.deltaTime * 17);
        }

    }

    public IEnumerator drawCards()
    {
        drawingCards = true;
        while (hand.Count < 5 && deck.cardsInDeck.Count > 0)
        {
            var playingCard = Instantiate(cardPrefab, handGO.spawnSpot.position, Quaternion.identity);
            playingCard.SetUI(deck.cardsInDeck[0]);
            playingCard.SetStats(deck.cardsInDeck[0]);
            hand.Add(playingCard);
            deck.cardsInDeck.RemoveAt(0);
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
        //if (hand[0])
        //{
        //    hand[0].transform.position = handGO.slot1.position;
        //}
        //if (hand[1])
        //{
        //    hand[1].transform.position = handGO.slot2.position;
        //}
        //if (hand[2])
        //{
        //    hand[2].transform.position = handGO.slot3.position;
        //}
        //if (hand[3])
        //{
        //    hand[3].transform.position = handGO.slot4.position;
        //}
        //if (hand[4])
        //{
        //    hand[4].transform.position = handGO.slot5.position;
        //}
        Destroy(playingCard);
    }


    public void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
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
        if (cardToPlace != null)
        {
            SetLayerAllChildren(cardToPlace.transform, LayerMask.NameToLayer("Default"));
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

    public void initiateFusion()
    {
        PlayingCard fusedCard = Instantiate(dummyPrefab);

        if (fusionTable.returnFusion(fusionList[0], fusionList[1]))
        {
            Debug.Log("fusiontable did not return null, compared:" + fusionList[0].cardName + " with: " + fusionList[1].cardName);
            fusedCard.SetStats(fusionTable.returnFusion(fusionList[0], fusionList[1]));
        }
        else
        {
            Debug.Log("fusiontable returned null, compared:" + fusionList[0].cardName + " with: " + fusionList[1].cardName);
            fusedCard.SetStatsFromPlayingCard(fusionList[1]);
        }
        
        Destroy(fusionList[1].gameObject);
        Destroy(fusionList[0].gameObject);

        hand.Remove(fusionList[1]);
        hand.Remove(fusionList[0]);

        fusionList.RemoveAt(1);
        fusionList.RemoveAt(0);
        
        Debug.Log("Count: +" + fusionList.Count + " fused card name: " + fusedCard.cardName);
        
        fusionList.Insert(0, fusedCard);
        
        if (fusionList.Count > 1)
        {
            
            initiateFusion();
        }
        else
        {
            placingFusedCard = true;
            prepareToPlace(fusedCard);
        }
    }

    public void declareAttack()
    {
        FieldCard attackingPlayerCard = gameEvaluation.playerFieldCards.Find(x => x.declaringAttack == true);
        Debug.Log(attackingPlayerCard.cardName);

        //TODO: WHEN PLAYER DECLARES A CARD TO ATTACK, HIGHLIGHT ALL EnemyField TO SHOW THAT HE'S ATTACKING
        //: ONLY WHEN PLAYER HAS FIRST CLICKED ON ONE OF HIS CARDS if playedcard == true, ALLOW ABOVE
        //: ALLOW CANCELLATION OF ATTACK
    }
}

