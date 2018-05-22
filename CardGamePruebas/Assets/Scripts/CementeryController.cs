using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CementeryController : MonoBehaviour
{
    public static CementeryController instance = null;

    public List<int> cementeryPlayer1 = new List<int>();
    public List<int> cementeryPlayer2 = new List<int>();
    public List<int> temp = new List<int>();

    public HorizontalScrollSnap scroll;
    public GameObject showCementery;
    public int viewingCementeryPlayer;

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

    public void AddCardToCementery(int aIdCard, int aPlayerOwner)
    {
        if (aPlayerOwner == 1)
        {
            cementeryPlayer1.Add(aIdCard);
        }
        else
        {
            cementeryPlayer2.Add(aIdCard);
        }
    }

    public void RemoveCardFromCementery(int aIdCard, int aPlayerOwner)
    {
        if (aPlayerOwner == 1)
        {
            cementeryPlayer1.Remove(aIdCard);
        }
        else
        {
            cementeryPlayer2.Remove(aIdCard);
        }
    }

    public List<int> GetCardInCementery(int aPlayerOwner, int aType)
    {
        //atype -1 para devolver todas las cartas
        temp.Clear();
        if (aPlayerOwner == 1)
        {
            if (aType != -1)
            {
                for (int i = 0; i < cementeryPlayer1.Count; i++)
                {
                    if (MatchController.instance.playerController.cards[cementeryPlayer1[i]].TypeCard == aType)
                    {
                        temp.Add(cementeryPlayer1[i]);
                    }
                }
            }
            else
            {
                return cementeryPlayer1;
            }


        }
        else if (aPlayerOwner == 2)
        {
            if (aType != -1)
            {
                for (int i = 0; i < cementeryPlayer2.Count; i++)
                {
                    if (MatchController.instance.playerController.cards[cementeryPlayer2[i]].TypeCard == aType)
                    {
                        temp.Add(cementeryPlayer2[i]);
                    }
                }
            }
            else
            {
                return cementeryPlayer2;
            }

        }
        else
        {
            if (aType != -1)
            {
                for (int i = 0; i < cementeryPlayer1.Count; i++)
                {
                    if (MatchController.instance.playerController.cards[cementeryPlayer1[i]].TypeCard == aType)
                    {
                        temp.Add(cementeryPlayer1[i]);
                    }
                }
                for (int i = 0; i < cementeryPlayer2.Count; i++)
                {
                    if (MatchController.instance.playerController.cards[cementeryPlayer2[i]].TypeCard == aType)
                    {
                        temp.Add(cementeryPlayer2[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < cementeryPlayer1.Count; i++)
                {
                    temp.Add(cementeryPlayer1[i]);
                }
                for (int i = 0; i < cementeryPlayer2.Count; i++)
                {
                    temp.Add(cementeryPlayer2[i]);
                }
            }
         

        }
        return temp;
    }

    public void ShowCardsInCementery(List<int> aIdCards)
    {
        if (showCementery.activeSelf)
        {
            GameObject[] a;
            scroll.RemoveAllChildren(out a);
        }
        if (aIdCards.Count>0) {
          
            showCementery.SetActive(true);
   
            for (int i = 0; i < aIdCards.Count; i++)
            {
                Card card = MatchController.instance.playerController.cards[aIdCards[i]];
                GameObject cardInstance = Instantiate(MatchController.instance.playerController.prefabCard[card.TypeCard], HandController.instance.transform.position, Quaternion.identity);
                cardInstance.GetComponent<CardController>().card = card;
                cardInstance.GetComponent<CardController>().DontDestroyCard();
                cardInstance.GetComponent<Dragg>().enabled = false;
                scroll.AddChild(cardInstance);
            }
        }
    }
    public void HideCardsInCementery()
    {
        GameObject[] a;
        scroll.RemoveAllChildren(out a);
        showCementery.SetActive(false);

    }
}
