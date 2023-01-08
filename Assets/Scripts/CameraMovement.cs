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
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnState == STATE.PLAYERTURN && player.currentAction == Player.ACTION.CHOOSING &&!turnEnded)
        {
            cam.transform.position = playerCamPos.position;
            cam.transform.rotation = playerCamPos.rotation;
        }

        if (turnState == STATE.PLAYERTURN && turnEnded)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, enemyCamPos.position, 1);
            //cam.transform.position = Vector3.MoveTowards(playerCamPos.position, enemyCamPos.position, 5 * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(playerCamPos.rotation, enemyCamPos.rotation, 1);
        }


        if (turnState == STATE.ENEMYTURN && !turnEnded)
        {
            cam.transform.position = enemyCamPos.position;
        }

        if (turnState == STATE.ENEMYTURN && turnEnded)
        {
            //cam.transform.position = Vector3.MoveTowards(enemyCamPos.position, playerCamPos.position, 5 * Time.deltaTime);
            cam.transform.position = Vector3.Slerp(cam.transform.position, playerCamPos.position, 1);
            cam.transform.rotation = Quaternion.Slerp(transform.rotation, playerCamPos.rotation, 1);
        }

        if((player.currentAction == Player.ACTION.BOARDVIEW || player.currentAction == Player.ACTION.PLACINGCARD ) && turnState == STATE.PLAYERTURN)
        {
            cam.transform.position = playerBoardView.position;
            cam.transform.rotation = playerBoardView.rotation;
        }
        if(turnState == STATE.PLAYERTURN && player.currentAction == Player.ACTION.CONFIRMING)
        {
            cam.transform.position = playerCamPos.position;
            cam.transform.rotation = playerCamPos.rotation;
        }
    }
    public IEnumerator nextTurn()
    {
        turnEnded = true;
        yield return new WaitForSeconds(1f);

        if(turnState == STATE.PLAYERTURN)
        {
            turnState = STATE.ENEMYTURN;
        }
        else
        {
            turnState = STATE.PLAYERTURN;
        }
        turnEnded = false;
    }
}
