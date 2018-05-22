using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxeadorEnAscenso : MonoBehaviour {

    MonsterController monsterController;

    // Use this for initialization
    void Start()
    {
        monsterController = GetComponent<MonsterController>();
        //usar este if else para controlar que el efecto solo se corra en el monstruo del 
        //jugador que lo invoco ya que es el quien tiene que seleccionar a quien activarlo y no el 
        //otro jugador
        if (monsterController.playerOwner==MatchController.instance.GetPlayerNumber()) {
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

                if (MatchController.instance.monstersInGame[indexMonster].playerOwner != MatchController.instance.GetPlayerNumber())
                {
                    MatchController.instance.playerController.SetModeMonster(MatchController.instance.monstersInGame[indexMonster].idSpawn,1);
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
