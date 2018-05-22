using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolectorDeBasura : MagicController
{
	int indexMonster;
	public int seCostToDestroy=2;
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
		for (int i = 0; i < MatchController.instance.monstersInGame.Count; i++) {
			if(!MatchController.instance.monstersInGame[i].king && MatchController.instance.playerController.cards[MatchController.instance.monstersInGame[i].idCard].seCost<=seCostToDestroy)
			{
				MatchController.instance.playerController.AddMonsterToDestroy (MatchController.instance.monstersInGame[i].idSpawn);
			}
		}
		MatchController.instance.playerController.DestroyMonsters ();
	}
}
