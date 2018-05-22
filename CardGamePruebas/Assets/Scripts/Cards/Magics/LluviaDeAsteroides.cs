using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LluviaDeAsteroides : MagicController
{
  
    public override bool CanActiveEffect(int aIdFloor)
    {
        
        if (aIdFloor!=4&&aIdFloor!=20)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public override void ActiveEffect(int aIdFloor, int aIdCard)
    {
        MatchController.instance.playerController.ShowCard(MatchController.instance.playerController.cards[aIdCard].TypeCard, aIdCard);
        if (MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor)!=-1&& MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor)].playerOwner!=MatchController.instance.GetPlayerNumber())
        {
            MatchController.instance.playerController.HitMonster(-1, MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor), MatchController.instance.playerController.cards[aIdCard].attack);
        }
        for (int i = 1; i < 5; i++)
        {
            if (aIdFloor+(i*6)<BoardController.instance.groundList.Count&&BoardController.instance.GetDistanceToObject(BoardController.instance.groundList[aIdFloor + ((i-1)* 6)].transform, BoardController.instance.groundList[aIdFloor + (i * 6)].transform)==1) 
            {
                if (MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor + (i * 6)) != -1 && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor + (i * 6))].playerOwner != MatchController.instance.GetPlayerNumber())
                {
                    MatchController.instance.playerController.HitMonster(-1, MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor + (i * 6)), MatchController.instance.playerController.cards[aIdCard].attack);
                }
            }
           if (aIdFloor - (i * 6) >=0 && BoardController.instance.GetDistanceToObject(BoardController.instance.groundList[aIdFloor - ((i - 1) * 6)].transform, BoardController.instance.groundList[aIdFloor - (i * 6)].transform) == 1)
            {
                if (MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor - (i * 6)) != -1 && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor - (i * 6))].playerOwner != MatchController.instance.GetPlayerNumber())
                {
                    MatchController.instance.playerController.HitMonster(-1, MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor - (i * 6)), MatchController.instance.playerController.cards[aIdCard].attack);
                }
            }
        }
      

    }

}
