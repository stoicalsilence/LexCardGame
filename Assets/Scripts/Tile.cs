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

    public PlayingCard cardOnTile;
    public FieldCard fieldCard;

   // public Card cardOnTop;
    void Start()
    {
        GameObject spawnLoc = new GameObject();
        spawnLoc.transform.position = this.transform.position;
        spawnLoc.transform.position += new Vector3(0, 0.6f, 0);
        spawnLoc.name = "TileSpawnPos";
        spawnPoint = spawnLoc.transform;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHighlighted)
        {
            this.gameObject.GetComponent<Renderer>().material = baseMaterial;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = highlightMaterial;
        }
        if(isHighlighted && player.currentAction == Player.ACTION.PLACINGCARD && Input.GetKeyDown(KeyCode.Mouse0))
        {
            cardOnTile = player.cardToPlace;
            FieldCard cardToSpawn = Instantiate(fieldCard, spawnPoint.position, Quaternion.identity);
            cardToSpawn.initialise(cardOnTile.cardName, cardOnTile.attack, cardOnTile.defense, cardOnTile.description, cardOnTile.faceUp);
            Quaternion target;
            player.SetLayerAllChildren(cardToSpawn.transform, LayerMask.NameToLayer("Default"));
            if (cardOnTile.faceUp == true)
            {
                target = Quaternion.Euler(0, -90, -90); // 0 -180 -90 for defense mode
            }
            else
            {
                 target = Quaternion.Euler(0, 90, 90);
            }
            cardToSpawn.transform.rotation = target;
            
            player.hand.Remove(player.cardToPlace);
            Destroy(player.UICardToDelete.gameObject);
            player.currentAction = Player.ACTION.BOARDVIEW;
        }
        
        
        if (isDarkTile)
        {
            baseMaterial = darkMaterial;
        }
        else
        {
            baseMaterial = lightMaterial;
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
    }
}
