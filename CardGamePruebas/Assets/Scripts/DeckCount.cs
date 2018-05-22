using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCount : MonoBehaviour {
    public int playerOwner;
    public Text txtCount;
    private void OnMouseOver()
    {
        txtCount.enabled = true;
        if (playerOwner==MatchController.instance.GetPlayerNumber())
        {
            txtCount.text=Dealer.instance.deck.Count.ToString();
        }
        else
        {          
            txtCount.text = Dealer.instance.countCardsInDeckEnemy.ToString();
        }
       
    }
    private void OnMouseExit()
    {
        txtCount.enabled = false;
    }
}
