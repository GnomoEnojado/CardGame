using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragon : MonoBehaviour {
    MonsterController monsterController;
    public int damageEffect=2;
	// Use this for initialization
	void Start () {
        monsterController = GetComponent<MonsterController>();
        if (monsterController.playerOwner == MatchController.instance.GetPlayerNumber())
        {
            MatchController.instance.activatingCard = true;
        }
        else
        {
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (BoardController.instance.floorOver!=null && Input.GetMouseButtonDown(0))
        {
            if (MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor)!=-1)
            {
                int indexMonster = MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor);

                if (MatchController.instance.monstersInGame[indexMonster].playerOwner!=MatchController.instance.GetPlayerNumber())
                {
                    if (BoardController.instance.IsInNearFloors(monsterController.idFloor, BoardController.instance.floorOver.idFloor))
                    {
                        MatchController.instance.playerController.HitMonster(-1,indexMonster,damageEffect);
                        MatchController.instance.activatingCard = false;
                        this.enabled = false;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MatchController.instance.activatingCard = false;
            this.enabled = false;
        }
	}
}
