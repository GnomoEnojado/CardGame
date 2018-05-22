using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimerosAuxilios : MagicController
{
    int indexMonster;
    public int hpEffect = 3;
    public override bool CanActiveEffect(int aIdFloor)
    {
        indexMonster = MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor);
        if (indexMonster >= 0)
        {
            if (MatchController.instance.monstersInGame[indexMonster].king && MatchController.instance.monstersInGame[indexMonster].playerOwner == MatchController.instance.GetPlayerNumber())
            {
                return true;
            }
            else
            {
                return false;
            }
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
        int hpLess = MatchController.instance.playerController.cards[MatchController.instance.monstersInGame[indexMonster].idCard].hp - MatchController.instance.monstersInGame[indexMonster].hp;
        if (hpLess >= hpEffect)
        {
            MatchController.instance.playerController.AddStatsMonster(indexMonster, 0, 0, hpEffect, 0);

        }
        else
        {
            MatchController.instance.playerController.AddStatsMonster(indexMonster, 0, 0, hpLess, 0);
        }
    }
}
