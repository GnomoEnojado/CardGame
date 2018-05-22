using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianEspiritual : MonoBehaviour {
    MonsterController monsterController;
	// Use this for initialization
	void Start () {
        monsterController = GetComponent<MonsterController>();
	}

    private void OnDestroy()
    {
        if (MatchController.instance.GetPlayerNumber() == monsterController.playerOwner && monsterController.monsterMode==0)
        {
            List<int> monstersInCementery = CementeryController.instance.GetCardInCementery(monsterController.playerOwner, 0);
            if (monstersInCementery.Count>0)
            {
                int monster = Random.Range(0, monstersInCementery.Count);
                MatchController.instance.playerController.CmdCreateMonster(monstersInCementery[monster],monsterController.idFloor,monsterController.playerOwner);
				CementeryController.instance.RemoveCardFromCementery (monster,monsterController.playerOwner);
            }
        }
    }
}
