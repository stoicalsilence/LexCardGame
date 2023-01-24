using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayingCard : MonoBehaviour
{
    public bool isHighlighted;
    public TextMeshProUGUI UI_cardName;
    public TextMeshProUGUI UI_atkText;
    public TextMeshProUGUI UI_defText;
    public TextMeshProUGUI UI_type;
    public TextMeshProUGUI UI_starsign;
    public GameObject UI_selector;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public int searchItemId;

    public string cardName;
    public int attack;
    public int defense;
    public string description;

    public Vector3 originalPos;
    public Transform selectPos;
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

        if (isHighlighted)
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

        if (!player.playedCard || cameraMovement.turnState == CameraMovement.STATE.ENEMYTURN)
        {
            if (selected)
            {
                if (!initialselect)
                {
                    originalPos = transform.position;
                    initialselect = true;
                    UI_selector.gameObject.SetActive(true);
                    //UI_selector.gameObject.GetComponent<Selector>().StartCoroutine("wee()");
                }
                if (player.currentAction != Player.ACTION.PLACINGCARD)
                {
                    player.currentAction = Player.ACTION.CONFIRMING;
                }
                transform.position = Vector3.Slerp(transform.position, selectPos.position, Time.deltaTime * 5);
                //transform.position = selectPos.position;
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && player.currentAction == Player.ACTION.CONFIRMING)
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
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.S))
                {
                    player.currentAction = Player.ACTION.CHOOSING;
                    //transform.position = Vector3.Slerp(transform.position, originalPos, Time.deltaTime * 5);   <--- this doesnt work cuz it only does it for one frame. would have to coroutine it
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

        if(isHighlighted && Input.GetKeyDown(KeyCode.Mouse1) && player.currentAction == Player.ACTION.CHOOSING)
        {
            //fusion
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
        description = card.description;
    }
    public void showUIDetails()
    {
        UI_cardName.text = cardName;
        UI_atkText.text = "Atk: " + attack.ToString();
        UI_defText.text = "Def: " + defense.ToString();
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
