using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionTable : MonoBehaviour
{
    public PlayingCard failedFuseCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card returnFusion(PlayingCard firstCard, PlayingCard secondCard) //always +1 because idk
    {
        HashSet<PlayingCard.TYPE> cardTypes = new HashSet<PlayingCard.TYPE>() { firstCard.type, secondCard.type };
        HashSet<int> ids = new HashSet<int> { firstCard.id, secondCard.id };

        if (firstCard.attack + secondCard.attack < 2000)
        {
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER))
            {
                Debug.Log("fused thunder dragon");
                return Database.GetCardById(0); //Thunder Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.MACHINE))
            {
                Debug.Log("fused machine dragon");
                return Database.GetCardById(3);  //Machine Dragon
            }
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.STONE))
            {
                Debug.Log("fused stone dragon");
                return Database.GetCardById(1);  //Stone Dragon
            }
        }

        if ((firstCard.attack + secondCard.attack) > 2000)
        {
            if (cardTypes.Contains(PlayingCard.TYPE.DRAGON) && cardTypes.Contains(PlayingCard.TYPE.THUNDER) || ids.ToString() == "[1,1]")
            {
                Debug.Log("fused twin headed thunder dragon");
                return Database.GetCardById(3);  //Twin Headed Thunder Dragon
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