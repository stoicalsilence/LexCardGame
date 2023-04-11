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
    public enum TYPE { STONE, THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT, WINGEDBEAST, SPELLCASTER }
    public TYPE type;
    public TextMeshProUGUI UI_id;
    public TextMeshProUGUI UI_cardName;
    public TextMeshProUGUI UI_atkText;
    public TextMeshProUGUI UI_defText;
    public TextMeshProUGUI UI_type;
    public TextMeshProUGUI UI_starsign;
    public Material cardArt;
    public Image image;
    public Image iconImage;
    public IconGiver iconGiver;
    void Start()
    {
        iconGiver = FindObjectOfType<IconGiver>();
        iconImage.material = iconGiver.icons[type.ToString()];
    }

    // Update is called once per frame
    void Update()
    {
        
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
        UI_atkText.text = attack.ToString();
        UI_defText.text = defense.ToString();
        UI_cardName.text = cardName;
        UI_id.text = id.ToString();
        UI_type.text = type.ToString();
        image.material = cardArt;
        
    }
}
