using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    public Card[] cards;

    public int playerNumber;
    public List<GameObject> prefabCard;
    float timerMouseOnMonster = 0;
    public bool showingCard = false;
    public MonsterController monsterSelected;
    int idFloorMonsterSelected=-1;
    //esta variable es la carta que esta viendo el jugador en el momento (solo a efectos visuales en el jugador local)
    GameObject cardShowing;
    GameObject cardShowing2;
    //esta variable es la carta que se esta jugando en el momento (solo a efectos visuales para mostrarla en ambos jugadores) 
    public GameObject cardPlaying;
    public List<int> monstersToDestroy;
    public GameObject king;
    public bool winCondition;
    public bool loseCondition;

  


    private void Start()
    {

        if (isLocalPlayer)
        {
            cards = GameController.instance.gameCards;
            prefabCard = GameController.instance.prefabCard;
            
            if (isServer)
            {
                playerNumber = 1;

            }
            else
            {
                playerNumber = 2;

            }
            MatchController.instance.SetPlayer(this);

        }

    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (MatchController.instance.playerPlaying!=playerNumber && monsterSelected!=null)
            {
                BoardController.instance.DeselectFloorMonsterSelected(monsterSelected.idFloor);
                monsterSelected = null;
            }
            if (!MatchController.instance.activatingCard)
            {
                if (monsterSelected != null)
                {
                    BoardController.instance.SelectFloorMonsterSelected(monsterSelected.idFloor);
                    if (monsterSelected.canMove)
                    {
                        BoardController.instance.ShowFloorCanMove(monsterSelected.idFloor, monsterSelected.playerOwner, monsterSelected.remainingVelocity);
                        if (BoardController.instance.floorOver!=null && BoardController.instance.IsInPosibleFloors(BoardController.instance.floorOver))
                        {
                            Vector2 hotSpot = new Vector2(MatchController.instance.cursorTextures[2].width / 2, MatchController.instance.cursorTextures[1].height / 2);
                            Cursor.SetCursor(MatchController.instance.cursorTextures[2], hotSpot, CursorMode.Auto);
                        }
                        else
                        {
                            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                        }
                    }
                    else
                    {
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        BoardController.instance.ClearListPosibleFloors();
                    }
                    if (monsterSelected.canMove && !monsterSelected.frozen && monsterSelected.canMove && monsterSelected.GetState() == 0 && Input.GetMouseButtonDown(0) && BoardController.instance.floorOver != null)
                    {

                        if (BoardController.instance.IsInPosibleFloors(BoardController.instance.floorOver))
                        {
                            BoardController.instance.DeselectFloorMonsterSelected(monsterSelected.idFloor);
                            monsterSelected.SetFloorToMove(BoardController.instance.floorOver.idFloor);
                            if (isServer)
                            {
                                RpcMoveMonster(monsterSelected.idSpawn, BoardController.instance.floorOver.idFloor);
                            }
                            else
                            {
                                CmdMoveMonster(monsterSelected.idSpawn, BoardController.instance.floorOver.idFloor);
                            }
                        }
                    }
                }
                else
                {
                    if (idFloorMonsterSelected!=-1)
                    {
                        BoardController.instance.DeselectFloorMonsterSelected(idFloorMonsterSelected);
                        idFloorMonsterSelected = -1;
                    }
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    BoardController.instance.ClearListPosibleFloors();
                }
       
                if (BoardController.instance.floorOver != null)
                {
                    Floor floorOver = BoardController.instance.floorOver;
                    if (MatchController.instance.playerPlaying == playerNumber)
                    {
                        if (floorOver.isOcupateByMonster)
                        {
                            MonsterController monsterInFloor = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor)];
                            if (monsterInFloor.playerOwner != playerNumber)
                            {

                                if (monsterSelected != null && !monsterSelected.frozen && monsterSelected.canAttack && monsterSelected.monsterMode == 1 && monsterSelected.GetState() == 0)
                                {
                                    List<MonsterController> monstersCanAttack = BoardController.instance.GetMonstersCanAttack(monsterSelected.idFloor, playerNumber, monsterSelected.typeAttack);
                                    for (int i = 0; i < monstersCanAttack.Count; i++)
                                    {
                                        if (monstersCanAttack[i].gameObject == monsterInFloor.gameObject)
                                        {
                                            //si puedo atacar al monstruo seteo el cursor a la espada
                                            Vector2 hotSpot = new Vector2(MatchController.instance.cursorTextures[1].width / 2, MatchController.instance.cursorTextures[1].height / 2);
                                            Cursor.SetCursor(MatchController.instance.cursorTextures[1], hotSpot, CursorMode.Auto);

                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                Debug.Log("Presione sobre un monstruo enemigo");
                                                MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(monsterSelected.idSpawn)].canAttack = false;
                                                //ataco y dependiendo si soy el servidor o cliente notifico con cmd o rpc
                                                HitMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(monsterSelected.idSpawn), MatchController.instance.GetIndexMonsterInGameListWithSpawn(monsterInFloor.idSpawn), monsterSelected.attack);
                                                monsterSelected.AlreadyAttack();
                                                monsterSelected.IHitEnemy(monsterInFloor.idSpawn);
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    //si no puedo atacar al monstruo seteo el cursor a default
                                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                                }
                            }
                            else
                            {
                                //si no puedo atacar al monstruo seteo el cursor a default
                                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                            }
                            if (Input.GetMouseButtonDown(0) && monsterInFloor.playerOwner == playerNumber && !monsterInFloor.king)
                            {
                                Debug.Log("Presione sobre un monstruo propio");
                                if (monsterSelected!=null && monsterInFloor.gameObject==monsterSelected.gameObject)
                                {
                                    BoardController.instance.DeselectFloorMonsterSelected(monsterSelected.idFloor);
                                    monsterSelected = null;
                                    BoardController.instance.ClearListPosibleFloors();
                                   
                                }
                                else if (monsterSelected != null && monsterInFloor.gameObject != monsterSelected.gameObject)
                                {
                                    BoardController.instance.DeselectFloorMonsterSelected(monsterSelected.idFloor);
                                    monsterSelected = monsterInFloor;
                                    idFloorMonsterSelected = monsterInFloor.idFloor;
                                }
                                else if (monsterSelected == null)
                                {
                                    monsterSelected = monsterInFloor;
                                    idFloorMonsterSelected = monsterInFloor.idFloor;
                                }
                               

                            }
                        
                            if (Input.GetMouseButtonDown(1) && monsterInFloor.canChangeMode && !monsterInFloor.frozen && monsterInFloor.playerOwner == playerNumber && !monsterInFloor.king)
                            {
                                Debug.Log("Presione click derecho sobre un monstruo propio");
                                monsterInFloor.ChangeNextMonsterMode();
                                if (!isServer)
                                {
                                    CmdChangeModeMonster(monsterInFloor.idSpawn);
                                }
                                else
                                {
                                    RpcChangeModeMonster(monsterInFloor.idSpawn);
                                }
                            }
                        }
                    }
                    
                    //muestra la carta sobre la que tengo el mouse

                    if (floorOver.isOcupateByMonster || floorOver.haveTrap)
                    {
                        if (!MatchController.instance.draggingCard)
                        {
                            timerMouseOnMonster += Time.deltaTime;

                            if (timerMouseOnMonster > 0.5f && !showingCard)
                            {

                                //los typecard al igual que la variables que se repiten con id spawn playerowner, etc, pasarlas a una clase en comun a los monstruos y el resto de las cartas

                                if (floorOver.isOcupateByMonster)
                                {
                                    DestroyCardShow();
                                    cardShowing = Instantiate(prefabCard[0], HandController.instance.transform.position, Quaternion.identity);
                                    cardShowing.GetComponent<CardController>().card = cards[MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(floorOver.idFloor)].idCard];
                                    cardShowing.GetComponent<CardController>().DontDestroyCard();
                                    cardShowing.GetComponent<Dragg>().enabled = false;
                                }
                                if (floorOver.haveTrap && MatchController.instance.trapsInGame[MatchController.instance.GetIndexTrapsInGameListWithFloor(floorOver.idFloor)].playerOwner == playerNumber)
                                {
                                    if (floorOver.isOcupateByMonster)
                                    {
                                        DestroyCardShow2();
                                        cardShowing2 = Instantiate(prefabCard[1], HandController.instance.transform.position, Quaternion.identity);
                                        cardShowing2.GetComponent<CardController>().card = cards[MatchController.instance.trapsInGame[MatchController.instance.GetIndexTrapsInGameListWithFloor(floorOver.idFloor)].idCard];
                                        cardShowing2.GetComponent<CardController>().DontDestroyCard();
                                        cardShowing2.GetComponent<Dragg>().enabled = false;
                                    }
                                    else
                                    {
                                        DestroyCardShow();
                                        cardShowing = Instantiate(prefabCard[1], HandController.instance.transform.position, Quaternion.identity);
                                        cardShowing.GetComponent<CardController>().card = cards[MatchController.instance.trapsInGame[MatchController.instance.GetIndexTrapsInGameListWithFloor(floorOver.idFloor)].idCard];
                                        cardShowing.GetComponent<CardController>().DontDestroyCard();
                                        cardShowing.GetComponent<Dragg>().enabled = false;
                                    }
                                   
                                }
                                if (cardShowing!= null)
                                {
                                    cardShowing.transform.SetParent(CMainCanvas.Inst.transform);

                                    cardShowing.transform.localScale = new Vector3(1, 1, 1);
															
									Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(floorOver.transform.position);
									Vector2 WorldObject_ScreenPosition=new Vector2(
										((ViewportPosition.x*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x*0.5f)),
										((ViewportPosition.y*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y*0.5f)));
                                    cardShowing.transform.localRotation = Quaternion.Euler(0, 0, 0);
									WorldObject_ScreenPosition.x += 100;
									WorldObject_ScreenPosition.y += 50;
									cardShowing.transform.RT().anchoredPosition = WorldObject_ScreenPosition;
                                
                                }
                               

                                if (cardShowing2!=null)
                                {
                                    cardShowing2.transform.SetParent(CMainCanvas.Inst.transform);

                                    cardShowing2.transform.localScale = new Vector3(1, 1, 1);
									Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(floorOver.transform.position);
									Vector2 WorldObject_ScreenPosition=new Vector2(
										((ViewportPosition.x*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x*0.5f)),
										((ViewportPosition.y*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y*0.5f)));
									cardShowing2.transform.localRotation = Quaternion.Euler(0, 0, 0);
									WorldObject_ScreenPosition.x += 250;
									WorldObject_ScreenPosition.y += 50;
									cardShowing2.transform.RT().anchoredPosition = WorldObject_ScreenPosition;
                                }

                               

                                showingCard = true;

                            }
                        }
                    }
                    else
                    {
                        ResetShowCard();
                    }
                }
                else
                {
                    ResetShowCard();
                }
                if (Input.GetMouseButtonDown(1)&&!MatchController.instance.activatingCard && (BoardController.instance.floorOver == null || !BoardController.instance.floorOver.isOcupateByMonster || MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithFloor(BoardController.instance.floorOver.idFloor)].playerOwner!=playerNumber))
                {
                    if (monsterSelected != null)
                    {
                        BoardController.instance.DeselectFloorMonsterSelected(monsterSelected.idFloor);
                        monsterSelected = null;
                        BoardController.instance.ClearListPosibleFloors();
                        Debug.Log("Deseleccione mi monstruo");

                    }
                }


            }
        
          
        }
    }
    public void EndMonsterMove(MonsterController aMonster, int aRemainingVelocity)
    {
        if (monsterSelected != null && monsterSelected.gameObject == aMonster.gameObject)
        {
            BoardController.instance.ShowFloorCanMove(monsterSelected.idFloor, monsterSelected.playerOwner, aRemainingVelocity);
        }
    }
    void ResetShowCard()
    {
        timerMouseOnMonster = 0;
        showingCard = false;
        DestroyCardShow();
        DestroyCardShow2();

    }
    void DestroyCardShow()
    {
        if (cardShowing != null)
        {
            Destroy(cardShowing.gameObject);
        }
    }
    void DestroyCardShow2()
    {
        if (cardShowing2 != null)
        {
            Destroy(cardShowing2.gameObject);
        }
    }
    public MonsterController GetMonsterSelected()
    {
        return monsterSelected;
    }
    public void RemoveEnemyCard(int aIdSpawnCard)
    {
        if (isServer)
        {
            RpcRemoveEnemyCard(aIdSpawnCard);
        }
        else
        {
            CmdRemoveEnemyCard(aIdSpawnCard);
        }
    }

    [Command]
    public void CmdRemoveEnemyCard(int aIdSpawnCard)
    {
        HandController.instance.RemoveCardFromEnemyHand(aIdSpawnCard);
  
    }

    [ClientRpc]
    public void RpcRemoveEnemyCard(int aIdSpawnCard)
    {
        if (!isServer)
        {
            HandController.instance.RemoveCardFromEnemyHand(aIdSpawnCard);
        }
    }
    public void CreateEnemyCard(int aIdSpawnCard, int idCard)
	{
		if (isServer)
		{
			RpcCreateEnemyCard (aIdSpawnCard,idCard);
		}
		else
		{
			CmdCreateEnemyCard (aIdSpawnCard,idCard);
		}
	}

	[Command]
	public void CmdCreateEnemyCard(int aIdSpawnCard, int idCard)
	{
		Dealer.instance.DrawEnemyCard (aIdSpawnCard,idCard);
	}

	[ClientRpc]
	public void RpcCreateEnemyCard(int aIdSpawnCard, int idCard)
	{
		if (!isServer)
		{
			Dealer.instance.DrawEnemyCard (aIdSpawnCard,idCard);
		}
	}
	public void EnemyDraggingCard(int aIdSpawnCard,  int aState, int aIdFlor)
	{
		if (isServer)
		{
			RpcEnemyDraggingCard (aIdSpawnCard,aState,aIdFlor);
		}
		else
		{
			CmdEnemyDraggingCard (aIdSpawnCard,aState,aIdFlor);
		}
	}

	[Command]
	public void CmdEnemyDraggingCard(int aIdSpawnCard,int aState, int aIdFlor)
	{
		HandController.instance.EnemyDraggingCard(aIdSpawnCard,aState,aIdFlor);
	}

	[ClientRpc]
	public void RpcEnemyDraggingCard(int aIdSpawnCard,int aState, int aIdFlor)
	{
		if (!isServer)
		{
			HandController.instance.EnemyDraggingCard (aIdSpawnCard,aState,aIdFlor);
		}
	}
    public void DestroyEnemyCard(int aIdSpawnCard, int aIdFloor)
    {
        if (isServer)
        {
            RpcDestroyEnemyCard(aIdSpawnCard,aIdFloor);
        }
        else
        {
            CmdDestroyEnemyCard(aIdSpawnCard,aIdFloor);
        }
    }

    [Command]
    public void CmdDestroyEnemyCard(int aIdSpawnCard, int aIdFloor)
    {
        HandController.instance.DestroyEnemyCard(aIdSpawnCard,aIdFloor);
    }

    [ClientRpc]
    public void RpcDestroyEnemyCard(int aIdSpawnCard, int aIdFloor)
    {
        if (!isServer)
        {
            HandController.instance.DestroyEnemyCard(aIdSpawnCard,aIdFloor);
        }
    }
    public void EnemyLookingCard(int aIdSpawnCard,  int aState)
	{
		if (isServer)
		{
			RpcEnemyLookingCard (aIdSpawnCard,aState);
		}
		else
		{
			CmdEnemyLookingCard (aIdSpawnCard,aState);
		}
	}

	[Command]
	public void CmdEnemyLookingCard(int aIdSpawnCard,int aState)
	{
		HandController.instance.EnemyLookingCard(aIdSpawnCard,aState);
	}

	[ClientRpc]
	public void RpcEnemyLookingCard(int aIdSpawnCard,int aState)
	{
		if (!isServer)
		{
			HandController.instance.EnemyLookingCard (aIdSpawnCard,aState);
		}
	}
	public void HitMonster(int aMonsterAttacking, int aMonsterToDamage, int aDamage)
	{

		if (playerNumber == MatchController.instance.playerPlaying)
		{
			int aAccuracy = Random.Range(0, 100);
			MatchController.instance.monstersInGame[aMonsterToDamage].HitMonster(aMonsterAttacking, aDamage,aAccuracy);

			if (isServer)
			{
				RpcHitMonster(aMonsterAttacking, aMonsterToDamage, aDamage, aAccuracy);
			}
			else
			{
				CmdHitMonster(aMonsterAttacking, aMonsterToDamage, aDamage, aAccuracy);
			}
		}


	}
    [Command]
    public void CmdHitMonster(int aMonsterAttacking, int aMonsterDamage, int aDamage, int aAccuracy)
    {

        if (aMonsterAttacking >= 0 && MatchController.instance.monstersInGame[aMonsterAttacking].playerOwner == MatchController.instance.playerPlaying)
        {
            MatchController.instance.monstersInGame[aMonsterAttacking].canAttack = false;
        }
        MatchController.instance.monstersInGame[aMonsterDamage].HitMonster(aMonsterAttacking, aDamage, aAccuracy);


    }

    [ClientRpc]
    public void RpcHitMonster(int aMonsterAttacking, int aMonsterDamage, int aDamage, int aAccuracy)
    {
        if (!isServer)
        {
            if (aMonsterAttacking >= 0 && MatchController.instance.monstersInGame[aMonsterAttacking].playerOwner == MatchController.instance.playerPlaying)
            {
                MatchController.instance.monstersInGame[aMonsterAttacking].canAttack = false;
            }
            MatchController.instance.monstersInGame[aMonsterDamage].HitMonster(aMonsterAttacking, aDamage, aAccuracy);


        }
    }
    public void SetModeMonster(int aIdSpawn, int aMode)
    {
        MonsterController monster = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)];
        monster.SetMonsterMode(aMode);
        if (!isServer)
        {
            CmdSetModeMonster(aIdSpawn, aMode);
        }
        else
        {
            RpcSetModeMonster(aIdSpawn, aMode);
        }
    }
    [Command]
    public void CmdSetModeMonster(int aIdSpawn, int aMode)
    {
        MonsterController monster = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)];
        monster.SetMonsterMode(aMode);
    }
    [ClientRpc]
    public void RpcSetModeMonster(int aIdSpawn, int aMode)
    {
        MonsterController monster = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)];
        monster.SetMonsterMode(aMode);
    }
    [Command]
    public void CmdChangeModeMonster(int aIdSpawn)
    {
        MonsterController monster = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)];
        monster.ChangeNextMonsterMode();
    }
    [ClientRpc]
    public void RpcChangeModeMonster(int aIdSpawn)
    {
        if (!isServer)
        {
            MonsterController monster = MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)];
            monster.ChangeNextMonsterMode();
        }
    }
    public void EndGame(int aPlayerNumber)
    {
        if (isLocalPlayer)
        {
            if (aPlayerNumber == playerNumber)
            {
                loseCondition = true;
            }
            else
            {
                winCondition = true;
            }
            if (isServer)
            {
                RpcEndGame(aPlayerNumber);
            }
            else
            {
                CmdEndGame(aPlayerNumber);
            }
        }
    }
    [Command]
    void CmdEndGame(int aPlayerNumber)
    {
        if (aPlayerNumber == playerNumber)
        {
            loseCondition = true;
        }
        else
        {
            winCondition = true;
        }
    }
    [ClientRpc]
    void RpcEndGame(int aPlayerNumber)
    {
        if (!isServer)
        {

            if (aPlayerNumber == playerNumber)
            {
                loseCondition = true;
            }
            else
            {
                winCondition = true;
            }

        }
    }
    public void AddMonsterToDestroy(int aIdSpawn)
    {
        for (int i = 0; i < monstersToDestroy.Count; i++)
        {
            if (monstersToDestroy[i] == aIdSpawn)
            {
                return;
            }
        }
        if (isServer)
        {
            monstersToDestroy.Add(aIdSpawn);
        }
        else
        {
            CmdAddMonsterToDestroy(aIdSpawn);
        }
    }
    [Command]
    void CmdAddMonsterToDestroy(int aIdSpawn)
    {
        monstersToDestroy.Add(aIdSpawn);
    }
    public void DestroyMonsters()
    {
        if (isLocalPlayer)
        {
            if (!isServer)
            {
                CmdDestroyMonster();
            }
            else
            {
                RpcDestroyMonster();
            }
        }
    }
    [Command]
    public void CmdDestroyMonster()
    {
        Debug.Log("entreee");
        for (int i = monstersToDestroy.Count - 1; i >= 0; i--)
        {
            if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersToDestroy[i])!=-1) {
                int indexMonster = MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersToDestroy[i]);
                //destruir del tablero y enviar al cementerio
                MonsterController monster = MatchController.instance.monstersInGame[indexMonster];
                BoardController.instance.groundList[monster.idFloor].SetOcupateState(false, 0);
                RpcSetOcupateFloor(monster.idFloor, false, 0);

                monster.SetState(2);
            }
        }

        monstersToDestroy.Clear();

    }
    [ClientRpc]
    public void RpcDestroyMonster()
    {
        Debug.Log("entreeeRpc");
        for (int i = monstersToDestroy.Count - 1; i >= 0; i--)
        {
            if (MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersToDestroy[i]) != -1)
            {
                int indexMonster = MatchController.instance.GetIndexMonsterInGameListWithSpawn(monstersToDestroy[i]);
                //destruir del tablero y enviar al cementerio
                MonsterController monster = MatchController.instance.monstersInGame[indexMonster];
                BoardController.instance.groundList[monster.idFloor].SetOcupateState(false, 0);
                RpcSetOcupateFloor(monster.idFloor, false, 0);

                monster.SetState(2);
            }
        }

        monstersToDestroy.Clear();

    }

    public void SetOcupateFloor(int aIdFloor, bool aState, int aTypeCard)
    {
        if (isLocalPlayer)
        {
            BoardController.instance.groundList[aIdFloor].SetOcupateState(aState, aTypeCard);
            if (isServer)
            {
                RpcSetOcupateFloor(aIdFloor, aState, aTypeCard);
            }
            else
            {
                CmdSetOcupateFloor(aIdFloor, aState, aTypeCard);
            }
        }
    }
    public void SetCountCardsInDeck(int aCountCardsInDeck)
    {
        if (isServer)
        {
            RpcSetCountCardsInDeck(aCountCardsInDeck);
        }
        else
        {
            CmdSetCountCardsInDeck(aCountCardsInDeck);
        }
    }
    [Command]
    void CmdSetCountCardsInDeck(int aCountCardsInDeck)
    {
        Dealer.instance.countCardsInDeckEnemy = aCountCardsInDeck;
    }
    [ClientRpc]
    void RpcSetCountCardsInDeck(int aCountCardsInDeck)
    {
        if (!isServer)
        {
            Dealer.instance.countCardsInDeckEnemy = aCountCardsInDeck;
        }
       
    }
    public void AddCardToCementery(int aIdCard, int aPlayerOwner)
	{
		CementeryController.instance.AddCardToCementery (aIdCard,aPlayerOwner);
		if (isServer)
		{
			RpcAddCardToCementery(aIdCard, aPlayerOwner);
		}
		else
		{
			CmdAddCardToCementery(aIdCard, aPlayerOwner);
		}
	}
	[Command]
	public void CmdAddCardToCementery(int aIdCard, int aPlayerOwner)
	{
		CementeryController.instance.AddCardToCementery (aIdCard,aPlayerOwner);
	}
	[ClientRpc]
	public void RpcAddCardToCementery(int aIdCard, int aPlayerOwner)
	{
		if (!isServer)
		{
			CementeryController.instance.AddCardToCementery (aIdCard,aPlayerOwner);
		}

	}
    public void ShowCard(int aTypeCard, int aIdCard)
    {
       
        cardPlaying = Instantiate(prefabCard[aTypeCard], HandController.instance.transform.position, Quaternion.identity);
        cardPlaying.GetComponent<Dragg>().enabled = false;
        cardPlaying.GetComponent<CardController>().card = MatchController.instance.playerController.cards[aIdCard];
        cardPlaying.transform.SetParent(CMainCanvas.Inst.transform);


        cardPlaying.transform.localScale = new Vector3(1, 1, 1);
        cardPlaying.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cardPlaying.transform.RT().anchorMin = new Vector2(0, 0.5f);
        cardPlaying.transform.RT().anchorMax = new Vector2(0, 0.5f);
        cardPlaying.transform.RT().anchoredPosition = new Vector2(cardPlaying.transform.RT().rect.width / 2, 0);
      

        if (isServer)
        {
            RpcShowCard(aTypeCard, aIdCard);
        }
        else
        {
            CmdShowCard(aTypeCard, aIdCard);
        }
    }
    [Command]
    public void CmdShowCard(int aTypeCard, int aIdCard)
    {
        cardPlaying = Instantiate(prefabCard[aTypeCard], HandController.instance.transform.position, Quaternion.identity);
        cardPlaying.GetComponent<Dragg>().enabled = false;
        cardPlaying.GetComponent<CardController>().card = MatchController.instance.playerController.cards[aIdCard];
        cardPlaying.transform.SetParent(CMainCanvas.Inst.transform);


        cardPlaying.transform.localScale = new Vector3(1, 1, 1);
        cardPlaying.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cardPlaying.transform.RT().anchorMin = new Vector2(0, 0.5f);
        cardPlaying.transform.RT().anchorMax = new Vector2(0, 0.5f);

        cardPlaying.transform.RT().anchoredPosition = new Vector2(cardPlaying.transform.RT().rect.width / 2, -cardPlaying.transform.RT().rect.height / 2);
    }
    [ClientRpc]
    public void RpcShowCard(int aTypeCard, int aIdCard)
    {
        if (!isServer)
        {
            cardPlaying = Instantiate(prefabCard[aTypeCard], HandController.instance.transform.position, Quaternion.identity);
            cardPlaying.GetComponent<Dragg>().enabled = false;
            cardPlaying.GetComponent<CardController>().card = MatchController.instance.playerController.cards[aIdCard];
            cardPlaying.transform.SetParent(CMainCanvas.Inst.transform);


            cardPlaying.transform.localScale = new Vector3(1, 1, 1);
            cardPlaying.transform.localRotation = Quaternion.Euler(0, 0, 0);
            cardPlaying.transform.RT().anchorMin = new Vector2(0, 0.5f);
            cardPlaying.transform.RT().anchorMax = new Vector2(0, 0.5f);

            cardPlaying.transform.RT().anchoredPosition = new Vector2(cardPlaying.transform.RT().rect.width / 2, -cardPlaying.transform.RT().rect.height / 2);
        }

    }
    public void SetMonsterInmune(int aIndexdMonster, int aInmune)
    {
        MatchController.instance.monstersInGame[aIndexdMonster].SetMonsterInmune(aInmune);
        if (isServer)
        {
            RpcSetMonsterInmune(aIndexdMonster, aInmune);
        }
        else
        {
            CmdSetMonsterInmune(aIndexdMonster, aInmune);
        }
    }
    [Command]
    public void CmdSetMonsterInmune(int aIndexdMonster, int aInmune)
    {
        MatchController.instance.monstersInGame[aIndexdMonster].SetMonsterInmune(aInmune);
    }
    [ClientRpc]
    public void RpcSetMonsterInmune(int aIndexdMonster, int aInmune)
    {
        if (!isServer)
        {
            MatchController.instance.monstersInGame[aIndexdMonster].SetMonsterInmune(aInmune); 
        }

    }
    public void AddStatsMonster(int aIndexdMonster, int aAttack, int aDefense, int aHp, int aVelocity)
    {
        MatchController.instance.monstersInGame[aIndexdMonster].AddStatsMonster(aAttack, aDefense, aHp, aVelocity);
        if (isServer)
        {
            RpcAddStatsMonster(aIndexdMonster, aAttack, aDefense, aHp, aVelocity);
        }
        else
        {
            CmdAddStatsMonster(aIndexdMonster, aAttack, aDefense, aHp, aVelocity);
        }
    }
    [Command]
    public void CmdAddStatsMonster(int aIndexdMonster, int aAttack, int aDefense, int aHp, int aVelocity)
    {
        MatchController.instance.monstersInGame[aIndexdMonster].AddStatsMonster(aAttack, aDefense, aHp, aVelocity);
    }
    [ClientRpc]
    public void RpcAddStatsMonster(int aIndexdMonster, int aAttack, int aDefense, int aHp, int aVelocity)
    {
        if (!isServer)
        {
            MatchController.instance.monstersInGame[aIndexdMonster].AddStatsMonster(aAttack, aDefense, aHp, aVelocity);
        }

    }
    public void EndShowCard()
    {
        Destroy(cardPlaying.gameObject);
        if (isServer)
        {
            RpcEndShowCard();
        }
        else
        {
            CmdEndShowCard();
        }
    }
    [Command]
    public void CmdEndShowCard()
    {
        Destroy(cardPlaying.gameObject);
    }
    [ClientRpc]
    public void RpcEndShowCard()
    {
        if (!isServer)
        {
            Destroy(cardPlaying.gameObject);
        }

    }
    [Command]
    public void CmdCreateMonster(int idCard, int idFloor, int playerNumber)
    {
        GameObject monsterObject = (GameObject)Instantiate(cards[idCard].prefabCard, BoardController.instance.groundList[idFloor].transform.position, Quaternion.identity);
        monsterObject.GetComponent<MonsterController>().SetStats(idCard, MatchController.instance.GetNextIdSpawn(), idFloor, playerNumber, cards[idCard].seCost, cards[idCard].attack, cards[idCard].defense, cards[idCard].hp, cards[idCard].velocity, cards[idCard].typeAttack, cards[idCard].TypeCard);
        BoardController.instance.groundList[idFloor].SetOcupateState(true, 0);
        RpcSetOcupateFloor(idFloor, true, 0);
        NetworkServer.Spawn(monsterObject);
       

    }
    public void DestroyTrap(int aIndexTrap)
    {
        MatchController.instance.DestroyTrap(aIndexTrap);
        if (isServer)
        {
            RpcDestroyTrap(aIndexTrap);
        }
        else
        {
            CmdDestroyTrap(aIndexTrap);
        }
    }
    [Command]
    void CmdDestroyTrap(int aIndexTrap)
    {
        MatchController.instance.DestroyTrap(aIndexTrap);
    }
    [ClientRpc]
    void RpcDestroyTrap(int aIndexTrap)
    {
        if (!isServer)
        {
            MatchController.instance.DestroyTrap(aIndexTrap);
        }

    }
    [Command]
    public void CmdCreateTrap(int idCard, int idFloor, int playerNumber)
    {
        Debug.Log(cards[idCard].TypeCard);
        GameObject trapObject = (GameObject)Instantiate(cards[idCard].prefabCard, BoardController.instance.groundList[idFloor].transform.position, Quaternion.identity);
        trapObject.GetComponent<TrapController>().SetStats(idCard, idFloor, playerNumber, cards[idCard].TypeCard);
        BoardController.instance.groundList[idFloor].SetOcupateState(true, 1);
        RpcSetOcupateFloor(idFloor, true, 1);
        NetworkServer.Spawn(trapObject);


    }
    [Command]
    public void CmdMoveMonster(int aIdSpawn, int aIdFloorToMove)
    {
        MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)].SetFloorToMove(aIdFloorToMove);

    }
    [Command]
    public void CmdEndTurn()
    {
        MatchController.instance.EndTurn();

    }
    [Command]
    public void CmdSetOcupateFloor(int aIdFloor, bool aState, int aTypeCard)
    {
        BoardController.instance.groundList[aIdFloor].SetOcupateState(aState, aTypeCard);

    }

    [ClientRpc]
    void RpcMoveMonster(int aIdSpawn, int aIdFloorToMove)
    {
        if (!isServer)
        {

            MatchController.instance.monstersInGame[MatchController.instance.GetIndexMonsterInGameListWithSpawn(aIdSpawn)].SetFloorToMove(aIdFloorToMove);

        }
    }
    [ClientRpc]
    void RpcSetOcupateFloor(int aIdFloor, bool aState, int aTypeCard)
    {
        if (!isServer)
        {
            BoardController.instance.groundList[aIdFloor].SetOcupateState(aState, aTypeCard);
        }
    }
}
