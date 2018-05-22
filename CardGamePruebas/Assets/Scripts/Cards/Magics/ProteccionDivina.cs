using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteccionDivina : MagicController
{
    int idSpawnMonster;
    int indexMonster;
    int turnActive = -1;
    int hpLocked;

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
        MatchController.instance.playerController.SetMonsterInmune(indexMonster, 1);
        ProteccionDivina instance = Instantiate(MatchController.instance.playerController.cards[aIdCard].prefabCard, transform.position, Quaternion.identity).GetComponent<ProteccionDivina>();
        instance.SetMonsterBuffed(idSpawnMonster, MatchController.instance.GetTurnNumber());
    }
    private void Update()
    {
        if (MatchController.instance.GetTurnNumber() > turnActive && turnActive >= 0)
        {
            if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster)!=-1)
            {
                MatchController.instance.playerController.SetMonsterInmune(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonster), 0);
            }
            Destroy(this.gameObject);
        }
    }
    public void SetMonsterBuffed(int aIdSpawnMonster, int aTurnActive)
    {
        idSpawnMonster = aIdSpawnMonster;
        turnActive = aTurnActive;
   
    }
}
