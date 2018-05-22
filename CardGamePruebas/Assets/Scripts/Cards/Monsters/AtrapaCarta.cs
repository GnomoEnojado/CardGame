using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrapaCarta : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GetComponent<MonsterController>().playerOwner==MatchController.instance.GetPlayerNumber())
        {
            Dealer.instance.DrawCard();
        }
    }

}
