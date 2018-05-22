using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggMagic : Dragg {
    MagicController magicController;
 

    new void  Start()
    {
        base.Start();
  
        magicController = card.prefabCard.GetComponent<MagicController>();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        MatchController.instance.idDraggingCard = -1;
        canOrder = true;
        draggingInWorld = false;
        MatchController.instance.draggingCard = false;

        if (BoardController.instance.floorOver != null)
        {
            if (MatchController.instance.playerSE-card.seCost>=0)
            {
                if (magicController.CanActiveEffect(BoardController.instance.floorOver.idFloor))
                {
                    HandController.instance.RemoveCardFromPlayerHand(GetComponent<CardController>().idSpawnCard);
                    MatchController.instance.playerController.RemoveEnemyCard(GetComponent<CardController>().idSpawnCard);
                   
                    MatchController.instance.playerSE -= card.seCost;
                    magicController.ActiveEffect(BoardController.instance.floorOver.idFloor,card.Id);
					MatchController.instance.playerController.AddCardToCementery (card.Id,MatchController.instance.GetPlayerNumber());
                   
                    Destroy(this.gameObject);
                }

                

            }
         
        }
   
        MatchController.instance.playerController.EnemyDraggingCard(GetComponent<CardController>().idSpawnCard, 0, -1);
    }
}
