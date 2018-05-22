using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    public static HandController instance = null;
    public List<GameObject> cardsInHand;
    public List<GameObject> cardsInEnemyHand;
    float offSetCards = 55;


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

    // Update is called once per frame
    void Update()
    {

        UpdateList();
        OrderHand();

    }
    void OrderHand()
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i].GetComponent<Dragg>().canOrder)
            {
                cardsInHand[i].transform.SetSiblingIndex(i);
                RectTransform rtf = cardsInHand[i].transform.RT();


                rtf.anchorMin = new Vector2(0.5f, 0);
                rtf.anchorMax = new Vector2(0.5f, 0);

                cardsInHand[i].GetComponent<Dragg>().isDragging = true;
                float totalTwist = 30f;
           
                int numberOfCards = cardsInHand.Count;
                float twistPerCard = totalTwist / numberOfCards;
                float startTwist = -1f * (totalTwist / 2f);
                float twistForThisCard = startTwist +
                (i * twistPerCard);
                cardsInHand[i].GetComponent<Dragg>().rotate = new Vector3(0f, 0f, -twistForThisCard);
                float scalingFactor = 2f;

                float nudgeThisCard = Mathf.Abs(twistForThisCard);
                nudgeThisCard *= scalingFactor;
               

                cardsInHand[i].GetComponent<Dragg>().draggingPosition = new Vector3(offSetCards * i -170, 55 - nudgeThisCard, 0);


            }
        }

        for (int i = 0; i < cardsInEnemyHand.Count; i++)
        {
            if (cardsInEnemyHand[i].GetComponent<EnemyCardController>().canOrder)
            {

                RectTransform rtf = cardsInEnemyHand[i].transform.RT();


                rtf.anchorMin = new Vector2(0.5f, 1);
                rtf.anchorMax = new Vector2(0.5f, 1);

                cardsInEnemyHand[i].GetComponent<EnemyCardController>().isDragging = true;

                float totalTwist = 30f;

                int numberOfCards = cardsInEnemyHand.Count;
                float twistPerCard = totalTwist / numberOfCards;
                float startTwist = -1f * (totalTwist / 2f);
                float twistForThisCard = startTwist +
                (i * twistPerCard);
                cardsInEnemyHand[i].GetComponent<EnemyCardController>().rotate = new Vector3(0f, 0f, -twistForThisCard);
                float scalingFactor = 3f;

                float nudgeThisCard = Mathf.Abs(twistForThisCard);
                nudgeThisCard *= scalingFactor;

                cardsInEnemyHand[i].GetComponent<EnemyCardController>().dragPosition = new Vector3(177 - offSetCards * i, -10+nudgeThisCard,0);

            }
        }

    }
    void UpdateList()
    {
        for (int i = cardsInHand.Count - 1; i >= 0; i--)
        {
            if (cardsInHand[i] == null)
            {
                cardsInHand.RemoveAt(i);

            }
        }
        for (int i = cardsInEnemyHand.Count - 1; i >= 0; i--)
        {
            if (cardsInEnemyHand[i] == null)
            {
                cardsInEnemyHand.RemoveAt(i);

            }
        }
    }
    public void RemoveCardFromPlayerHand(int aIdSpawn)
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i].GetComponent<CardController>().idSpawnCard==aIdSpawn)
            {
                cardsInHand.RemoveAt(i);
            }
        }
    }
    public void RemoveCardFromEnemyHand(int aIdSpawn)
    {
        for (int i = 0; i < cardsInEnemyHand.Count; i++)
        {
            if (cardsInEnemyHand[i].GetComponent<EnemyCardController>().idSpawnCard == aIdSpawn)
            {
                GameObject card = cardsInEnemyHand[i];
                cardsInEnemyHand.RemoveAt(i);
                Destroy(card.gameObject);
            }
        }
    }
    public int GetCountCardsInHand()
    {
        return cardsInHand.Count;
    }
    public void EnemyLookingCard(int aIdSpawn, int aState)
    {
        for (int i = 0; i < cardsInEnemyHand.Count; i++)
        {
            if (cardsInEnemyHand[i].GetComponent<EnemyCardController>().idSpawnCard == aIdSpawn)
            {
                cardsInEnemyHand[i].GetComponent<EnemyCardController>().LookingCard(aState);

            }
        }
    }
    public void EnemyDraggingCard(int aIdSpawn, int aState, int aIdFloor)
    {
        for (int i = 0; i < cardsInEnemyHand.Count; i++)
        {
            if (cardsInEnemyHand[i].GetComponent<EnemyCardController>().idSpawnCard == aIdSpawn)
            {
                cardsInEnemyHand[i].GetComponent<EnemyCardController>().DragginCard(aState, aIdFloor);

            }
        }
    }
    public void DestroyEnemyCard(int aIdSpawn, int aIdFloor)
    {
        for (int i = 0; i < cardsInEnemyHand.Count; i++)
        {
            if (cardsInEnemyHand[i].GetComponent<EnemyCardController>().idSpawnCard == aIdSpawn)
            {
                if (GameController.instance.gameCards[cardsInEnemyHand[i].GetComponent<EnemyCardController>().idCard].TypeCard != 1)
                {
                    GameObject cardShowing = Instantiate(MatchController.instance.playerController.prefabCard[GameController.instance.gameCards[cardsInEnemyHand[i].GetComponent<EnemyCardController>().idCard].TypeCard], HandController.instance.transform.position, Quaternion.identity);
                    cardShowing.GetComponent<Dragg>().enabled = false;
                    cardShowing.GetComponent<CardController>().card = MatchController.instance.playerController.cards[cardsInEnemyHand[i].GetComponent<EnemyCardController>().idCard];
                    cardShowing.transform.SetParent(CMainCanvas.Inst.transform);
                    

                    cardShowing.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                    Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(BoardController.instance.groundList[aIdFloor].transform.position);
                    Vector2 WorldObject_ScreenPosition = new Vector2(
                        ((ViewportPosition.x * CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x) - (CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x * 0.5f)),
                        ((ViewportPosition.y * CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y) - (CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y * 0.5f)));
                    cardShowing.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    WorldObject_ScreenPosition.x += 80;
                    WorldObject_ScreenPosition.y += 50;
                    cardShowing.transform.RT().anchoredPosition = WorldObject_ScreenPosition;
                    
                }

                Destroy(cardsInEnemyHand[i].gameObject);

            }
        }
    }
}
