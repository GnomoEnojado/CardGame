using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ilusionista : MonoBehaviour {

    MonsterController monsterController;

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
                    monsterController.attack = MatchController.instance.monstersInGame[indexMonster].attack;
                    monsterController.hp = MatchController.instance.monstersInGame[indexMonster].hp;
                    monsterController.defense = MatchController.instance.monstersInGame[indexMonster].defense;
                    monsterController.velocity = MatchController.instance.monstersInGame[indexMonster].velocity;
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
