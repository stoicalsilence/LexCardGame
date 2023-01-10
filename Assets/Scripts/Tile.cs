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
    public Transform spawnPoint;
    public Transform dropPoint;

    public bool hasCard;
    public PlayingCard cardOnTile;
    public FieldCard fieldCard;
    public bool droppingCard;

    FieldCard cardToSpawn;

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
    }

    // Update is called once per frame
    void Update()
    {

        if (hasCard)
        {
            if (cardToSpawn.faceUp)
            {
                if (cardToSpawn.inDefenseMode == true)
                {
                    Quaternion target1 = Quaternion.Euler(0, 0, -90);
                    cardToSpawn.transform.rotation = Quaternion.Slerp(cardToSpawn.transform.rotation, target1, Time.deltaTime * 10);
                    //cardToSpawn.transform.rotation = Quaternion.Euler(0, 0, -90);   works but no anim
                    
                }
                else
                {
                    Quaternion target2 = Quaternion.Euler(0, -90, -90);
                    cardToSpawn.transform.rotation = Quaternion.Slerp(cardToSpawn.transform.rotation, target2, Time.deltaTime * 10);
                   //cardToSpawn.transform.rotation = Quaternion.Euler(0, -90, -90);   works but no anim
                }
            }
            else //if not faceup
            {
                if (cardToSpawn.inDefenseMode)
                {
                    Quaternion target_ = Quaternion.Euler(180, 180, 270);
                    cardToSpawn.transform.rotation = Quaternion.Slerp(cardToSpawn.transform.rotation, target_, Time.deltaTime * 10);
                    
                    //cardToSpawn.transform.rotation = Quaternion.Euler(180, 180, 270);
                    
                }
                else 
                {
                    
                    Quaternion _target = Quaternion.Euler(0, 90, 90);
                    cardToSpawn.transform.rotation = Quaternion.Slerp(cardToSpawn.transform.rotation, _target, Time.deltaTime * 10);
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
            if (hasCard)
            {
                cardOnTile.showUIDetails();
            }


            if(hasCard && Input.GetKeyDown(KeyCode.R))
            {
                if (cardToSpawn.inDefenseMode)
                {
                    cardToSpawn.inDefenseMode = false;
                }
                else
                {
                    cardToSpawn.inDefenseMode = true;
                }
            }

            if (hasCard && Input.GetKeyDown(KeyCode.Q))
            {
                if (cardToSpawn.faceUp)
                {
                    cardToSpawn.faceUp = false;
                }
                else
                {
                    cardToSpawn.faceUp = true;
                }
            }
        
        if(isHighlighted && player.currentAction == Player.ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.Mouse0))
        {
            cardOnTile = player.cardToPlace;
            cardToSpawn = Instantiate(fieldCard, dropPoint.position, Quaternion.identity);
            
            cardToSpawn.initialise(cardOnTile.cardName, cardOnTile.attack, cardOnTile.defense, cardOnTile.description, cardOnTile.faceUp);
            Quaternion target;
            player.SetLayerAllChildren(cardToSpawn.transform, LayerMask.NameToLayer("Default"));
            cardToSpawn.tile = this;
            if (cardOnTile.faceUp == true)
            {
                target = Quaternion.Euler(0, -90, -90);
            }
            else
            {
                 target = Quaternion.Euler(0, 90, 90);
            }
            cardToSpawn.transform.rotation = target;
            StartCoroutine(dropAnimation());
            player.hand.Remove(player.cardToPlace);
            Destroy(player.UICardToDelete.gameObject);
            hasCard = true;
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
            dropCard(cardToSpawn);
        }
    }
    private void OnMouseEnter()
    {
        if (player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD)
        {
            isHighlighted = true;
        }
    }
    private void OnMouseExit()
    {
        isHighlighted = false;
        if (hasCard)
        {
            cardOnTile.hideUIDetails();
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
    void dropCard(FieldCard cardToDrop)
    {
        cardToDrop.transform.position = Vector3.Slerp(cardToDrop.transform.position, spawnPoint.position, Time.deltaTime * 5);
    }

   
}
