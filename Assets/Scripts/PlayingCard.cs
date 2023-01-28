using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayingCard : MonoBehaviour
{
    public bool isHighlighted;
    public bool gettingFusioned;
    public TextMeshProUGUI UI_cardName;
    public TextMeshProUGUI UI_atkText;
    public TextMeshProUGUI UI_defText;
    public TextMeshProUGUI UI_type;
    public TextMeshProUGUI UI_starsign;
    public GameObject UI_selector;
    public TextMeshProUGUI fusionCounter;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public int searchItemId;

    public string cardName;
    public int attack;
    public int defense;
    public string description;
    public enum TYPE { STONE, THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT }
    public TYPE type;

    public Vector3 originalPos;
    public Transform selectPos;
    public Vector3 fusionPos;
    public bool selected;
    public bool faceDown;
    bool initialselect;
    public bool isbeingplaced;
    public CameraMovement cameraMovement;
    public Player player;

    
    private void Start()
    {
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        SetReferences();
        faceDown = false;
    }
    public void getNewRandomCard()
    {
        SetUI(Database.GetRandomCard());
    }

    public void getCardById()
    {
        SetUI(Database.GetCardById(searchItemId));
    }

    private void Update()
    {
        atkText.text =  attack.ToString();
        defText.text =  defense.ToString();

        if (gettingFusioned)
        {
            fusionCounter.gameObject.SetActive(true);
            fusionCounter.text = (player.fusionList.IndexOf(this)+1).ToString();
        }
        else
        {
            fusionCounter.gameObject.SetActive(false);
        }


        if (cameraMovement.turnState == CameraMovement.STATE.PLAYERTURN && isHighlighted)
        {
            showUIDetails();
        }
        
        if (faceDown)
        {
            Quaternion target = Quaternion.Euler(0, 90, 0);
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
        }
        else
        {
            Quaternion target = Quaternion.Euler(0, -90, 0);
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5);
        }
        if(cameraMovement.turnState == CameraMovement.STATE.ENEMYTURN)
        {
            if (selected)
            {
                if (!initialselect)
                {
                    originalPos = transform.position;
                    initialselect = true;
                    UI_selector.gameObject.SetActive(true);
                }
                
                transform.position = Vector3.Slerp(transform.position, selectPos.position, Time.deltaTime * 5);
            }
            else
            {
                UI_selector.gameObject.SetActive(false);
            }
        }



        if (cameraMovement.turnState == CameraMovement.STATE.PLAYERTURN && !player.playedCard)
        {
            if (selected)
            {
                if (!initialselect)
                {
                    originalPos = transform.position;
                    initialselect = true;
                    faceDown = true;
                    UI_selector.gameObject.SetActive(true);
                    player.currentAction = Player.ACTION.CONFIRMING;
                    //UI_selector.gameObject.GetComponent<Selector>().StartCoroutine("wee()");
                }
                
                transform.position = Vector3.Slerp(transform.position, selectPos.position, Time.deltaTime * 5);
                if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && player.currentAction == Player.ACTION.CONFIRMING)
                {
                    if (faceDown)
                    {
                        faceDown = false;
                    }
                    else
                    {
                        faceDown = true;
                    }
                }
                if (isHighlighted && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    player.prepareToPlace(this);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    UI_selector.gameObject.SetActive(false);
                    player.currentAction = Player.ACTION.CHOOSING;
                    transform.position = originalPos;
                    initialselect = false;
                    faceDown = false;
                    selected = false;
                }
            }
            else
            {
                UI_selector.gameObject.SetActive(false);
            }
        }

        if (isHighlighted && Input.GetKeyDown(KeyCode.Mouse0) && player.currentAction == Player.ACTION.CHOOSING)
        {
            selected = true;
        }

        if(isHighlighted && Input.GetKeyDown(KeyCode.Mouse1) && player.currentAction == Player.ACTION.CHOOSING && cameraMovement.turnState == CameraMovement.STATE.PLAYERTURN && !selected)
        {
            if (gettingFusioned)
            {
                gettingFusioned = false;
                player.fusionList.Remove(this);
            }
            else
            {
                //if select a card, all cards go unfusioned
                gettingFusioned = true;
                player.fusionList.Add(this);
            }
        }
    }

    public void SetUI(Card card)
    {
        if(card == null)
        {
            atkText.text = "ERROR";
            defText.text = "ERROR";
        }
        atkText.text = card.attack.ToString();
        defText.text = card.defense.ToString();
    }
    public void SetReferences()
    {
        UI_cardName = GameObject.FindWithTag("UI_CardName").GetComponent<TextMeshProUGUI>();
        UI_atkText = GameObject.FindWithTag("UI_Atk").GetComponent<TextMeshProUGUI>();
        UI_defText = GameObject.FindWithTag("UI_Def").GetComponent<TextMeshProUGUI>();
        UI_type = GameObject.FindWithTag("UI_Type").GetComponent<TextMeshProUGUI>();
        UI_starsign = GameObject.FindWithTag("UI_Starsign").GetComponent<TextMeshProUGUI>();
        selectPos = GameObject.FindWithTag("SelectPos").GetComponent<Transform>();
        player = FindObjectOfType<Player>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }
    public void SetStats(Card card)
    {
        cardName = card.cardName;
        attack = card.attack;
        defense = card.defense;
        type = (TYPE)card.type;
        description = card.description;
    }
    public void showUIDetails()
    {
        UI_cardName.text = cardName;
        UI_atkText.text = "Atk: " + attack.ToString();
        UI_defText.text = "Def: " + defense.ToString();
        UI_type.text = type.ToString();
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
        isHighlighted = true;
    }
    
    private void OnMouseExit()
    {
        isHighlighted = false;
    }
}
