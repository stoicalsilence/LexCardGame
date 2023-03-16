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
    public Transform playerAttackFirstCardPos;
    public Transform playerAttackSecondCardPos;

    public Transform enemyAttackFirstCardPos;
    public Transform enemyAttackSecondCardPos;
    public CameraMovement cameraMovement;
    Vector3 playerCardOriginalPos;
    Vector3 enemyCardOriginalPos;


    public bool playerAttacking;
    public bool playerAttackingEnemyLP;
    public bool enemyAttackingEnemyLP;
    public bool enemyAttacking;
    public FieldCard playerCard;
    public FieldCard enemyCard;

    public bool cardsReturning;
    public bool firstTurnPlayed;

    public GameObject slashAnimation;
    public Transform pos_EnemyCard_SlashAnim;
    public Transform pos_PlayerCard_SlashAnim;
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if((FindPlayerDeclaringAttackCard() && FindEnemyTargetedCard())  || (FindEnemyDeclaringAttackCard() && FindPlayerTargetedCard()))
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
                StartCoroutine(enemyAttack());
            }
            attackCommencing = false;
        }

        if (playerAttacking)
        {
            Quaternion target2 = Quaternion.Euler(0, -90, -90);
            if(playerCard)
            playerCard.transform.rotation = Quaternion.Slerp(playerCard.transform.rotation, target2, Time.deltaTime * 10);
            if(enemyCard)
            enemyCard.transform.rotation = Quaternion.Slerp(enemyCard.transform.rotation, target2, Time.deltaTime * 10);
            if(playerCard)
            playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, playerAttackFirstCardPos.position, Time.deltaTime * 15);
            if(enemyCard)
            enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, playerAttackSecondCardPos.position, Time.deltaTime * 15);
        }
        if (playerAttackingEnemyLP)
        {
            Quaternion target2 = Quaternion.Euler(0, -90, -90);
            playerCard.transform.rotation = Quaternion.Slerp(playerCard.transform.rotation, target2, Time.deltaTime * 10);
            playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, playerAttackFirstCardPos.position, Time.deltaTime * 15);
        }
        if (enemyAttacking)
        {
            Quaternion target2 = Quaternion.Euler(0, 90, -90);
            if (playerCard)
                playerCard.transform.rotation = Quaternion.Slerp(playerCard.transform.rotation, target2, Time.deltaTime * 10);
            if (enemyCard)
                enemyCard.transform.rotation = Quaternion.Slerp(enemyCard.transform.rotation, target2, Time.deltaTime * 10);
            if (playerCard)
                playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, enemyAttackFirstCardPos.position, Time.deltaTime * 15);
            if (enemyCard)
                enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, enemyAttackSecondCardPos.position, Time.deltaTime * 15);
        }
        if (enemyAttackingEnemyLP)
        {
            Quaternion target2 = Quaternion.Euler(0, 90, -90);
            enemyCard.transform.rotation = Quaternion.Slerp(enemyCard.transform.rotation, target2, Time.deltaTime * 10);
            enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, enemyAttackSecondCardPos.position, Time.deltaTime * 15);
        }


        if (cardsReturning)
        {
            returnCards();
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

    public FieldCard findStrongestPlayerCardOnlyAttackMode()
    {
        int highestAttack = -1;
        foreach (FieldCard currentCard in playerFieldCards)
        {
            if (!currentCard.inDefenseMode)
            {
                if (currentCard.attack > highestAttack)
                {
                    highestAttack = currentCard.attack;
                }
            }
            if (highestAttack == -1)
            {
                return null;
            }
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

    public bool AssertIsEnemyWinning()
    {
        //TODO
        //IF PlAYER HAS NO CARD ON FIELD, ENEMY HAS CARD ON FIELD
        //IF ENEMY HAS HIGHER DEF CARD THAN PLAYERS STRONGEST ATK CARD
        //ENEMY CAN THEN DO USE MAGIC
        //IS IS SPECIFIC ENEMY ALWAYS USE SPECIFIC MAGIC -> FIELD MAGE -> HAS FIELD CARD -> IF FIELD ISNT ALREADY X -> PLAY FIELD CARD //IF HAS MORE THAN 2000 HP
        return true;
    }

    public PlayingCard FindStrongestAttackEnemyCardInHand()
    {
        return enemy.hand.OrderByDescending(sc => sc.attack).FirstOrDefault();
    }

    public bool CheckIfEnemyHasCardsWithoutAttacks()
    {
        if (enemyFieldCards.Any(fieldcard => fieldcard.attackedThisTurn == false))
        {
            return true;
        }
        return false;
    }

    public FieldCard FindEnemyCardWithoutActedAttack()
    {
        return enemyFieldCards.FirstOrDefault(fieldcard => fieldcard.attackedThisTurn == false);
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

        attackCommencing = false;
        disableInput();
        playerCard = FindPlayerDeclaringAttackCard();
        enemyCard = FindEnemyTargetedCard();
        removeAllDeclarationAndTargeting();
        playerCard.faceDown = false;
        enemyCard.faceDown = false;
        playerCardOriginalPos = playerCard.gameObject.transform.position;
        enemyCardOriginalPos = enemyCard.gameObject.transform.position;
        
        playerCard.movementBlocked = true;
        enemyCard.movementBlocked = true;

        playerAttacking = true;

        yield return new WaitForSeconds(1.5f);
        GameObject slashanim = Instantiate(slashAnimation, pos_EnemyCard_SlashAnim.position, Quaternion.Euler(0,-90,-90));
        Destroy(slashanim, 0.4f);
        yield return new WaitForSeconds(1.5f);
        
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
                enemyFieldCards.Remove(enemyCard);
                enemyCard.tile.clearTile();
                Destroy(enemyCard.gameObject);
               
            }
            if(playerCard.attack == enemyCard.attack)
            {
                GameObject deathParticles = Instantiate(enemyCard.CardDeathParticles, enemyCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                //GameObject pdeathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                //Destroy(pdeathParticles, 5);
                playerCard.tile.clearTile();
                enemyCard.tile.clearTile();
                playerFieldCards.Remove(playerCard);
                enemyFieldCards.Remove(enemyCard);
                Destroy(playerCard.gameObject);
                Destroy(enemyCard.gameObject);
            }
            if(playerCard.attack < enemyCard.attack)
            {
                int overkillDmg = enemyCard.attack - playerCard.attack;
                player.lifepoints -= overkillDmg;
                GameObject deathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                playerCard.tile.clearTile();
                playerFieldCards.Remove(playerCard);
                Destroy(playerCard.gameObject);
            }
        }
        else
        {
            if(playerCard.attack > enemyCard.defense)
            {
                GameObject deathParticles = Instantiate(enemyCard.CardDeathParticles, enemyCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                enemyFieldCards.Remove(enemyCard);
                enemyCard.tile.clearTile();
                Destroy(enemyCard.gameObject);
            }
            if (playerCard.attack == enemyCard.defense)
            {
                //TODO: spawn impact effect
            }
            if (playerCard.attack < enemyCard.defense)
            {
                GameObject slashanim2 = Instantiate(slashAnimation, playerAttackFirstCardPos.position, Quaternion.Euler(0, -90, -90));
                Destroy(slashanim2, 0.4f);
                int overkillDmg = enemyCard.defense - playerCard.attack;
                player.lifepoints -= overkillDmg;
            }
        }
        yield return new WaitForSeconds(1);
        playerAttacking = false;
        cardsReturning = true;
        playerCard.attackedThisTurn = true;
        enableInput();
        yield return new WaitForSeconds(1);
        cardsReturning = false;
        
        if (playerCard)
        {
            playerCard.transform.position = playerCardOriginalPos;
        }
        if (enemyCard)
        {
            enemyCard.transform.position = enemyCardOriginalPos;
        }
    }

    public IEnumerator enemyAttack()
    {
        attackCommencing = false;
        disableInput();
        playerCard = FindPlayerTargetedCard();
        enemyCard = FindEnemyDeclaringAttackCard();
        removeAllDeclarationAndTargeting();
        playerCard.faceDown = false;
        enemyCard.faceDown = false;
        playerCardOriginalPos = playerCard.gameObject.transform.position;
        enemyCardOriginalPos = enemyCard.gameObject.transform.position;

        playerCard.movementBlocked = true;
        enemyCard.movementBlocked = true;

        enemyAttacking = true;

        yield return new WaitForSeconds(1.5f);
        GameObject slashanim = Instantiate(slashAnimation, enemyAttackFirstCardPos.position, Quaternion.Euler(0, 90, -90));
        Destroy(slashanim, 0.4f);
        yield return new WaitForSeconds(1.5f);

        playerCard.movementBlocked = false;
        
        
        if (!playerCard.inDefenseMode)
        {
            if (enemyCard.attack > playerCard.attack)
            {
                int overkillDmg = enemyCard.attack - playerCard.attack;
                player.lifepoints -= overkillDmg;
                GameObject deathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                playerFieldCards.Remove(playerCard);
                playerCard.tile.clearTile();
                Destroy(playerCard.gameObject);

            }
            if (enemyCard.attack == playerCard.attack)
            {
                GameObject deathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                //GameObject pdeathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                //Destroy(pdeathParticles, 5);
                playerCard.tile.clearTile();
                enemyCard.tile.clearTile();
                playerFieldCards.Remove(playerCard);
                enemyFieldCards.Remove(enemyCard);
                Destroy(playerCard.gameObject);
                Destroy(enemyCard.gameObject);
            }
            if (enemyCard.attack < playerCard.attack)
            {
                int overkillDmg = playerCard.attack - enemyCard.attack;
                enemy.lifepoints -= overkillDmg;
                GameObject deathParticles = Instantiate(enemyCard.CardDeathParticles, enemyCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                enemyCard.tile.clearTile();
                enemyFieldCards.Remove(enemyCard);
                Destroy(enemyCard.gameObject);
            }
        }
        else
        {
            if (enemyCard.attack > playerCard.defense)
            {
                
                GameObject deathParticles = Instantiate(playerCard.CardDeathParticles, playerCard.particlePos.position, Quaternion.identity);
                Destroy(deathParticles, 5);
                playerFieldCards.Remove(playerCard);
                playerCard.tile.clearTile();
                Destroy(playerCard.gameObject);
            }
            if (enemyCard.attack == playerCard.defense)
            {
                //TODO: spawn impact effect
            }
            if (enemyCard.attack < playerCard.defense)
            {
                //TODO: spawn attack effect on playercard
                int overkillDmg = playerCard.defense - enemyCard.attack;
                enemy.lifepoints -= overkillDmg;
            }
        }
        yield return new WaitForSeconds(1);
        enemyAttacking = false;
        cardsReturning = true;
        enemyCard.movementBlocked = false;
        enemyCard.attackedThisTurn = true;
        enableInput();
        yield return new WaitForSeconds(1);
        cardsReturning = false;

        if (playerCard)
        {
            playerCard.transform.position = playerCardOriginalPos;
        }
        if (enemyCard)
        {
            enemyCard.transform.position = enemyCardOriginalPos;
        }
    }

    public void returnCards()
    {
        if (playerCard)
        {
            playerCard.transform.position = Vector3.Slerp(playerCard.transform.position, playerCardOriginalPos, Time.deltaTime * 15);
        }
        if (enemyCard)
        {
            enemyCard.transform.position = Vector3.Slerp(enemyCard.transform.position, enemyCardOriginalPos, Time.deltaTime * 15);
        }
    }

    public IEnumerator playerAttackEnemyLP()
    {
        disableInput();
        playerCard = FindPlayerDeclaringAttackCard();
        playerCard.faceDown = false;
        playerCardOriginalPos = playerCard.gameObject.transform.position;
        playerCard.movementBlocked = true;
        playerAttackingEnemyLP = true;

        yield return new WaitForSeconds(1.5f);
        GameObject slashanim = Instantiate(slashAnimation, pos_EnemyCard_SlashAnim.position, Quaternion.Euler(0, -90, -90));
        Destroy(slashanim, 0.4f);
        yield return new WaitForSeconds(1.5f);

        playerCard.movementBlocked = false;
        enemy.lifepoints -= playerCard.attack;
        yield return new WaitForSeconds(1);
        playerAttackingEnemyLP = false;
        cardsReturning = true;
        enableInput();
        removeAllDeclarationAndTargeting();
        playerCard.attackedThisTurn = true;
        yield return new WaitForSeconds(1);
        cardsReturning = false;
        playerCard.transform.position = playerCardOriginalPos;
    }
    public IEnumerator enemyAttackEnemyLP()
    {
        disableInput();
        enemyCard = FindEnemyDeclaringAttackCard();
        enemyCard.faceDown = false;
        enemyCardOriginalPos = enemyCard.gameObject.transform.position;
        enemyCard.movementBlocked = true;
        enemyAttackingEnemyLP = true;

        yield return new WaitForSeconds(1.5f);
        GameObject slashanim = Instantiate(slashAnimation, enemyAttackFirstCardPos.position, Quaternion.Euler(0, 90, -90));
        Destroy(slashanim, 0.4f);
        yield return new WaitForSeconds(1.5f);

        
        player.lifepoints -= enemyCard.attack;
        yield return new WaitForSeconds(1);
        enemyAttackingEnemyLP = false;
        cardsReturning = true;
        enableInput();
        removeAllDeclarationAndTargeting();
        enemyCard.attackedThisTurn = true;
        enemyCard.movementBlocked = false;
        yield return new WaitForSeconds(1);
        cardsReturning = false;
        enemyCard.transform.position = enemyCardOriginalPos;
    }


    public void disableInput()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<CameraMovement>().turnEndable = false;
    }
    public void enableInput()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        FindObjectOfType<CameraMovement>().turnEndable = true;
    }
    public void refreshCardAttacks()
    {
        foreach(FieldCard card in playerFieldCards)
        {
            card.attackedThisTurn = false;
        }
        foreach (FieldCard card in enemyFieldCards)
        {
            card.attackedThisTurn = false;
        }
    }

    public FieldCard findPlayerCardWithLowerAttack(FieldCard enemyCard)
    {
        return playerFieldCards.FirstOrDefault(obj => obj.attack < enemyCard.attack);
    }

    public FieldCard findPlayerCardWithLowerDefenseThanEnemyAttack(FieldCard enemyCard)
    {
        return playerFieldCards.FirstOrDefault(obj => obj.defense < enemyCard.attack);
    }


}
