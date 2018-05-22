using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

public class ShowCards : MonoBehaviour {
    public HorizontalScrollSnap scrollCards;
    public Canvas canvasShowCards;
    public GameObject prefabDeckCards;
    public Transform deckCards;
    public GameObject buttonPlay;

	// Use this for initialization
	void Start () {
        ShowCardsPlayer();

    }
	
	// Update is called once per frame
	void Update ()
    {
        SelectCardForDeck();
        RemoveCardForDeck();
        if (GameController.instance.deck.Count==30)
        {
            buttonPlay.SetActive(true);
        }
        else
        {
            buttonPlay.SetActive(false);
        }
    }
    public void ShowCardsPlayer()
    {
        List<int> idCardsPlayer = GameController.instance.idCardsPlayer; 
        for (int i = 0; i < idCardsPlayer.Count; i++)
        {
            Card card = GameController.instance.gameCards[idCardsPlayer[i]];
            GameObject cardInstance = Instantiate(GameController.instance.prefabCard[card.TypeCard], transform.position, Quaternion.identity);
            cardInstance.GetComponent<CardController>().card = card;
            cardInstance.GetComponent<CardController>().DontDestroyCard();
            cardInstance.GetComponent<Dragg>().enabled = false;
            scrollCards.AddChild(cardInstance);
        }
        if (scrollCards.ChildObjects.Length>=6)
        {
            scrollCards.StartingScreen = 3;
        }
        else
        {
            scrollCards.StartingScreen = scrollCards.ChildObjects.Length;
        }
    }
    public void SelectCardForDeck()
    {
        if (Input.GetMouseButtonDown(0) && deckCards.transform.childCount<30)
        {
            List<RaycastResult> results = CMainCanvas.Inst.GetGraphicsElements();
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<CardController>() && CanAddToDeck(results[i].gameObject.GetComponent<CardController>().card.Id))
                {
                    Card card = GameController.instance.gameCards[results[i].gameObject.GetComponent<CardController>().card.Id];
                    GameObject cardInstance = Instantiate(prefabDeckCards, transform.position, Quaternion.identity);
                    cardInstance.GetComponent<DeckCardController>().card = card;
                    cardInstance.transform.SetParent(deckCards.transform);
                    cardInstance.transform.localScale = new Vector3(1,1,1);
                    GameController.instance.deck.Add(card.Id);
                }
            }
        }
    }
    public void RemoveCardForDeck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = CMainCanvas.Inst.GetGraphicsElements();
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<DeckCardController>())
                {
                    Card card = GameController.instance.gameCards[results[i].gameObject.GetComponent<DeckCardController>().card.Id];
                    GameController.instance.deck.Remove(card.Id);
                    Destroy(results[i].gameObject);
                }
            }
        }
    }
    bool CanAddToDeck(int aIdCard)
    {
        List<int> deck = GameController.instance.deck;
        int countCardsInDeck=0;
        for (int i = 0; i < deck.Count; i++)
        {           
            if (deck[i]==aIdCard)
            {
                countCardsInDeck++;
            }
        }
        if (GameController.instance.gameCards[aIdCard].rarity >= 4)
        {
            if (countCardsInDeck<1)
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
            if (countCardsInDeck < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

 
}
    
