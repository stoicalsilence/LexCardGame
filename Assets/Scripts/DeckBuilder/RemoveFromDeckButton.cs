using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromDeckButton : MonoBehaviour
{
   
    private void OnMouseEnter()
    {
        CardButton[] buttons = FindObjectsOfType<CardButton>(); //OPTIMIZATION: GET ALL CARDBUTTONS AT START NOT EVERYTIME ITS HOVERED OVER
        foreach(CardButton button in buttons)
        {
            button.removeFromDeckButtonHighlighted = true;
        }
    }

    private void OnMouseExit()
    {
        CardButton[] buttons = FindObjectsOfType<CardButton>();
        foreach (CardButton button in buttons)
        {
            button.removeFromDeckButtonHighlighted = false;
        }
    }
}
