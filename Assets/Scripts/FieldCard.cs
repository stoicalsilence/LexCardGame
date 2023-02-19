using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldCard : MonoBehaviour
{
    public TextMeshProUGUI UI_cardName;
    public TextMeshProUGUI UI_atkText;
    public TextMeshProUGUI UI_defText;
    public TextMeshProUGUI UI_type;
    public TextMeshProUGUI UI_starsign;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;

    public string cardName;
    public int attack;
    public int defense;
    public string description;
    public GameObject image;
    public Material cardArt;
    public enum TYPE { STONE, THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT, WINGEDBEAST }
    public TYPE type;

    public Player player;

    public bool faceDown;
    public bool inDefenseMode;
    public Tile tile;

    public bool declaringAttack;
    public bool targeted;
    public bool movementBlocked;

    public Transform particlePos;
    public GameObject strongParticles;
    public GameObject CardDeathParticles;
    public bool strongParticlesSpawned;
    GameObject strongy;
    // Start is called before the first frame update
    void Start()
    {
        SetReferences();
        image.GetComponent<Renderer>().material = cardArt;
    }

    // Update is called once per frame
    void Update()
    {
        atkText.text = attack.ToString();
        defText.text = defense.ToString();
        if(attack >= 2500)
        {
            if (!strongParticlesSpawned)
            {
                strongParticlesSpawned = true;
                strongy = Instantiate(strongParticles, particlePos.position, Quaternion.identity);
            }
        }
        
        
            if (strongParticlesSpawned)
            {
            strongy.transform.position = particlePos.position;
            }
        
    }

    public void SetReferences()
    {
        UI_cardName = GameObject.FindWithTag("UI_CardName").GetComponent<TextMeshProUGUI>();
        UI_atkText = GameObject.FindWithTag("UI_Atk").GetComponent<TextMeshProUGUI>();
        UI_defText = GameObject.FindWithTag("UI_Def").GetComponent<TextMeshProUGUI>();
        UI_type = GameObject.FindWithTag("UI_Type").GetComponent<TextMeshProUGUI>();
        UI_starsign = GameObject.FindWithTag("UI_Starsign").GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    public void initialise(string cardName, int attack, int defense, TYPE type, string description, bool faceDown, Material cardArt)
    {
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.type = type;
        this.description = description;
        this.faceDown = faceDown;
        this.cardArt = cardArt;
    }

    public void showUIDetails()
    {
        UI_cardName.text = cardName;
        UI_atkText.text = "Atk: " + attack.ToString();
        UI_defText.text = "Def: " + defense.ToString();
        UI_type.text = type.ToString();
        //UI_starsign.text = "";
    }

    public void showFaceDownUIDetails()
    {
        UI_cardName.text = "";
        UI_atkText.text = "";
        UI_defText.text = "";
        UI_type.text = "";
        UI_starsign.text = "FaceDown!";
    }
    public void hideUIDetails()
    {
        UI_cardName.text = "";
        UI_atkText.text = "";
        UI_defText.text = "";
        UI_type.text = "";
        UI_starsign.text = "";
    }
    private void OnMouseEnter()
    {
        if (player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD)
        {
            tile.isHighlighted = true;
            if (tile.gameObject.tag == "PlayerField")
            {
                tile.fieldCardOnTile.showUIDetails();
            }

            if(tile.gameObject.tag == "EnemyField" && !tile.fieldCardOnTile.faceDown)
            {
                tile.fieldCardOnTile.showUIDetails();
            }

            if (tile.gameObject.tag == "EnemyField" && tile.fieldCardOnTile.faceDown)
            {
                tile.fieldCardOnTile.showFaceDownUIDetails();
            }
        }

    }
    private void OnMouseExit()
    {
        tile.isHighlighted = false;
        tile.fieldCardOnTile.hideUIDetails();
    }


}
