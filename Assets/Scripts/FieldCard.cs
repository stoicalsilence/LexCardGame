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

    public Player player;

    public bool faceUp;
    public bool inDefenseMode;
    // Start is called before the first frame update
    void Start()
    {
        SetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        atkText.text = attack.ToString();
        defText.text = defense.ToString();
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

    public void initialise(string cardName, int attack, int defense, string description, bool faceUp)
    {
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.description = description;
        this.faceUp = faceUp;
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

   
}
