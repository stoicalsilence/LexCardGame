using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public enum STATE { PLAYERTURN, ENEMYTURN } //Load-in Swirl?
    public STATE turnState;

    public Transform playerCamPos;
    public Transform playerBoardView;
    public Transform enemyBoardView;
    public Transform enemyCamPos;
    public bool turnEnded;

    public Player player;
    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !turnEnded && player.playedCard && turnState == STATE.PLAYERTURN)
        {
            StartCoroutine(nextTurn());
        }

        if (turnState == STATE.PLAYERTURN && (player.currentAction == Player.ACTION.CHOOSING  || player.currentAction == Player.ACTION.CONFIRMING))
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        }

        if(turnState == STATE.PLAYERTURN && (player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD)) 
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, playerBoardView.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerBoardView.rotation, Time.deltaTime * 8);
        }

        if(turnState == STATE.ENEMYTURN && (enemy.currentAction == Enemy.ACTION.CHOOSING || enemy.currentAction == Enemy.ACTION.CONFIRMING))
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, enemyCamPos.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, enemyCamPos.rotation, Time.deltaTime * 8);
        }

        if (turnState == STATE.ENEMYTURN && (enemy.currentAction == Enemy.ACTION.BOARDVIEW || enemy.currentAction == Enemy.ACTION.PLACINGCARD))
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, enemyBoardView.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, enemyBoardView.rotation, Time.deltaTime * 8);
        }
        
    }
    public IEnumerator nextTurn()
    {
        turnEnded = true;
        yield return new WaitForSeconds(1f);
        if (turnState == STATE.PLAYERTURN)
        {
            turnState = STATE.ENEMYTURN;
            enemy.currentAction = Enemy.ACTION.CHOOSING;
            enemy.StartCoroutine(enemy.drawCards());
            if (enemy.hand.Count > 0)
            {
                enemy.ResetLayers();
            }
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(enemy.prepareCardToPlace());
        }
        else
        {
            turnState = STATE.PLAYERTURN;
            enemy.UnrenderCards();
            player.currentAction = Player.ACTION.CHOOSING;
            StartCoroutine(player.drawCards());
            player.ResetLayers();
            player.playedCard = false;
        }
        turnEnded = false;
    }
}
