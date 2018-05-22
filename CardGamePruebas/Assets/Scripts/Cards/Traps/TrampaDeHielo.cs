using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaDeHielo : MonoBehaviour {

    TrapController trapController;
    public MonsterController monsterDetected;
    public float timer;
    public bool trapActive;
    public int idSpawnMonsterFrozen=-1;
    public int turnActiveTrap=-1;

    // Use this for initialization
    void Start()
    {
        trapController = GetComponent<TrapController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (idSpawnMonsterFrozen==-1) {

            if (monsterDetected != null && monsterDetected.GetState() == 0)
            {
                MatchController.instance.activatingCard = true;

                trapController.ShowCard();
                monsterDetected.frozen = true;
                trapActive = true;
            }
         
            if (trapActive)
            {
                timer += Time.deltaTime;
                if (timer >= 2)
                {                    
                    TrampaDeHielo instance = Instantiate(MatchController.instance.playerController.cards[trapController.idCard].prefabCard, new Vector3(200,200,200), Quaternion.identity).GetComponent<TrampaDeHielo>();
                    instance.SetFrozenMonster(monsterDetected.idSpawn, MatchController.instance.GetTurnNumber());
                    trapController.DestroyTrap();

                }
            }
        }
        else
        {
            if (MatchController.instance.GetTurnNumber() > turnActiveTrap+1)
            {
                if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonsterFrozen)!=-1)
                {
                    MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawnMonsterFrozen)].frozen = false;
                }
                trapController.goToCementery = false;
                trapController.DestroyTrap();
            }
        }
    }

    public void SetFrozenMonster(int aIdSpawnMonster, int aTurnActive)
    {
        idSpawnMonsterFrozen = aIdSpawnMonster;
        turnActiveTrap = aTurnActive;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<MonsterController>() && col.GetComponent<MonsterController>().playerOwner != trapController.playerOwner)
        {
            monsterDetected = col.GetComponent<MonsterController>();
        }
    }
}
