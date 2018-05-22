using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CementeryShowCards : MonoBehaviour {
    public bool showCementery=false;
    public int playerOwner;

	// Update is called once per frame
	void Update () {
        if ((!MatchController.instance.activatingCard&&Input.GetMouseButtonDown(1) && showCementery)|| (MatchController.instance.draggingCard && showCementery))
        {
            CementeryController.instance.HideCardsInCementery();
            showCementery = false;
            CementeryController.instance.viewingCementeryPlayer = 0;
        }
        if (CementeryController.instance.viewingCementeryPlayer!=playerOwner && showCementery)
        {
            showCementery = false;
        }
	}
    private void OnMouseDown()
    {
        if (!showCementery)
        {
            List<int> cards = CementeryController.instance.GetCardInCementery(playerOwner, -1);
           
            if (cards.Count>0) {
                CementeryController.instance.ShowCardsInCementery(cards);
                CementeryController.instance.viewingCementeryPlayer = playerOwner;
                showCementery = true;
            }
        }
        
    }
}
