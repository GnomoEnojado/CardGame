﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcaDeAtaque : MagicController
{
    int indexMonster;

    public override bool CanActiveEffect(int aIdFloor)
    {
        indexMonster = MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor);
        if (indexMonster >= 0)
        {
            if (!MatchController.instance.monstersInGame[indexMonster].king && MatchController.instance.monstersInGame[indexMonster].playerOwner == MatchController.instance.GetPlayerNumber())
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
        MatchController.instance.playerController.AddStatsMonster(indexMonster, MatchController.instance.playerController.cards[aIdCard].attack,0,0,0);
    }
}
