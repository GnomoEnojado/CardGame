using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueDeLasSombras : MagicController
{
	public List<int> idSpawnMonster=null;
	int turnActive=-1;
	int debuff;
	public int seCostToBuff=2;
	bool activedEffect;

	public override bool CanActiveEffect(int aIdFloor)
	{
		int indexMonsterActive = MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor);
		if (indexMonsterActive >= 0)
		{
			if (MatchController.instance.monstersInGame[indexMonsterActive].king && MatchController.instance.monstersInGame[indexMonsterActive].playerOwner == MatchController.instance.GetPlayerNumber())
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
		if(turnActive==-1)
		{
		AtaqueDeLasSombras instance = Instantiate(MatchController.instance.playerController.cards[aIdCard].prefabCard,transform.position,Quaternion.identity).GetComponent<AtaqueDeLasSombras>();
		instance.SetMonsterBuffed(MatchController.instance.GetTurnNumber(), MatchController.instance.playerController.cards[aIdCard].attack);
		instance.ActiveEffect (aIdFloor,aIdCard);
		}
		else if(!activedEffect)
		{
			MatchController.instance.playerController.ShowCard(MatchController.instance.playerController.cards[aIdCard].TypeCard, aIdCard);
			for (int i = 0; i < MatchController.instance.monstersInGame.Count; i++) {
				if(!MatchController.instance.monstersInGame[i].king &&MatchController.instance.monstersInGame[i].playerOwner == MatchController.instance.GetPlayerNumber() &&MatchController.instance.playerController.cards[MatchController.instance.monstersInGame[i].idCard].seCost<=seCostToBuff)
			{
				MatchController.instance.playerController.AddStatsMonster(i, MatchController.instance.playerController.cards[aIdCard].attack, 0, 0, 0);
				idSpawnMonster.Add (MatchController.instance.monstersInGame[i].idSpawn);
				
			}
		}
			activedEffect = true;
	}
	}

	private void Update()
	{
		if (MatchController.instance.GetTurnNumber()>turnActive && turnActive>=0 &&activedEffect)
		{
			for (int i = 0; i < idSpawnMonster.Count; i++) 
			{
				if(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster[i])>=0)
				{
					MatchController.instance.playerController.AddStatsMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster[i]), -debuff, 0, 0, 0);
				}

			}
			Destroy(this.gameObject);
		}
	}
	public void SetMonsterBuffed( int aTurnActive, int aDebuff)
	{
		turnActive = aTurnActive;
		debuff = aDebuff;
	}
}
