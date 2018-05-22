using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clerigo : MonoBehaviour {

    MonsterController monsterController;
    public int hpEffect = 2;
    // Use this for initialization
    void Start()
    {
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
    void Update()
    {
        if (BoardController.instance.floorOver != null && Input.GetMouseButtonDown(0))
        {
            if (MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor) != -1)
            {
                int indexMonster = MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor);

                if (MatchController.instance.monstersInGame[indexMonster].playerOwner == MatchController.instance.GetPlayerNumber())
                {
                    int hpLess = MatchController.instance.playerController.cards[MatchController.instance.monstersInGame[indexMonster].idCard].hp - MatchController.instance.monstersInGame[indexMonster].hp;
                    if (hpLess >= hpEffect)
                    {
                        MatchController.instance.playerController.AddStatsMonster(indexMonster, 0, 0, hpEffect, 0);
                    
                    }
                    else
                    {
                        MatchController.instance.playerController.AddStatsMonster(indexMonster, 0, 0, hpLess, 0);
                    }
                    MatchController.instance.activatingCard = false;
                    this.enabled = false;
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
