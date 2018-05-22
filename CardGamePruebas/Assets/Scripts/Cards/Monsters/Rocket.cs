using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonsterController
{
    public override void EnemyHitMe(int aIdSpawnEnemy)
    {
        if (monsterMode==0 && playerOwner==MatchController.instance.GetPlayerNumber())
        {
            Dealer.instance.DrawCard();
        }
    }
}
