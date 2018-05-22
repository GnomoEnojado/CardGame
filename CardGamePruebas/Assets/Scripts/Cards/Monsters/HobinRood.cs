using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobinRood : MonsterController {

    public override void IHitEnemy(int aIdSpawnEnemy)
    {
        if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawnEnemy)==-1 || (MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawnEnemy) !=-1 && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawnEnemy)].hp<=0))
        {
            canAttack = true;
        }
    }
}
