using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiderDeGuerra : MonoBehaviour {
    public int buff=1;
    MonsterController monsterController;
    public List<int> nearMonsters;
    public List<int> monstersBuffed;
    // Use this for initialization
    void Start () {
        monsterController = GetComponent<MonsterController>();
    
    }
	
	// Update is called once per frame
	void Update () {
        if (monsterController.playerOwner==MatchController.instance.GetPlayerNumber()) {
            BoardController.instance.GetMonstersInNearFloors(monsterController.idFloor, monsterController.playerOwner,ref nearMonsters);
            MonstersBuffedInList();
            ControlNearMonstersInBuffedList();
        }
    }
    void MonstersBuffedInList()
    {
           
            for (int i = 0; i < nearMonsters.Count; i++)
            {
                if (!IsInBuffedList(nearMonsters[i]))
                {
                if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(nearMonsters[i])!=-1)
                {
                    MatchController.instance.playerController.AddStatsMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(nearMonsters[i]), buff, 0, 0, 0);
                    monstersBuffed.Add(nearMonsters[i]);
                }
      
                }
            }
        
    }
    void ControlNearMonstersInBuffedList()
    { 
            for (int i = 0; i < monstersBuffed.Count; i++)
            {
                if (!IsInNearList(monstersBuffed[i]))
                {
                if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersBuffed[i])!=-1)
                {
                    MatchController.instance.playerController.AddStatsMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersBuffed[i]), -buff, 0, 0, 0);
                    monstersBuffed.RemoveAt(i);
                }
                   
                }
            }
        
    }
    bool IsInBuffedList(int aIdSpawn)
    {
        for (int i = 0; i < monstersBuffed.Count; i++)
        {
            if (aIdSpawn == monstersBuffed[i])
            {
                return true;
            }
        }
        return false;
    }
    bool IsInNearList(int aIdSpawn)
    {
        for (int i = 0; i < nearMonsters.Count; i++)
        {
            if (aIdSpawn == nearMonsters[i])
            {
                return true;
            }
        }
        return false;
    }

}
