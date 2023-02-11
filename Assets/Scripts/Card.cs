using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Assets/Cards")]
public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public int attack;
    public int defense;
    public enum TYPE { STONE, THUNDER, MACHINE, ROCK, FIRE, WATER, DRAGON, WARRIOR, FAIRY, INSECT, ZOMBIE, BEAST, PLANT, WINGEDBEAST }
    public TYPE type;
    [TextArea]
    public string description;
    public Material cardArt;
    //public Sprite image

    public Card()
    {

    }

    public Card(int id, string cardName, int attack, int defense, TYPE type, string description, Material cardArt)
    {
        this.id = id;
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.type = type;
        this.description = description;
        this.cardArt = cardArt;
    } 

    public void initialise(int id, string cardName, int attack, int defense, TYPE type, string description, Material cardArt)
    {
        this.id = id;
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.type = type;
        this.description = description;
        this.cardArt = cardArt;
    }
}
