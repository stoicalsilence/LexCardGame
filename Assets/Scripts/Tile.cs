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

   // public Card cardOnTop;
    void Start()
    {
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
