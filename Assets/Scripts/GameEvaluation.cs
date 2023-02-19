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
    public Player player;
    public bool attackCommencing;
    public Transform attackFirstCardPos;
    public Transform attackSecondCardPos;
    public CameraMovement cameraMovement;

    public bool playerAttacking;
    public bool enemyAttacking;
    public FieldCard playerCard;
    public FieldCard enemyCard;

    public bool cardsMoving;
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if((FindPlayerDeclaringAttackCard() && FindEnemyTargetedCard())  || FindEnemyTargetedCard() && FindPlayerTargetedCard())
        {
            attackCommencing = true;
        }

        if (attackCommencing)
        {
            if(cameraMovement.turnState == CameraMovement.STATE.PLAYERTURN)
            {
                StartCoroutine(playerAttack());
            }
            if(cameraMovement.turnState == CameraMovement.STATE.ENEMYTURN)
            {

            }
            attackCommencing = false;
            removeAllDeclarationAndTargeting();
        }

        if (playerAttacking)
        {
            Quaternion target2 = Quaternion.Euler(0, -90, -90);
            playerCard.transform.rotation = Quaternion.Slerp(playerCard.transform.rotation, target2, Time.deltaTime * 10);
            enemyCard.transform.rotation = Quaternion.Slerp(enemyCard.transform.rotation, target2, Time.deltaTime * 10);

            playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, attackFirstCardPos.position, Time.deltaTime * 15);
            enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, attackSecondCardPos.position, Time.deltaTime * 15);
        }
        if (enemyAttacking)
        {
            Quaternion target2 = Quaternion.Euler(0, -90, -90);
            playerCard.transform.rotation = Quaternion.Slerp(playerCard.transform.rotation, target2, Time.deltaTime * 10);
            enemyCard.transform.rotation = Quaternion.Slerp(enemyCard.transform.rotation, target2, Time.deltaTime * 10);

            playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, attackSecondCardPos.position, Time.deltaTime * 15);
            enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, attackFirstCardPos.position, Time.deltaTime * 15);
        }
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
            if (possibleTile.GetComponent<Tile>().cardOnTile == null)
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
            weakestCard = playerFieldCards.OrderBy(p => p.attack).FirstOrDefault();
        }
        else
        {
            weakestCard = playerFieldCards.OrderBy(p => p.defense).FirstOrDefault();
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

    public FieldCard FindPlayerDeclaringAttackCard()
    {
        return playerFieldCards.FirstOrDefault(card => card.declaringAttack == true);
    }

    public FieldCard FindEnemyDeclaringAttackCard()
    {
        return enemyFieldCards.FirstOrDefault(card => card.declaringAttack == true);
    }
    public FieldCard FindPlayerTargetedCard()
    {
        return playerFieldCards.FirstOrDefault(card => card.targeted == true);
    }
    public FieldCard FindEnemyTargetedCard()
    {
        return enemyFieldCards.FirstOrDefault(card => card.targeted == true);
    }

    public void removeAllDeclarationAndTargeting()
    {
        foreach(FieldCard card in playerFieldCards)
        {
            card.declaringAttack = false;
            card.targeted = false;
        }
        foreach (FieldCard card in enemyFieldCards)
        {
            card.declaringAttack = false;
            card.targeted = false;
        }
    }

    public IEnumerator playerAttack()
    {
        playerCard = FindPlayerDeclaringAttackCard();
        enemyCard = FindEnemyTargetedCard();
        playerCard.faceDown = false;
        enemyCard.faceDown = false;
        Vector3 playerCardOriginalPos = playerCard.gameObject.transform.position;
        Vector3 enemyCardOriginalPos = enemyCard.gameObject.transform.position;
        
        playerCard.movementBlocked = true;
        enemyCard.movementBlocked = true;

        playerAttacking = true;

        yield return new WaitForSeconds(3);
        playerCard.movementBlocked = false;
        enemyCard.movementBlocked = false;
        if (!enemyCard.inDefenseMode)
        {
            if(playerCard.attack > enemyCard.attack)
            {
                int overkillDmg = playerCard.attack - enemyCard.attack;
                enemy.lifepoints -= overkillDmg;
                GameObject deathParticles = Instantiate(enemyCard.CardDeathParticles, enemyCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                enemyCard.tile.hasCard = false; //add destroy card function to tile? or to fieldcard
                Destroy(enemyCard.gameObject);
            }
            if(playerCard.attack == enemyCard.attack)
            {
                GameObject deathParticles = Instantiate(enemyCard.CardDeathParticles, enemyCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                GameObject pdeathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(pdeathParticles, 5);
                Destroy(playerCard.gameObject);
                Destroy(enemyCard.gameObject);
            }
            if(playerCard.attack < enemyCard.attack)
            {
                int overkillDmg = enemyCard.attack - playerCard.attack;
                player.lifepoints -= overkillDmg;
                GameObject pdeathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(pdeathParticles, 5);
                Destroy(playerCard.gameObject);
            }
        }
        yield return new WaitForSeconds(1);
        playerAttacking = false;
        if (playerCard)
        {
            playerCard.transform.position = playerCardOriginalPos;
        }
        if (enemyCard)
        {
            enemyCard.transform.position = enemyCardOriginalPos;
        }
        
    }
    public void enemyAttack()
    {

    }
}
