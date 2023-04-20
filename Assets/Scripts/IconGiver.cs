using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconGiver : MonoBehaviour
{
    public enum TYPE { THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT, WINGEDBEAST, SPELLCASTER }
    public TYPE type;
    public Material warriorIcon;
    public Material spellcasterIcon;
    public Material stoneIcon;
    public Material plantIcon;
    public Material zombieIcon;
    public Material wingedbeastIcon;
    public Material dragonIcon;
    public Material fireIcon;
    public Material machineIcon;
    public Material thunderIcon;
    public Material waterIcon;
    public Material fairyIcon;
    public Material beastIcon;
    public Material insectIcon;

    public Dictionary<string, Material> icons = new Dictionary<string, Material>();
    // Start is called before the first frame update
    void Awake()
    {
        icons["WARRIOR"] = warriorIcon;
        icons["SPELLCASTER"] = spellcasterIcon;
        icons["ROCK"] = stoneIcon;
        icons["PLANT"] = plantIcon;
        icons["ZOMBIE"] = zombieIcon;
        icons["WINGEDBEAST"] = wingedbeastIcon;
        icons["DRAGON"] = dragonIcon;
        icons["FIRE"] = fireIcon;
        icons["MACHINE"] = machineIcon;
        icons["THUNDER"] = thunderIcon;
        icons["WATER"] = waterIcon;
        icons["FAIRY"] = fairyIcon;
        icons["BEAST"] = beastIcon;
        icons["INSECT"] = insectIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
