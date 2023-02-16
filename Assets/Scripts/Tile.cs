using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material baseMaterial;
    public Material lightMaterial;
    public Material darkMaterial;
    public Material highlightMaterial;
    public bool isHighlighted;
    public bool isDarkTile;
    public Player player;
    public Enemy enemy;
    public Transform spawnPoint;
    public Transform dropPoint;

    public bool hasCard;
    public PlayingCard cardOnTile;
    public FieldCard fieldCardOnTile;
    public FieldCard fieldCard;
    public bool droppingCard;

    public GameEvaluation gameEvaluation;

    // public Card cardOnTop;
    void Start()
    {
        GameObject spawnLoc = new GameObject();
        spawnLoc.transform.position = this.transform.position;
        spawnLoc.transform.position += new Vector3(0, 0.6f, 0);
        spawnLoc.name = "TileSpawnPos";
        spawnPoint = spawnLoc.transform;

        GameObject dropLoc = new GameObject();
        dropLoc.transform.position = this.transform.position;
        dropLoc.transform.position += new Vector3(0, 4.5f, 0);
        dropLoc.name = "TileDropPos";
        dropPoint = dropLoc.transform;
        player = FindObjectOfType<Player>();

        gameEvaluation = FindObjectOfType<GameEvaluation>();

        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCard)
        {
            if (!fieldCardOnTile.faceDown)
            {
                if (fieldCardOnTile.inDefenseMode == true)
                {
                    Quaternion target1 = Quaternion.Euler(0, 0, -90);
                    fieldCardOnTile.transform.rotation = Quaternion.Slerp(fieldCardOnTile.transform.rotation, target1, Time.deltaTime * 10);
                    //cardToSpawn.transform.rotation = Quaternion.Euler(0, 0, -90);   works but no anim
                    
                }
                else
                {
                    Quaternion target2 = Quaternion.Euler(0, -90, -90);
                    fieldCardOnTile.transform.rotation = Quaternion.Slerp(fieldCardOnTile.transform.rotation, target2, Time.deltaTime * 10);
                   //cardToSpawn.transform.rotation = Quaternion.Euler(0, -90, -90);   works but no anim
                }
            }
            else
            {
                if (fieldCardOnTile.inDefenseMode)
                {
                    Quaternion target_ = Quaternion.Euler(180, 0, 270);
                    fieldCardOnTile.transform.rotation = Quaternion.Slerp(fieldCardOnTile.transform.rotation, target_, Time.deltaTime * 10);
                }
                else 
                {
                    
                    Quaternion _target = Quaternion.Euler(0, 90, 90);
                    fieldCardOnTile.transform.rotation = Quaternion.Slerp(fieldCardOnTile.transform.rotation, _target, Time.deltaTime * 10);
                }
            }
        }

        if (!isHighlighted)
        {
            this.gameObject.GetComponent<Renderer>().material = baseMaterial;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = highlightMaterial;
        }


            if(hasCard && Input.GetKeyDown(KeyCode.R) &&isHighlighted)
            {
                if (fieldCardOnTile.inDefenseMode)
                {
                fieldCardOnTile.inDefenseMode = false;
                }
                else
                {
                fieldCardOnTile.inDefenseMode = true;
                }
            }

            if (hasCard && Input.GetKeyDown(KeyCode.Q))
            {
                //open card info
            }
        
        if(!hasCard &&isHighlighted && player.currentAction == Player.ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (this.gameObject.tag == "PlayerField")
            {
                player.placeCard(player.cardToPlace);
                cardOnTile = player.cardToPlace;
                fieldCardOnTile = Instantiate(fieldCard, dropPoint.position, Quaternion.identity);
                gameEvaluation.playerFieldCards.Add(fieldCardOnTile);
                fieldCardOnTile.initialise(cardOnTile.cardName, cardOnTile.attack, cardOnTile.defense, (FieldCard.TYPE)cardOnTile.type, cardOnTile.description, cardOnTile.faceDown, cardOnTile.cardArt);
                Quaternion target;
                player.SetLayerAllChildren(fieldCardOnTile.transform, LayerMask.NameToLayer("Default"));
                fieldCardOnTile.tile = this;
                if (cardOnTile.faceDown == false)
                {
                    target = Quaternion.Euler(0, -90, -90);
                }
                else
                {
                    target = Quaternion.Euler(0, 90, 90);
                }
                fieldCardOnTile.transform.rotation = target;
                StartCoroutine(dropAnimation());
                player.hand.Remove(player.cardToPlace);
                Destroy(player.UICardToDelete.gameObject);
                hasCard = true;
            }
            //player.currentAction = Player.ACTION.BOARDVIEW;
        }
        
        
        if (isDarkTile)
        {
            baseMaterial = darkMaterial;
        }
        else
        {
            baseMaterial = lightMaterial;
        }

        if (droppingCard)
        {
            dropCard(fieldCardOnTile);
        }
    }
    private void OnMouseEnter()
    {
        if (player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD)
        {
            isHighlighted = true;
            if ((hasCard && this.gameObject.tag == "PlayerField") || (hasCard && this.gameObject.tag =="EnemyField" && !fieldCardOnTile.faceDown))
            {
                fieldCardOnTile.showUIDetails();
            }

            if(hasCard && this.gameObject.tag == "EnemyField" && fieldCardOnTile.faceDown)
            {
                fieldCardOnTile.showFaceDownUIDetails();
            }
        }
        
    }
    private void OnMouseExit()
    {
        isHighlighted = false;
        if (hasCard)
        {
            fieldCardOnTile.hideUIDetails();
        }
        
    }
    IEnumerator dropAnimation()
    {
        droppingCard = true;
        player.currentAction = Player.ACTION.CHOOSING;
        player.playedCard = true;
        yield return new WaitForSeconds(1.5f);
        player.currentAction = Player.ACTION.BOARDVIEW;
        droppingCard = false;
    }

    IEnumerator enemyDropAnimation()
    {
        droppingCard = true;
        enemy.currentAction = Enemy.ACTION.CHOOSING;
        yield return new WaitForSeconds(1.5f);
        enemy.currentAction = Enemy.ACTION.BOARDVIEW;
        droppingCard = false;
    }
    void dropCard(FieldCard cardToDrop)
    {
        cardToDrop.transform.position = Vector3.Slerp(cardToDrop.transform.position, spawnPoint.position, Time.deltaTime * 5);
    }

    public void enemyDropCard()
    {
        fieldCardOnTile = Instantiate(fieldCard, dropPoint.position, Quaternion.identity);
        gameEvaluation.enemyFieldCards.Add(fieldCardOnTile);
        fieldCardOnTile.initialise(cardOnTile.cardName, cardOnTile.attack, cardOnTile.defense, (FieldCard.TYPE)cardOnTile.type, cardOnTile.description, cardOnTile.faceDown, cardOnTile.cardArt);
        Quaternion target;
        fieldCardOnTile.tile = this;
        if (cardOnTile.faceDown == false)
        {
            target = Quaternion.Euler(0, -90, -90);
        }
        else
        {
            target = Quaternion.Euler(0, 90, 90);
        }
        fieldCardOnTile.transform.rotation = target;
        StartCoroutine(enemyDropAnimation());
        hasCard = true;
    }

}
