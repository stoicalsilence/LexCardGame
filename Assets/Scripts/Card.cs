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
    [TextArea]
    public string description;
    //public Sprite image

    public Card()
    {

    }

    public Card(int id, string cardName, int attack, int defense, string description)
    {
        this.id = id;
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.description = description;
    } 

    public void initialise(int id, string cardName, int attack, int defense, string description)
    {
        this.id = id;
        this.cardName = cardName;
        this.attack = attack;
        this.defense = defense;
        this.description = description;
    }
}
