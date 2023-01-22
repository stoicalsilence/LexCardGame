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
        if(turnState == STATE.PLAYERTURN && (player.currentAction == Player.ACTION.CHOOSING  || player.currentAction == Player.ACTION.CONFIRMING))
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        }

        if(turnState == STATE.PLAYERTURN && (player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD)) 
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, playerBoardView.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerBoardView.rotation, Time.deltaTime * 8);
        }

        if(turnState == STATE.ENEMYTURN)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, enemyCamPos.position, Time.deltaTime * 8);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, enemyCamPos.rotation, Time.deltaTime * 8);
        }

        //if (turnState == STATE.PLAYERTURN && player.currentAction == Player.ACTION.CHOOSING &&!turnEnded)
        //{
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
        //    //cam.transform.position = Vector3.MoveTowards(playerCamPos.position, enemyCamPos.position, 5 * Time.deltaTime);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        //}

        //if (turnState == STATE.PLAYERTURN && turnEnded)
        //{
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
        //    //cam.transform.position = Vector3.MoveTowards(playerCamPos.position, enemyCamPos.position, 5 * Time.deltaTime);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        //    //enemy.playedcard = false;
        //}


        //if (turnState == STATE.ENEMYTURN && turnEnded)
        //{
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, enemyCamPos.position, Time.deltaTime * 15);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, enemyCamPos.rotation, Time.deltaTime * 15);
        //    cam.transform.position = enemyCamPos.position;
        //    cam.transform.rotation = enemyCamPos.rotation;
        //}

        //if (turnState == STATE.ENEMYTURN && turnEnded)
        //{
        //    //cam.transform.position = Vector3.MoveTowards(enemyCamPos.position, playerCamPos.position, 5 * Time.deltaTime);
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        //    player.playedCard = false;
        //}

        //if((player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD ) && turnState == STATE.PLAYERTURN)
        //{
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, playerBoardView.position, Time.deltaTime * 8);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerBoardView.rotation, Time.deltaTime * 8);
        //}
        //if(turnState == STATE.PLAYERTURN && player.currentAction == Player.ACTION.CONFIRMING)
        //{
        //    cam.transform.position = Vector3.Slerp(cam.transform.position, playerCamPos.position, Time.deltaTime * 8);
        //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, playerCamPos.rotation, Time.deltaTime * 8);
        //}
    }
    public IEnumerator nextTurn()
    {
        turnEnded = true;
        yield return new WaitForSeconds(1f);

        if(turnState == STATE.PLAYERTURN)
        {
            turnState = STATE.ENEMYTURN;
            enemy.StartCoroutine(enemy.drawCards());
            yield return new WaitForSeconds(1.5f);
            enemy.prepareCardToPlace();
        }
        else
        {
            turnState = STATE.PLAYERTURN;
            player.currentAction = Player.ACTION.CHOOSING;
            player.StartCoroutine(player.drawCards());
            player.ResetLayers();
            player.playedCard = false;
        }
        turnEnded = false;
    }
}
