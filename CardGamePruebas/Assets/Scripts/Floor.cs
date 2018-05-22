using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    MeshRenderer mesh;
    public bool isOcupateByMonster;
    private int playerFloorNumber;
    public int spawnForPlayerNumber;
    public bool haveTrap;

    public int idFloor;
    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Start () {
       
        if (transform.position.z > 0 && transform.position.z < 10)
        {
            spawnForPlayerNumber = 2;
        }
        else if (transform.position.z < 0 && transform.position.z > -10)
        {
            spawnForPlayerNumber = 1;
        }
        else if(transform.position.z == 0)
        {
            spawnForPlayerNumber = 0;
        }
        else
        {
            spawnForPlayerNumber = -1;
        }
    }

    public void PlayerOverThis()
    {
        
            //sacar este if cuando no se usen los colores o cuando use los colores en serio xD
            transform.localScale = new Vector3(2.3f, 0.1f, 2.3f);
            if (mesh.material.color != Color.yellow)
            {
            if (MatchController.instance.draggingCard)
            {
                if (MatchController.instance.playerPlaying == spawnForPlayerNumber && !isOcupateByMonster)
                {
                    mesh.material.color = Color.green;
                }
                else
                {
                    mesh.material.color = Color.red;
                }
            }

        }
        
    }
    public void PlayerNotOverThis()
    {
        transform.localScale = new Vector3(2f, 0.1f, 2f);
        //sacar este if cuando no se usen los colores o cuando use los colores en serio xD
        if (mesh.material.color != Color.yellow)
        {
            mesh.material.color = Color.white;
        }
            

    }
    public void SetOcupateState(bool aState, int aTypeCard)
    {
        if (aTypeCard==0)
        {
            isOcupateByMonster = aState;
        }
        else if (aTypeCard==1)
        {
            haveTrap = aState;
        }
    }
    public bool GetOcupateMonsterState()
    {
        return isOcupateByMonster;
    }
    public int playerFloor()
    {
        return spawnForPlayerNumber;
    }

}
