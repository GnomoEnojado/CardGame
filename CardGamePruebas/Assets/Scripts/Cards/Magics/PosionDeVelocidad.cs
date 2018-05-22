using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionDeVelocidad : MagicController {

    int idSpawnMonster;
    int indexMonster;
	int turnActive=-1;
	int debuff;

	public override bool CanActiveEffect(int aIdFloor)
	{
		indexMonster = MatchController.instance.GetIndexMonsterInGameListWithFloor(aIdFloor);
		if (indexMonster >= 0)
		{
			if (!MatchController.instance.monstersInGame[indexMonster].king && MatchController.instance.monstersInGame[indexMonster].playerOwner == MatchController.instance.GetPlayerNumber())
			{
                idSpawnMonster = MatchController.instance.monstersInGame[indexMonster].idSpawn;
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
		MatchController.instance.playerController.AddStatsMonster(indexMonster,0, 0, 0,  MatchController.instance.playerController.cards[aIdCard].velocity);
		PosionDeVelocidad instance = Instantiate(MatchController.instance.playerController.cards[aIdCard].prefabCard,transform.position,Quaternion.identity).GetComponent<PosionDeVelocidad>();
		instance.SetMonsterBuffed(idSpawnMonster, MatchController.instance.GetTurnNumber(), MatchController.instance.playerController.cards[aIdCard].velocity);
	}
	private void Update()
	{
		if (MatchController.instance.GetTurnNumber()>turnActive && turnActive>=0)
		{
            if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster) != -1)
            {
                MatchController.instance.playerController.AddStatsMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster), 0, 0, 0, -debuff);
            }
            
			Destroy(this.gameObject);
		}
	}
	public void SetMonsterBuffed(int aIdSpawnMonster, int aTurnActive, int aDebuff)
	{
        idSpawnMonster = aIdSpawnMonster;
		turnActive = aTurnActive;
		debuff = aDebuff;
	}
}
