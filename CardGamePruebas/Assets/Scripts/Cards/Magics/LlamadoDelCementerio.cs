using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LlamadoDelCementerio : MagicController
{
    int idFloorEffect = -1;

    private void Update()
    {
        if (idFloorEffect!=-1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                List<RaycastResult> results = CMainCanvas.Inst.GetGraphicsElements();
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.GetComponent<CardController>() && results[i].gameObject.GetComponent<CardController>().GetDontDestroyCard()==true)
                    {                        
                       MatchController.instance.playerController.CmdCreateMonster(results[i].gameObject.GetComponent<CardController>().card.Id, idFloorEffect, MatchController.instance.GetPlayerNumber());
						CementeryController.instance.RemoveCardFromCementery (results[i].gameObject.GetComponent<CardController>().card.Id,MatchController.instance.GetPlayerNumber());
						CementeryController.instance.HideCardsInCementery();
                       MatchController.instance.activatingCard = false;
                    }
                }
            }
      
        }
    }

    public override bool CanActiveEffect(int aIdFloor)
	{
        List<int> cards = CementeryController.instance.GetCardInCementery(MatchController.instance.GetPlayerNumber(), 0);
        if (cards.Count>0 && !BoardController.instance.groundList[aIdFloor].isOcupateByMonster && BoardController.instance.groundList[aIdFloor].playerFloor()==MatchController.instance.GetPlayerNumber())
		{
			return true;
		}
		else
		{
			return false;
		}

	}
	//idcard es el id de la carta magica
	public override void ActiveEffect(int aIdFloor, int aIdCard)
	{
		MatchController.instance.playerController.ShowCard(MatchController.instance.playerController.cards[aIdCard].TypeCard, aIdCard);

		MatchController.instance.activatingCard = true;

        List<int> cards = CementeryController.instance.GetCardInCementery(MatchController.instance.GetPlayerNumber(), 0);

        CementeryController.instance.ShowCardsInCementery(cards);
           
        LlamadoDelCementerio instance = Instantiate(MatchController.instance.playerController.cards[aIdCard].prefabCard, transform.position, Quaternion.identity).GetComponent<LlamadoDelCementerio>();
        instance.SetIdFloor(aIdFloor);
    }
    public void SetIdFloor(int aIdFloor)
    {
        idFloorEffect = aIdFloor;
    }
}
