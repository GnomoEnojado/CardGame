using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croom : MonsterController
{
    int countTimesAttacked=0;
    public override void IHitEnemy(int aIdSpawnEnemy)
    {
        countTimesAttacked++;
        if (countTimesAttacked<2)
        {

            for (int i = 0; i < MatchController.instance.monstersInGame.Count; i++)
            {
                if (!MatchController.instance.monstersInGame[i].king && MatchController.instance.monstersInGame[i].playerOwner==playerOwner && MatchController.instance.monstersInGame[i].gameObject!=this.gameObject)
                {

                    return;
                }
            }

            canAttack = true;
        }
    }
    public override void ResetVariables()
    {
        base.ResetVariables();
        countTimesAttacked = 0;
    }
}
