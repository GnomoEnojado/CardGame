﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaDeFuego : MonoBehaviour {

    TrapController trapController;
    MonsterController monsterDetected;
    float timer;
    bool trapActive;
    public int trapDamage=2;

    // Use this for initialization
    void Start()
    {
        trapController = GetComponent<TrapController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterDetected != null && monsterDetected.GetState() == 0)
        {
            MatchController.instance.activatingCard = true;
   
            trapController.ShowCard();

            trapActive = true;
        }
        if (trapActive)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                MatchController.instance.playerController.HitMonster(-1, MatchController.instance.GetIndexMonsterInGameListWithSpawn(monsterDetected.idSpawn), trapDamage);
                trapController.DestroyTrap();
            }
        }

    }



    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<MonsterController>() && col.GetComponent<MonsterController>().playerOwner != trapController.playerOwner)
        {
            monsterDetected = col.GetComponent<MonsterController>();
        }
    }
}
