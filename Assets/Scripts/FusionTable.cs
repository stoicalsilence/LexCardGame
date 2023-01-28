using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionTable : MonoBehaviour
{
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

        if (firstCard.attack + secondCard.attack < 2000)
        {
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER))
            {
                return Database.GetCardById(0); //Thunder Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.MACHINE))
            {
                return Database.GetCardById(3);  //Machine Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.STONE))
            {
                return Database.GetCardById(1);  //Stone Dragon
            }
        }

        if (firstCard.attack + secondCard.attack > 2000)
        {
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER))
            {
                return Database.GetCardById(2);  //Twin Headed Thunder Dragon
            }
        }

        return null;
    }
}
//if (firstCard.type == PlayingCard.TYPE.DRAGON && secondCard.type == PlayingCard.TYPE.THUNDER)
//{
//    return Database.GetCardById(0);  //Thunder Dragon
//}