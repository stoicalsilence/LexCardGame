using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameEvaluation : MonoBehaviour
{
    // Start is called before the first frame update
    public List<FieldCard> playerFieldCards;
    public List<FieldCard> enemyFieldCards;
    public Enemy enemy;
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FieldCard findStrongestPlayerCard()
    {
        int highestAttack = -1;
        foreach(FieldCard currentCard in playerFieldCards)
        {
            if(currentCard.attack > highestAttack)
            {
                highestAttack = currentCard.attack;
            }
        }
        if(highestAttack == -1)
        {
            return null;
        }
        return playerFieldCards.Find(card => card.attack == highestAttack);
    }

    public Tile findEmptyEnemyTile()
    {
        GameObject[] possibleTiles = GameObject.FindGameObjectsWithTag("EnemyField");

        foreach (GameObject possibleTile in possibleTiles)
        {
            if (possibleTile.GetComponent<Tile>().cardOnTile != null)
            {
                return possibleTile.GetComponent<Tile>();
            }
        }
        return null;
    }

    public FieldCard findWeakestPlayerCard(bool lookForAttack)
    {
        FieldCard weakestCard;
        if (lookForAttack)
        {
            weakestCard = enemyFieldCards.OrderBy(p => p.attack).FirstOrDefault();
        }
        else
        {
            weakestCard = enemyFieldCards.OrderBy(p => p.defense).FirstOrDefault();
        }
        return weakestCard;
    }

    public Tile FindTileWithWeakestEnemyCard()
    {
        GameObject[] possibleTiles = GameObject.FindGameObjectsWithTag("EnemyField");
        GameObject tileWithWeakestMonster = possibleTiles.OrderBy(twwc => twwc.GetComponent<Tile>().cardOnTile.attack).FirstOrDefault();
        return tileWithWeakestMonster.GetComponent<Tile>();
    }

    public PlayingCard FindStrongestAttackEnemyCardInHand()
    {
        return enemy.hand.OrderByDescending(sc => sc.attack).FirstOrDefault();
    }

    public PlayingCard FindStrongestDefenseEnemyCardInHand()
    {
        return enemy.hand.OrderByDescending(sc => sc.defense).FirstOrDefault();
    }

}
