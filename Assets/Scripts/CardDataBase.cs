using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Database", menuName = "Assets/Databases/CardDatabase")]
public class CardDataBase : ScriptableObject
{
    public List<Card> cardList;
    //id, name, attack, defense, description

    private void Awake()
    {

        //GameObject ThunderDragon = new GameObject();
        //ThunderDragon.AddComponent<Card>();
        //ThunderDragon.GetComponent<Card>().id = 0;
        //ThunderDragon.GetComponent<Card>().name = "Thunder Dragon";
        //ThunderDragon.GetComponent<Card>().cardName = "Thunder Dragon";
        //ThunderDragon.GetComponent<Card>().attack = 1600;
        //ThunderDragon.GetComponent<Card>().defense = 1500;
        //ThunderDragon.GetComponent<Card>().description = "An Elemental Thunder Dragon.";

        //GameObject StoneDragon = new GameObject();
        //StoneDragon.AddComponent<Card>();
        //StoneDragon.GetComponent<Card>().id = 1;
        //StoneDragon.GetComponent<Card>().name = "Stone Dragon";
        //StoneDragon.GetComponent<Card>().cardName = "Stone Dragon";
        //StoneDragon.GetComponent<Card>().attack = 2000;
        //StoneDragon.GetComponent<Card>().defense = 2300;
        //StoneDragon.GetComponent<Card>().description = "An Elemental Stone Dragon.";

        //GameObject TwinHeadedThunderDragon = new GameObject();
        //TwinHeadedThunderDragon.AddComponent<Card>();
        //TwinHeadedThunderDragon.GetComponent<Card>().id = 2;
        //TwinHeadedThunderDragon.GetComponent<Card>().name = "Twin Headed Thunder Dragon";
        //TwinHeadedThunderDragon.GetComponent<Card>().cardName = "Twin Headed Thunder Dragon";
        //TwinHeadedThunderDragon.GetComponent<Card>().attack = 2800;
        //TwinHeadedThunderDragon.GetComponent<Card>().defense = 2100;
        //TwinHeadedThunderDragon.GetComponent<Card>().description = "A twin headed dragon that thrives in mountainous and conductive fields alike.";

        //GameObject MachineDragon = new GameObject();
        //MachineDragon.AddComponent<Card>();
        //MachineDragon.GetComponent<Card>().id = 3;
        //MachineDragon.GetComponent<Card>().name = "Machine Dragon";
        //MachineDragon.GetComponent<Card>().cardName = "Machine Dragon";
        //MachineDragon.GetComponent<Card>().attack = 1850;
        //MachineDragon.GetComponent<Card>().defense = 1700;
        //MachineDragon.GetComponent<Card>().description = "A flying Machine that resembles the look of a dragon.";
        ////Vergil, The Demon Hunter

        //cardList.Add(ThunderDragon.GetComponent<Card>());
        //cardList.Add(StoneDragon.GetComponent<Card>());
        //cardList.Add(TwinHeadedThunderDragon.GetComponent<Card>());
        //cardList.Add(MachineDragon.GetComponent<Card>());
    }
}
