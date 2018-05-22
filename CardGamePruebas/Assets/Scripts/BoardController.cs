using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
	public GameObject floor;
	public GameObject baseField;
	public GameObject deck;
	public GameObject cementery;
	public Floor floorOver;

	private float offSetX = 1.25f;
	private float offSetZ = 2.5f;
    public List<Floor> groundList;
	public static BoardController instance = null;
	public List<Floor> listPath;
	public List<Floor> listPosibleFloors;
	private int velocityToUse;
	public List<MonsterController> monstersCanAttack = new List<MonsterController>();
   


    private void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	// Use this for initialization
	void Start ()
	{
		CreateBattleField ();
	}

	private void Update ()
	{
		OverFloor ();
	}

	private void CreateBattleField ()
	{
		int idCount = 0;
		Instantiate (baseField, new Vector3 (5, 6, 0), Quaternion.identity);
		Instantiate (deck, new Vector3 (9, 7, -10.5f), Quaternion.identity).GetComponent<DeckCount>().playerOwner=1;
		Instantiate (cementery, new Vector3 (1, 6.5f, -10.5f), Quaternion.identity).GetComponent<CementeryShowCards>().playerOwner = 1;
        Instantiate(deck, new Vector3(1f, 7, 10), Quaternion.identity).GetComponent<DeckCount>().playerOwner = 2;
        Instantiate (cementery, new Vector3 (9f, 6.5f, 10), Quaternion.Euler (0, 180, 0)).GetComponent<CementeryShowCards>().playerOwner = 2;

        for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				Floor floorInstance = Instantiate (floor, new Vector3 ((i * offSetX) + offSetX * j, 6.5f, (j * offSetZ) - offSetZ * i), Quaternion.identity).GetComponent<Floor> ();
				floorInstance.idFloor = idCount;
				groundList.Add (floorInstance);
				idCount++;

			}
		}

	}

	public void ShowFloorCanMove (int idFloorMonster, int aPlayerOwner, int aVelocity)
	{
		ClearListPosibleFloors ();


		//groundList[4].transform floor rey player 2 ---- groundList[20] floor rey player 1
		// resto 1 porque me puedo mover hasta el cuadrado anterior al del rey que es al que se le esta calculando la distancia.
		if (aPlayerOwner == 1) {
			velocityToUse = GetDistanceToObject ( groundList [idFloorMonster].transform, groundList [4].transform) - 1;
	
		} else {
			velocityToUse = GetDistanceToObject (groundList [idFloorMonster].transform, groundList [20].transform) - 1;
		}
		// si la distancia del monstruo es menor o igual a la velocidad del monstruo, la velocidad a usar es la misma que la velocidad maxima que puede moverse el monstruo
		// si la distancia del monstruo es mayor a la velocidad maxima del monstruo, la velocidad a usar es la de la distancia hasta el rey ya que mas de eso no se puede mover.

		if (aVelocity <= velocityToUse) {

			velocityToUse = aVelocity;
		}
	
		for (int i = 0; i < groundList.Count; i++) {
			if (groundList [i].idFloor != 4 && groundList [i].idFloor != 20) {

				if (aPlayerOwner == 1) {
					if ((idFloorMonster - groundList [i].idFloor) == 5 || (idFloorMonster - groundList [i].idFloor) == -1) {

						float dist = GetDistanceToObject (groundList [idFloorMonster].transform, groundList [i].transform);

						if (dist == 1 && !groundList [i].GetOcupateMonsterState()) {

							groundList [i].GetComponent<MeshRenderer> ().material.color = Color.yellow;
							listPosibleFloors.Add (groundList [i]);
						}

					}

				} else if (aPlayerOwner == 2) {

					if ((idFloorMonster - groundList [i].idFloor) == -5 || (idFloorMonster - groundList [i].idFloor) == 1) {

						float dist = GetDistanceToObject (groundList [idFloorMonster].transform, groundList [i].transform);

						if (dist == 1 && !groundList [i].GetOcupateMonsterState()) {

							groundList [i].GetComponent<MeshRenderer> ().material.color = Color.yellow;
							listPosibleFloors.Add (groundList [i]);
						}

					}
				
				}
			} 
			
		}
	}


	public List<MonsterController> GetMonstersCanAttack(int idFloorMonster, int aPlayerOwner,int aTypeAttack)
	{
		
		monstersCanAttack.Clear ();
		if(aTypeAttack==3)
		{
			

			if(groundList[idFloorMonster-1].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-1)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster-1].transform);
				if(dist==1)
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-1)]);
				}
			
			
			}
			if(groundList[idFloorMonster+1].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+1)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster+1].transform);
				if (dist == 1) 
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+1)]);
				}

			
			}
			if(groundList[idFloorMonster-5].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-5)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster-5].transform);
				if (dist == 1) 
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-5)]);
				}

			}
			if(groundList[idFloorMonster+5].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+5)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster+5].transform);
				if (dist == 1) 
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+5)]);
				}

			}
			if(groundList[idFloorMonster-6].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-6)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster-6].transform);
				if (dist == 1) 
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster-6)]);
				}
		
			}
			if(groundList[idFloorMonster+6].isOcupateByMonster && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+6)].playerOwner!=aPlayerOwner)
			{
				float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster+6].transform);
				if (dist == 1) 
				{
					monstersCanAttack.Add (MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster+6)]);
				}

			}
		}
		else
		{
		for (int i = 0; i < groundList.Count; i++) {
			//if (groundList [i].idFloor != 4 && groundList [i].idFloor != 20) {

				if(aPlayerOwner==1)
				{
                    if ((idFloorMonster - groundList[i].idFloor) == 5 || (idFloorMonster - groundList[i].idFloor) == -1)
                    {

                        for (int j = 0; j < aTypeAttack; j++)
                        {
                            if (i + j < groundList.Count && groundList[i + j].GetOcupateMonsterState())
                            {
                                //groundList[i + j].GetComponent<MeshRenderer>().material.color = Color.magenta;
                                MonsterController monsterToAdd = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(i + j)];
                                if (monsterToAdd!=null && monsterToAdd.playerOwner != aPlayerOwner && !IsInMonsterCanAttackList(monsterToAdd.gameObject))
                                {
                                    monstersCanAttack.Add(monsterToAdd);
                                }
                            }

                            if (i - (j * 5) >= 0 && groundList[i - (j * 5)].GetOcupateMonsterState())
                            {
                                //groundList[i - (j * 5)].GetComponent<MeshRenderer>().material.color = Color.magenta;

                                MonsterController monsterToAdd = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(i - (j * 5))];
                                if (monsterToAdd!=null && monsterToAdd.playerOwner != aPlayerOwner && !IsInMonsterCanAttackList(monsterToAdd.gameObject))
                                {
                                    monstersCanAttack.Add(monsterToAdd);
                                }
                            }

                        }

                        /*float dist = GetDistanceToObject (groundList [idFloorMonster].transform, groundList [i].transform);

						if (dist <= aTypeAttack) {

							groundList [i].GetComponent<MeshRenderer> ().material.color = Color.magenta;
							listPosibleFloors.Add (groundList [i]);
						}*/

                    }
                }
                else if(aPlayerOwner==2)
				{

					if ((idFloorMonster - groundList [i].idFloor) == -5 || (idFloorMonster - groundList [i].idFloor) == 1) {

						for (int j=0; j < aTypeAttack; j++)
						{
							if (i-j >= 0 && groundList[i - j].GetOcupateMonsterState())
							{
								//groundList[i - j].GetComponent<MeshRenderer>().material.color = Color.magenta;
								MonsterController monsterToAdd = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(i - j)];
								if(monsterToAdd!=null && monsterToAdd.playerOwner != aPlayerOwner && !IsInMonsterCanAttackList(monsterToAdd.gameObject))
								{
									monstersCanAttack.Add (monsterToAdd);
								}								
							}

							if (i+(j*5)<groundList.Count && groundList[i + (j * 5)].GetOcupateMonsterState()) 
							{
								//groundList[i + (j * 5)].GetComponent<MeshRenderer>().material.color = Color.magenta;
					
								MonsterController monsterToAdd = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(i + (j * 5))];
								if(monsterToAdd!=null && monsterToAdd.playerOwner != aPlayerOwner && !IsInMonsterCanAttackList(monsterToAdd.gameObject))
								{
									monstersCanAttack.Add (monsterToAdd);
								}
							}

						}

						/*float dist = GetDistanceToObject (groundList [idFloorMonster].transform, groundList [i].transform);

						if (dist <= aTypeAttack) {

							groundList [i].GetComponent<MeshRenderer> ().material.color = Color.magenta;
							listPosibleFloors.Add (groundList [i]);
						}*/

					}
				}
			

			//} 

		}
		}
		return monstersCanAttack;

	}
	bool IsInMonsterCanAttackList(GameObject aObject)
	{
		for (int i = 0; i < monstersCanAttack.Count; i++) {
			if (monstersCanAttack[i].gameObject==aObject.gameObject) {
				return true;
			}
		}
		return false;
	}
	public int GetDistanceToObject (Transform aStartObject, Transform aEndObject)
	{
		float distance = Vector3.Distance (aEndObject.position, aStartObject.position);
		int roundDist = (int)Mathf.Round (distance);
     
		// valores de distancia// 
		//2 (1 cuadrado) 5y6 (2 cuadrados) 8 (3 caudrados) 10 y 11(4 cuadrados) 13 (5 cuadrados) 14y15 (6 cuadrados)
		if (roundDist >= 2 && roundDist < 5) {
			return 1;
		} else if (roundDist >= 5 && roundDist < 8) {
			return 2;
		} else if (roundDist >= 8 && roundDist < 10) {
			return 3;
		} else if (roundDist >= 10 && roundDist < 13) {
			return 4;
		} else if (roundDist >= 13 && roundDist < 15) {
			return 5;
		} else if (roundDist >= 15) {
			return 6;
		} else {
			return roundDist = 0;
		}
	}

	public bool IsInPosibleFloors (Floor aFloor)
	{
		for (int i = 0; i < listPosibleFloors.Count; i++) {
			if (listPosibleFloors [i].gameObject == aFloor.gameObject) {
				return true;
			}
		}
		return false;
	}

	void OverFloor ()
	{
		RaycastHit[] raycastHit;
		raycastHit = Physics.RaycastAll (Camera.main.ScreenPointToRay (Input.mousePosition), 100.0f);
		for (int i = 0; i < raycastHit.Length; i++) {
			if (raycastHit [i].collider.GetComponent<Floor> ()) {
				if (floorOver == null) {
					floorOver = raycastHit [i].collider.GetComponent<Floor> ();
					floorOver.PlayerOverThis ();
				}
				if (floorOver != null && raycastHit [i].collider.gameObject != floorOver.gameObject) {
					floorOver.PlayerNotOverThis ();
					floorOver = raycastHit [i].collider.GetComponent<Floor> ();
					floorOver.PlayerOverThis ();

				}
				return;
			}
			if (floorOver != null) {
				floorOver.PlayerNotOverThis ();
				floorOver = null;
			}

		}
	}

	public void ClearListPosibleFloors ()
	{
		for (int i = 0; i < listPosibleFloors.Count; i++) {
			listPosibleFloors [i].GetComponent<MeshRenderer> ().material.color = Color.white;
		}
		listPosibleFloors.Clear ();
	}
    public void SelectFloorMonsterSelected(int aIdFloor)
    {
        groundList[aIdFloor].GetComponent<MeshRenderer>().material.color = Color.magenta;
    }
    public void DeselectFloorMonsterSelected(int aIdFloor)
    {
        if (aIdFloor!=-1) {
            groundList[aIdFloor].GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
    public bool IsInNearFloors(int aIdFloorMonster, int aIdFloorMonsterToCheck)
    {
        if (Mathf.Abs(aIdFloorMonster-aIdFloorMonsterToCheck)==1 || Mathf.Abs(aIdFloorMonster - aIdFloorMonsterToCheck) == 5 || Mathf.Abs(aIdFloorMonster - aIdFloorMonsterToCheck) == 6)
        {
            float dist = GetDistanceToObject(groundList[aIdFloorMonster].transform, groundList[aIdFloorMonsterToCheck].transform);
            if (dist==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void GetMonstersInNearFloors(int idFloorMonster,int aPlayerOwner,ref List<int> aMonsters)
    {
        aMonsters.Clear();

        if (idFloorMonster - 1>=0 && groundList[idFloorMonster - 1].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 1)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 1)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster - 1].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 1)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 1)].idSpawn );
            }


        }
        if (idFloorMonster + 1<groundList.Count && groundList[idFloorMonster + 1].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 1)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 1)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster + 1].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 1)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 1)].idSpawn);
            }


        }
        if (idFloorMonster - 5>=0 && groundList[idFloorMonster - 5].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 5)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 5)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster - 5].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 5)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 5)].idSpawn);
            }

        }
        if (idFloorMonster + 5<groundList.Count && groundList[idFloorMonster + 5].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster +5)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 5)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster + 5].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 5)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 5)].idSpawn);
            }

        }
        if (idFloorMonster - 6>=0 && groundList[idFloorMonster - 6].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 6)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 6)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster - 6].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 6)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster - 6)].idSpawn);
            }

        }
        if (idFloorMonster + 6<groundList.Count && groundList[idFloorMonster + 6].isOcupateByMonster && !MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster +6)].king && MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 6)].playerOwner == aPlayerOwner)
        {
            float dist = GetDistanceToObject(groundList[idFloorMonster].transform, groundList[idFloorMonster + 6].transform);
            if (dist == 1 && !IsInMonstersInNearFloorsList(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 6)].idSpawn, ref aMonsters))
            {
                aMonsters.Add(MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(idFloorMonster + 6)].idSpawn);
            }

        }

    }
    bool IsInMonstersInNearFloorsList(int aIdSpawn,ref List<int> aMonsters )
    {
        for (int i = 0; i < aMonsters.Count; i++)
        {
            if (aMonsters[i]==aIdSpawn)
            {
                return true;
            }
        }
        return false;
    }
}
