using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    // Start is called before the first frame update

    public int id;
    public string cardName;
    public int attack;
    public int defense;
    public string description;
    public enum TYPE { THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT, WINGEDBEAST, SPELLCASTER, FIEND }
    public TYPE type;
    public TextMeshProUGUI UI_cardName;
    public TextMeshProUGUI UI_atkText;
    public TextMeshProUGUI UI_defText;
    public TextMeshProUGUI UI_type;
    public TextMeshProUGUI UI_starsign;
    public TextMeshProUGUI UI_description;

    public TextMeshProUGUI idText;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    
    public Material cardArt;
    public Image image;
    public Image iconImage;
    public IconGiver iconGiver;

    public bool isHighlighted;
    public bool removeFromDeckButtonHighlighted;
    public bool selected;
    public Color32 selectedColor = new Color32(171, 163, 110, 255);
    public Color32 normalColor = new Color32(255, 255, 170, 255);
    void Start()
    {
        iconGiver = FindObjectOfType<IconGiver>();
        iconImage.material = iconGiver.icons[type.ToString()];
        SetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isHighlighted)
            {
                selected = true;
            }
            else if (!removeFromDeckButtonHighlighted)
            {
                selected = false;
            }
        }

        if (selected)
        {
            this.gameObject.GetComponent<Image>().color = selectedColor;
            showUIDetails();
            if(transform.parent.name == "PlayerCards")
            {
                FindObjectOfType<ButtonSpawner>().removeCardFromDeckButton.SetActive(true);
                FindObjectOfType<ButtonSpawner>().addCardToDeckButton.SetActive(false);
            }
            else
            {
                FindObjectOfType<ButtonSpawner>().removeCardFromDeckButton.SetActive(false);
                FindObjectOfType<ButtonSpawner>().addCardToDeckButton.SetActive(true);
            }
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = normalColor;
        }
        
    }

    public void SetStats(Card card)
    {
        id = card.id;
        cardName = card.cardName;
        attack = card.attack;
        defense = card.defense;
        type = (TYPE)card.type;
        description = card.description;
        cardArt = card.cardArt;
    }
    public void SetTexts()
    {
        atkText.text = attack.ToString();
        defText.text = defense.ToString();
        cardNameText.text = cardName;
        idText.text = id.ToString();
        image.material = cardArt;   
    }

    public void SetReferences()
    {
        UI_cardName = GameObject.FindWithTag("UI_CardName").GetComponent<TextMeshProUGUI>();
        UI_atkText = GameObject.FindWithTag("UI_Atk").GetComponent<TextMeshProUGUI>();
        UI_defText = GameObject.FindWithTag("UI_Def").GetComponent<TextMeshProUGUI>();
        UI_type = GameObject.FindWithTag("UI_Type").GetComponent<TextMeshProUGUI>();
        //UI_starsign = GameObject.FindWithTag("UI_Starsign").GetComponent<TextMeshProUGUI>();
        UI_description = GameObject.FindWithTag("UI_Description").GetComponent<TextMeshProUGUI>();
    }

    public void showUIDetails()
    {
        UI_cardName.text = cardName;
        UI_atkText.text = "Atk: " + attack.ToString();
        UI_defText.text = "Def: " + defense.ToString();
        UI_type.text = type.ToString();
        UI_description.text = description;
        UI_type.text = type.ToString();
    }
    public void hideUIDetails()
    {
        UI_cardName.text = "";
        UI_atkText.text = "";
        UI_defText.text = "";
        UI_type.text = "";
        //UI_starsign.text = "";
        UI_description.text = "";
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
