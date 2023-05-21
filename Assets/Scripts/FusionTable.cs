using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionTable : MonoBehaviour
{
    public PlayingCard failedFuseCard;

    HashSet<int> thth = new HashSet<int> { 1, 1 };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card returnFusion(PlayingCard firstCard, PlayingCard secondCard)
    {
        HashSet<PlayingCard.TYPE> cardTypes = new HashSet<PlayingCard.TYPE>() { firstCard.type, secondCard.type };
        HashSet<int> ids = new HashSet<int> { firstCard.id, secondCard.id };
      
        if(firstCard.attack + secondCard.attack < 1000)
        {
            if(cardTypes.Contains(PlayingCard.TYPE.ZOMBIE) && cardTypes.Contains(PlayingCard.TYPE.PLANT))
            {
                Debug.Log("fused gnarlroot");
                return Database.GetCardById(108);
            }
        }

        if ((firstCard.attack + secondCard.attack < 2000) && (firstCard.attack + secondCard.attack > 1000))
        {
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER))
            {
                Debug.Log("fused thunder dragon");
                return Database.GetCardById(1); //Thunder Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.MACHINE))
            {
                Debug.Log("fused machine dragon");
                return Database.GetCardById(4);  //Machine Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.ROCK))
            {
                Debug.Log("fused stone dragon");
                return Database.GetCardById(2);  //Stone Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.PLANT) && cardTypes.Contains(PlayingCard.TYPE.WARRIOR))
            {
                Debug.Log("fused bean warrior");
                return Database.GetCardById(11);  //Bean Warrior
            }
            if (cardTypes.Contains(PlayingCard.TYPE.PLANT) && cardTypes.Contains(PlayingCard.TYPE.ZOMBIE))
            {
                Debug.Log("fused wood remains");
                return Database.GetCardById(120);  //Thornmire Terror
            }
            if (cardTypes.Contains(PlayingCard.TYPE.FIRE) && cardTypes.Contains(PlayingCard.TYPE.WINGEDBEAST))
            {
                Debug.Log("fused talontorch");
                return Database.GetCardById(58);  //Talontorch
            }
            if(cardTypes.Contains(PlayingCard.TYPE.FAIRY) && cardTypes.Contains(PlayingCard.TYPE.PLANT))
            {
                Debug.Log("fused Bloomweaver Dryad");
                return Database.GetCardById(79); //Bloomweaver Dryad
            }
            if (cardTypes.Contains(PlayingCard.TYPE.FAIRY) && cardTypes.Contains(PlayingCard.TYPE.SPELLCASTER))
            {
                Debug.Log("fused Celestial Arbiter");
                return Database.GetCardById(110); //Celestial Arbiter
            }
            if (cardTypes.Contains(PlayingCard.TYPE.WATER) && cardTypes.Contains(PlayingCard.TYPE.SPELLCASTER))
            {
                Debug.Log("Mystic Tidecaller");
                return Database.GetCardById(100); //Mystic Tidecaller
            }
            if (cardTypes.Contains(PlayingCard.TYPE.ZOMBIE) && cardTypes.Contains(PlayingCard.TYPE.FIEND))
            {
                Debug.Log("Hellghoul");
                return Database.GetCardById(105); //Hellghoul
            }
            if (cardTypes.Contains(PlayingCard.TYPE.FIRE) && cardTypes.Contains(PlayingCard.TYPE.SPELLCASTER))
            {
                Debug.Log("Hellghoul");
                return Database.GetCardById(114); //Fire Deon
            }
        }

        if ((firstCard.attack + secondCard.attack) > 2000)
        {
            if ((cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER)) || ids.SetEquals(thth))
            {
                Debug.Log("fused twin headed thunder dragon");
                return Database.GetCardById(3);  //Twin Headed Thunder Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.FIRE) && cardTypes.Contains(PlayingCard.TYPE.WINGEDBEAST))
            {
                Debug.Log("fused Crimson Sunbird");
                return Database.GetCardById(9);  // Crimson Sunbird
            }
            if(cardTypes.Contains(PlayingCard.TYPE.INSECT) && cardTypes.Contains(PlayingCard.TYPE.WARRIOR))
            {
                Debug.Log("fused Legendary Lord Scaraborn");
                return Database.GetCardById(94); //Legendary Lord Scaraborn
            }
            if (cardTypes.Contains(PlayingCard.TYPE.ROCK) && cardTypes.Contains(PlayingCard.TYPE.FIRE))
            {
                Debug.Log("fused Molten Rock Golem");
                return Database.GetCardById(99); //Molten Rock Golem
            }
            
        }
        failedFuseCard = secondCard;
        Debug.Log("Fused nothing");
        return null;
    }

    public PlayingCard returnFailedFusion()
    {
        return failedFuseCard;
    }
}
//if (firstCard.type == PlayingCard.TYPE.DRAGON && secondCard.type == PlayingCard.TYPE.THUNDER)
//{
//    return Database.GetCardById(0);  //Thunder Dragon
//}