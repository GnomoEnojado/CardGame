using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MonsterController : NetworkBehaviour
{
    [SyncVar]
    public int idCard;
    [SyncVar]
    public int idFloor;
    [SyncVar]
    public int playerOwner;

    int seCost;

    [SyncVar]
    public int attack;
    [SyncVar]
    public int defense;
    [SyncVar]
    public int hp;
    [SyncVar]
    public int velocity;
    [SyncVar]
    public int remainingVelocity;
    [SyncVar]
    public int idSpawn;
    [SyncVar]
    public int typeAttack;
    [SyncVar] //0 defensa 1 ataque
    public int monsterMode = 1;
    [SyncVar]
    public int accuracy=100;
    [SyncVar] //0 no es inmune // 1 es inmune
    public int inmune=0;
    public int typeCard;
    public int lastIdFloor=-1;
    private float VelMoveObject = 5;
    public bool canAttack = true;
    public bool canMove = true;
    public bool canChangeMode = true;
    public bool goToCementery=true;
    public bool frozen=false;

    public bool king;
    public List<SpriteRenderer> modeIcons; //0 defensa //1 ataque
	public List<GameObject> prefabIcons;


    bool counterHit;
    bool needResetVariables = true;
    int state; //0 iddle----  1 move

    public ColorCapsule colorCapsule;
    Text txtAttackDefense;
    Text txtHp;
    Text txtVelocity;


    private void Awake()
    {
        MatchController.instance.AddMonsterInGame(this);
       
        txtAttackDefense = transform.GetChild(0).GetComponent<Text>();
        txtHp = transform.GetChild(1).GetComponent<Text>();
        txtVelocity = transform.GetChild(2).GetComponent<Text>();
        canMove = false;
		canAttack = false;
    
  
		if(!king)
		{
			Vector3 attackAndDef = new Vector3 (-0.049f, 0.889f, -0.824f);
			Vector3 hearth = new Vector3 (1.001f, 0.508f, -0.76f);
			Vector3 vel = new Vector3 (0.327f, 0.598f, -0.799f);
			GameObject def= Instantiate (prefabIcons[0],transform.position,Quaternion.identity);
			GameObject attck=Instantiate (prefabIcons[1],transform.position,Quaternion.identity);
			GameObject hp=Instantiate (prefabIcons[2],transform.position,Quaternion.identity);
			GameObject velocity=Instantiate (prefabIcons[3],transform.position,Quaternion.identity);
			def.transform.parent = this.transform;
			attck.transform.parent = this.transform;
			hp.transform.parent = this.transform;
			velocity.transform.parent = this.transform;
			def.transform.localPosition = attackAndDef;
			attck.transform.localPosition = attackAndDef;
			hp.transform.localPosition = hearth;
			velocity.transform.localPosition = vel;

			txtAttackDefense.gameObject.transform.localPosition = new Vector3(-0.017f, 0.885f, -0.793f);
			txtHp.gameObject.transform.localPosition = new Vector3(1.05f, 0.523f, -0.781f);
			txtVelocity.gameObject.transform.localPosition = new Vector3(0.377f, 0.629f, -0.776f);

			modeIcons [0] = def.GetComponent<SpriteRenderer> ();
			modeIcons [1] = attck.GetComponent<SpriteRenderer> ();
		}
        if (MatchController.instance.GetPlayerNumber() == 2)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    private void Start()
    {
        colorCapsule.SetMaterialColor(playerOwner);
    
    }
    // Update is called once per frame
    void Update()
    {
        if (!king) {
            if (monsterMode == 1)
            {
                if (attack > GameController.instance.gameCards[idCard].attack)
                {
                    txtAttackDefense.color = Color.green;
                }
                else if (attack < GameController.instance.gameCards[idCard].attack)
                {
                    txtAttackDefense.color = Color.red;
                }
                else
                {
                    txtAttackDefense.color = Color.black;
                }
                txtAttackDefense.text = attack.ToString();
            }
            else
            {
                if (defense > GameController.instance.gameCards[idCard].defense)
                {
                    txtAttackDefense.color = Color.green;
                }
                else if (defense < GameController.instance.gameCards[idCard].defense)
                {
                    txtAttackDefense.color = Color.red;
                }
                else
                {
                    txtAttackDefense.color = Color.black;
                }
                txtAttackDefense.text = defense.ToString();
            }
        }
        else
        {
            txtAttackDefense.text = attack.ToString();
        }
        if (hp> GameController.instance.gameCards[idCard].hp)
        {
            txtHp.color = Color.green;
        }
        else if (hp < GameController.instance.gameCards[idCard].hp)
        {
            txtHp.color = Color.red;
        }
        else
        {
            txtHp.color = Color.black;
        }
        txtHp.text = hp.ToString();
        txtVelocity.text = remainingVelocity.ToString();


        if (MatchController.instance.playerPlaying != playerOwner && needResetVariables)
        {
            ResetVariables();
            needResetVariables = false;
        }
        else if (MatchController.instance.playerPlaying == playerOwner && !needResetVariables)
        {
            needResetVariables = true;
        }
        if (state == 0)
        {
            //Iddle
        }
        else if (state == 1)
        {
            //Move
            MoveMonster();
        }
        else if (state == 2)
        {
            //Die
            if (!MatchController.instance.activatingCard)
            {
                Destroy(gameObject);
            }
        }

    
    }
	void OnDestroy()
	{
		CementeryController.instance.AddCardToCementery(idCard,playerOwner);
	}
    public virtual void ResetVariables()
    {

        canAttack = true;
        canMove = true;
        canChangeMode = true;
        remainingVelocity = velocity;
    }

    private void ActiveIconMode(int aIcon)
    {
        if (!modeIcons[aIcon].enabled)
        {
            if (aIcon == 0)
            {
                modeIcons[1].enabled = false;
                modeIcons[0].enabled = true;

            }
            else
            {
                modeIcons[0].enabled = false;
                modeIcons[1].enabled = true;
            }
        }
    }
    public void ChangeNextMonsterMode()
    {
        if (monsterMode == 0)
        {
            monsterMode = 1;
            ActiveIconMode(1);

        }
        else
        {
            monsterMode = 0;
            ActiveIconMode(0);
        }
        canChangeMode = false;

    }
    public void SetMonsterMode(int aMode)
    {
        monsterMode = aMode;
        ActiveIconMode(aMode);

    }
    public void AlreadyAttack()
    {
        canChangeMode = false;
    }
    public void SetStats(int aIdCard, int aIdSpawn, int aIdFloor, int aPlayerOwner, int aSeCost, int aAttack, int aDefense, int aHp, int aVelocity, int aTypeAttack, int aTypeCard)
    {
        idCard = aIdCard;
        idSpawn = aIdSpawn;
        idFloor = aIdFloor;
        playerOwner = aPlayerOwner;
        seCost = aSeCost;
        attack = aAttack;
        defense = aDefense;
        hp = aHp;
        velocity = aVelocity;
        remainingVelocity = velocity;
        typeAttack = aTypeAttack;
        typeCard = aTypeCard;

    }
    public void AddStatsMonster(int aAttack, int aDefense, int aHp, int aVelocity)
    {
        attack += aAttack;
        defense += aDefense;
        hp += aHp;
        velocity += aVelocity;
		remainingVelocity += aVelocity;
        if (king)
        {
            attack = 10 - hp;
        }
    }
    public void SetMonsterInmune(int aInmune)
    {
        inmune = aInmune;
    }
    private void MoveMonster()
    {
        Vector3 direction = BoardController.instance.groundList[idFloor].transform.position - transform.position;
        float distanceInFrames = VelMoveObject * Time.deltaTime;

        if (direction.magnitude > distanceInFrames)
        {
            transform.Translate(direction.normalized * distanceInFrames, Space.World);
        }
        else
        {
            MatchController.instance.playerController.EndMonsterMove(this, remainingVelocity);
            SetState(0);

        }

    }
    public void SetFloorToMove(int aIdFloor)
    {
        remainingVelocity--;

        if (remainingVelocity == 0)
        {
            canMove = false;
        }
        MatchController.instance.playerController.SetOcupateFloor(idFloor, false, 0);
        lastIdFloor = idFloor;
        idFloor = aIdFloor;
        MatchController.instance.playerController.SetOcupateFloor(idFloor, true, 0);
        state = 1;
    }
    public void SetState(int aState)
    {
       
        if (state!=2) {
            state = aState;
          
        }
    }
    public int GetState()
    {
        return state;
    }
    public void HitMonster(int aMonsterAttackingMeIndex, int aDamage,int aAccuracy)
    {
        counterHit = false;
        //si la accuracy acierta pregunto en que modo esta el monstruo.
		if (((aMonsterAttackingMeIndex!=-1 && aAccuracy <= MatchController.instance.monstersInGame[aMonsterAttackingMeIndex].accuracy )||(aMonsterAttackingMeIndex==-1))  && inmune==0)
        {
            if (monsterMode == 1)
            {
                hp -= aDamage;
            }
            else if (monsterMode == 0)
            {
                if (aDamage > defense)
                {
                    hp -= aDamage - defense;
                    if (defense != 0)
                    {
                        defense = 0;
                    }
                }
                else
                {
                    defense = defense - aDamage;
                }
            }
        }
        else
        {
            if (inmune==1)
            {
                Debug.Log("Monstruo Inmune");
            }
            else
            {
                Debug.Log("El monstruo fallo el ataque");
            }
            
        }
        if (hp <= 0)
        {
            if (!king)
            {
                MatchController.instance.playerController.AddMonsterToDestroy(idSpawn);
            }
            else
            {
                MatchController.instance.EndGame(playerOwner);
            }
        }
        // usar index -1 para hacerle daño a un monstruo con cartas que no son mostruos (ejemplo magicas)
        if (aMonsterAttackingMeIndex >= 0)
        {
            //monsterAttackingMe = MatchController.instance.monstersInGame[aMonsterAttackingMeIndex];

            //si estoy en modo de ataque y puedo atacar al monstruo, lo ataco.
            if (monsterMode == 1 && playerOwner != MatchController.instance.playerPlaying && !king &&!frozen)
            {
                List<MonsterController> monstersCanAttack = BoardController.instance.GetMonstersCanAttack(idFloor, playerOwner, typeAttack);

                for (int i = 0; i < monstersCanAttack.Count; i++)
                {
                    if (monstersCanAttack[i].gameObject == MatchController.instance.monstersInGame[aMonsterAttackingMeIndex].gameObject)
                    {
                        MatchController.instance.playerController.HitMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawn), aMonsterAttackingMeIndex, attack);
                        counterHit = true;
                    }
                }

            }
            else if (playerOwner != MatchController.instance.playerPlaying && king)
            {
                //cambiar el 10 por una variable maxHp
                attack = 10 - hp;
                MatchController.instance.playerController.HitMonster(MatchController.instance.GetIndexMonsterInGameListWithSpawn(idSpawn), aMonsterAttackingMeIndex, attack);
                counterHit = true;
            }
        }
        //finalizo la secuencia de ataque ya sea porque no puedo devolver el ataque o porque ya lo devolvi(el devolver lo ejecuta el monstruo que es atacado primero)
        if (playerOwner == MatchController.instance.playerPlaying || monsterMode == 0 || aMonsterAttackingMeIndex < 0 || !counterHit || frozen)
        {
            MatchController.instance.playerController.DestroyMonsters();
        }
        if (aMonsterAttackingMeIndex>=0)
        {
            EnemyHitMe(MatchController.instance.monstersInGame[aMonsterAttackingMeIndex].idSpawn);
        }
        else
        {
            //recibi daño de algo que no es un monstruo
            EnemyHitMe(-1);
        }
    }

    //uso el id spawn porque el index si el monstruo es destruido puede que este accediendo a otro
    //monstruo en lugar del que quiero, el idspawn es unico.
    public virtual void EnemyHitMe(int aIdSpawnEnemy)
    {
        //implementar en las clases que necesiten hacer algo al recibir daño
    }
    public virtual void IHitEnemy(int aIdSpawnEnemy)
    {
        //implementar en las clases que necesiten hacer algo al atacar a un enemigo
    }
}
