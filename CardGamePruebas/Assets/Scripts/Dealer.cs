using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour {
    public static Dealer instance = null;
    public List<Card> deck;
    public List<GameObject> prefabCard;
    public bool shuffleDeck;
    public int countCardsInDeckEnemy;
	int spawnNumber=0;

    private float delayDestroyCard=2f;
    public List<int> cardsToShow;

	int countInitHand=5;
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
        for (int i = 0; i < GameController.instance.deck.Count; i++)
        {
            deck.Add(GameController.instance.gameCards[GameController.instance.deck[i]]);
        }
    }
  
    public void CreateHand(bool aStartPlayer)
    {
        if (aStartPlayer)
        {
            countInitHand++;
        }
        StartCoroutine(HandCreator());
    }
    IEnumerator HandCreator()
    {
        for (int i = 0; i < countInitHand; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DrawCard()
    {
        Card nextCard = GetTopCard(); 
        if (deck.Count > 0 && HandController.instance.GetCountCardsInHand()<10)
        {
            GameObject cardInstance;
            if (MatchController.instance.GetPlayerNumber()==2)
            {
                cardInstance = Instantiate(prefabCard[nextCard.TypeCard], new Vector3(2.5f, 7, 10), Quaternion.Euler(-60, 180, -115));
            }
            else
            {
                cardInstance = Instantiate(prefabCard[nextCard.TypeCard], new Vector3(8, 7, -10.5f), Quaternion.Euler(-60, 180, -115));
            }

            cardInstance.GetComponent<CardController>().card = nextCard;


            cardInstance.transform.SetParent(CMainCanvas.Inst.transform);
            cardInstance.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardInstance.transform.localRotation = Quaternion.Euler(-130, -8, 70);
   
            HandController.instance.cardsInHand.Add(cardInstance);
			int idSpawn = GetSpawnNumber ();
			cardInstance.GetComponent<CardController> ().idSpawnCard = idSpawn;
			MatchController.instance.playerController.CreateEnemyCard (idSpawn,nextCard.Id);

        }
        else if (HandController.instance.GetCountCardsInHand() == 10)
        {
            cardsToShow.Add(nextCard.Id);
            int idSpawn = GetSpawnNumber();
            MatchController.instance.playerController.CreateEnemyCard(idSpawn, nextCard.Id);
            if (cardsToShow.Count == 1)
            {
                StartCoroutine(CardsToShow(delayDestroyCard,false));
            }
       

        }
        MatchController.instance.playerController.SetCountCardsInDeck(deck.Count);
    }



	public void DrawEnemyCard(int aIdSpawnCard, int aIdCard)
    {

        if (HandController.instance.cardsInEnemyHand.Count<10)
        {
            GameObject cardInstance;
            if (MatchController.instance.GetPlayerNumber() == 2)
            {
                cardInstance = Instantiate(prefabCard[3], new Vector3(8, 7, -10.5f), Quaternion.Euler(-60, 180, -115));
         
            }
            else
            {
                cardInstance = Instantiate(prefabCard[3], new Vector3(2.5f, 7, 10), Quaternion.Euler(-60, 180, -115));
  
            }
           
			cardInstance.GetComponent<EnemyCardController>().idSpawnCard = aIdSpawnCard;
			cardInstance.GetComponent<EnemyCardController>().idCard = aIdCard;

			cardInstance.transform.SetParent(CMainCanvas.Inst.transform);
			cardInstance.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            cardInstance.transform.localRotation = Quaternion.Euler(-130, -8, 70);
            HandController.instance.cardsInEnemyHand.Add(cardInstance);

		}
		else if (HandController.instance.cardsInEnemyHand.Count == 10)
		{
            cardsToShow.Add(aIdCard);
            if (cardsToShow.Count == 1)
            {
                StartCoroutine(CardsToShow(delayDestroyCard,true));
            }


        }
		MatchController.instance.playerController.SetCountCardsInDeck(deck.Count);
	}

    public void AddCardToHand(int aIdCard)
    {
        Card nextCard = MatchController.instance.playerController.cards[aIdCard];
        if (deck.Count > 0 && HandController.instance.GetCountCardsInHand() < 10)
        {
            GameObject cardInstance = Instantiate(prefabCard[nextCard.TypeCard], new Vector3(Screen.width - 100, -100, 0), Quaternion.identity);
            cardInstance.GetComponent<CardController>().card = nextCard;


            cardInstance.transform.SetParent(CMainCanvas.Inst.transform); 
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            HandController.instance.cardsInHand.Add(cardInstance);
            int idSpawn = GetSpawnNumber();
            cardInstance.GetComponent<CardController>().idSpawnCard = idSpawn;
            MatchController.instance.playerController.CreateEnemyCard(idSpawn, nextCard.Id);
        }
        else if (HandController.instance.GetCountCardsInHand() == 10)
        {
            cardsToShow.Add(aIdCard);
            int idSpawn = GetSpawnNumber();
            MatchController.instance.playerController.CreateEnemyCard(idSpawn, nextCard.Id);
            if (cardsToShow.Count==1)
            {
                StartCoroutine(CardsToShow(delayDestroyCard,false));
            }
        

        }
    }
 
	private int GetSpawnNumber()
	{
		spawnNumber += 1;
		return spawnNumber;
	}
    IEnumerator DestroyCard(GameObject aObject,float delay)
    {
        for (int i = 0; i < cardsToShow.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            Destroy(aObject.gameObject);

        }
       
        
    }
    IEnumerator CardsToShow(float delay, bool aEnemyPlayer)
    {
        for (int i = 0; i < cardsToShow.Count; i++)
        {
            
            GameObject cardInstance = Instantiate(prefabCard[GameController.instance.gameCards[cardsToShow[i]].TypeCard], HandController.instance.transform.position, Quaternion.identity);
            cardInstance.GetComponent<CardController>().card = GameController.instance.gameCards[cardsToShow[i]];
            cardInstance.GetComponent<CardController>().DontDestroyCard();
            cardInstance.GetComponent<Dragg>().enabled = false;
            cardInstance.transform.SetParent(CMainCanvas.Inst.transform);
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            cardInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (aEnemyPlayer==false)
            {
                cardInstance.transform.RT().anchorMin = new Vector2(1, 0);
                cardInstance.transform.RT().anchorMax = new Vector2(1, 0);
                cardInstance.transform.RT().anchoredPosition = new Vector2(-80, 130);
            }
            else
            {
                cardInstance.transform.RT().anchorMin = new Vector2(0, 1);
                cardInstance.transform.RT().anchorMax = new Vector2(0, 1);
                cardInstance.transform.RT().anchoredPosition = new Vector2(80, -130);
            }
          

            StartCoroutine(DestroyCard(cardInstance, delayDestroyCard));
            yield return new WaitForSeconds(delay);
        }
        cardsToShow.Clear();

    }
    Card GetTopCard()
    {
        if (deck.Count>0)
        {
            Card topCard = deck[deck.Count - 1];
            deck.RemoveAt (deck.Count - 1);
            return topCard;
        }
        else
        {
            return null;
        }
    }
	
}
