using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadaEncantada : MonoBehaviour {

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
            if (MatchController.instance.GetIndexTrapsInGameListWithFloor(BoardController.instance.floorOver.idFloor) != -1)
            {
                int indexTrap = MatchController.instance.GetIndexTrapsInGameListWithFloor(BoardController.instance.floorOver.idFloor);

                if (MatchController.instance.trapsInGame[indexTrap].playerOwner != MatchController.instance.GetPlayerNumber())
                {
                    MatchController.instance.playerController.DestroyTrap(indexTrap);
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
