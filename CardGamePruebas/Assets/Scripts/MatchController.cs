using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;



public class MatchController : NetworkBehaviour
{
    public static MatchController instance = null;
    public int playerSE = 0;
    public int initSe=0;

    int turn = 1;

    public int playerPlaying = 0;
    public bool startRandomPlayer;
    public Text txtPlayerSE;
    public Text txtPlayerPlaying;
	public Text gameCondition;
    public Text txtTimerTurn;
    //usada en el script dragg para saber si se esta arrastrando una carta
    public bool draggingCard;
    public int idDraggingCard=-1;
    public Dealer dealer;
    public GameObject prefab;
    public PlayerController playerController;
    //public List<Floor> groundList;
    public bool playing;
    int playerNumber;
    public bool gameStarted;
    public Button endTurnBttn;
    public List<MonsterController> monstersInGame;
	public List<TrapController> trapsInGame;
    private int idSpawn = 0;
	public bool activatingCard;
    float timerTurn;
    public float turnDuration=90;
   
    public Texture2D[] cursorTextures;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
   
    }
    private void Start()
    {
        if (Dealer.instance.shuffleDeck)
        {
            Dealer.instance.deck.Shuffle();
        }
        if (isServer)
        {
            if (startRandomPlayer)
            {
                playerPlaying = Random.Range(1, 3);
            }
            else
            {
                playerPlaying = 2;
            }
            
        }
        timerTurn = turnDuration;
    }

    private void Update()
    {
        UpdateMonstersInGameList();
		UpdateTrapsInGameList ();
        txtPlayerSE.text = playerSE.ToString() + "/10";
        txtPlayerPlaying.text = playerPlaying.ToString();
       

        if (playerPlaying==playerNumber)
        {
            if (timerTurn>0)
            {
                timerTurn -= Time.deltaTime;
            }
            if (timerTurn<=30)
            {
                txtTimerTurn.enabled = true;
                txtTimerTurn.color = Color.red;
                txtTimerTurn.text = timerTurn.ToString("F0");
            }
            else
            {
                txtTimerTurn.color = Color.black;
            }
            if (timerTurn<=0)
            {
                timerTurn = 0;
                if (!activatingCard) {
                    EndTurn();
                    timerTurn = turnDuration;
                }
            }
        }
        else
        {
            timerTurn = turnDuration;
            txtTimerTurn.enabled = false;
        }
        if (activatingCard && playerPlaying==playerNumber)
        {
            hotSpot = new Vector2(cursorTextures[0].width / 2, cursorTextures[0].height / 2);
            Cursor.SetCursor(cursorTextures[0], hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        if (playerController != null)
        {
			if(playerController.winCondition)
			{
				gameCondition.text = "Ganaste";
			}
			else if(playerController.loseCondition)
			{
				gameCondition.text = "Perdiste";
			}
            if (playerController.playerNumber == playerPlaying && !activatingCard )
            {
                playing = true;
                endTurnBttn.enabled = true;
                endTurnBttn.GetComponent<Image>().color = Color.green;
            }
            else 
            {
                playing = false;
                endTurnBttn.enabled = false;
                endTurnBttn.GetComponent<Image>().color = Color.gray;
            }
        }
        if (isServer)
        {

            if (Input.GetKeyDown(KeyCode.Space) && NetworkServer.connections.Count == 2 && !gameStarted)
            {

                StartGame();

            }
        }
    }
    void UpdateMonstersInGameList()
    {
        for (int i = 0; i < monstersInGame.Count; i++)
        {
            if (monstersInGame[i]==null)
            {
                monstersInGame.RemoveAt (i);
            }
        }
    }
	void UpdateTrapsInGameList()
	{
		for (int i = 0; i < trapsInGame.Count; i++)
		{
			if (trapsInGame[i]==null)
			{
				trapsInGame.RemoveAt(i);
			}
		}
	}

    public void StartGame()
    {
		playerController.CmdCreateMonster(0,4,2);
		playerController.CmdCreateMonster(0,20,1);
        RpcStartTurn(playerPlaying);

    }
    public void EndTurn()
    {
        if (!MatchController.instance.activatingCard) {


            if (isServer)
            {
                turn++;

                if (playerPlaying == 1)
                {
                    playerPlaying = 2;

                }
                else
                {
                    playerPlaying = 1;

                }
                RpcNotifyEndTurn(turn, playerPlaying, CalculateSETurn());
            }
            else
            {
                playerController.CmdEndTurn();
            }
        }

    }

    int CalculateSETurn()
    {

        if (turn % 2 == 0 && turn <= 20)
        {
            return turn / 2;
        }
        else if (turn > 20)
        {
            return 10;
        }
        else
        {
            return (turn / 2) + 1;
        }
    }

    public bool CanSummonCard(Card aCard, Floor aFloor)
    {

        
        if (aCard.seCost <= playerSE&& aFloor.playerFloor() == playerPlaying)
        {
            if (aCard.TypeCard==0)
            {
                if (!aFloor.GetOcupateMonsterState())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (aCard.TypeCard == 1)
            {

                if (!aFloor.haveTrap)
				{
                    // si el piso esta ocupado por un monstruo, chequeo que el monstruo sea mio
                    // sino no puedo jugar la trampa
                    if (aFloor.isOcupateByMonster && monstersInGame[GetIndexMonsterInGameListWithFloor(aFloor.idFloor)].playerOwner == playerNumber)
					{
                        return true;
                    }
					else if(!aFloor.isOcupateByMonster)
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
    public bool SummonCard(Card aCard, Floor aFloor)
	{
        if (CanSummonCard(aCard,aFloor))
        {
            playerSE-=aCard.seCost;
            if (aCard.TypeCard==0)
            {
                playerController.CmdCreateMonster(aCard.Id, aFloor.idFloor, playerNumber);
            }
            else if (aCard.TypeCard == 1)
            {
                playerController.CmdCreateTrap(aCard.Id, aFloor.idFloor, playerNumber);
            }
            return true;
        }
        else
        {
            return false;
        }
   

    }
    public bool isPlaying()
    {
        return playing;
    }
    public void SetPlayer(PlayerController aPlayerController)
    {
        playerController = aPlayerController;
        playerNumber = aPlayerController.playerNumber;
        if (playerNumber==2)
        {
            Camera.main.transform.position = new Vector3(-15.55f, 23.75f, 4.23f);
            Camera.main.transform.eulerAngles = new Vector3(39.909f, 101.132f, 2.082f);
        }
    }
    public int GetNextIdSpawn()
    {
        idSpawn++;
        return idSpawn;
    }
    public void AddMonsterInGame(MonsterController aMonster)
    {
        monstersInGame.Add(aMonster);
    }
	public void AddTrapInGame(TrapController aTrap)
	{
		trapsInGame.Add(aTrap);
	}
    public int GetIndexMonsterInGameListWithSpawn(int aIdSpawn)
    {
        for (int i = 0; i < monstersInGame.Count; i++)
        {
            if (monstersInGame[i].idSpawn==aIdSpawn)
            {
                return i;
            }
        }
        return -1;
    }
	public int GetIndexMonsterInGameListWithFloor(int aIdFloor)
	{
		for (int i = 0; i < monstersInGame.Count; i++)
		{
			if (monstersInGame[i].idFloor==aIdFloor)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetIndexTrapsInGameListWithFloor(int aIdFloor)
	{
		for (int i = 0; i < trapsInGame.Count; i++)
		{
			if (trapsInGame[i].idFloor==aIdFloor)
			{
				return i;
			}
		}
		return -1;
	}
	public int GetPlayerNumber()
	{
		return playerNumber;
	}
    public int GetTurnNumber()
    {
        return turn;
    }
    public void EndGame(int aPlayerNumber)
	{
		playerController.EndGame(aPlayerNumber);
	}
    public void DestroyTrap(int aIndexTrap)
    {
		MatchController.instance.playerController.SetOcupateFloor(trapsInGame[aIndexTrap].idFloor, false, 1);
        Destroy(trapsInGame[aIndexTrap].gameObject);

    }



    [ClientRpc]
    void RpcStartTurn(int aPlayerPlaying)
    {
       
        playerPlaying = aPlayerPlaying;
        if (playerNumber == playerPlaying)
        {
            playerSE++;
            Dealer.instance.CreateHand(true);
            Debug.Log("Empezo el juego");
        }
        else
        {
            Dealer.instance.CreateHand(false);
        }

        gameStarted = true;
    }
    [ClientRpc]
    void RpcNotifyEndTurn(int aTurn, int aPlayerPlaying, int aSeForPlayer)
    {
        Debug.Log("RpcNotifyEndTurn");
        playerPlaying = aPlayerPlaying;
        turn = aTurn;
        if (playerNumber == playerPlaying)
        {
            playerSE = initSe + aSeForPlayer;
            dealer.DrawCard();
        }

    }
}
