using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    public static GameController instance =null;
    public List<int> idCardsPlayer;
    public Card[] gameCards;
    public List<int> deck;
    public List<GameObject> prefabCard;

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
        DontDestroyOnLoad(this);
        LoadGameCards();
    }
    public void ChangeScene(string aNameScene)
    {
        SceneManager.LoadSceneAsync(aNameScene);
    }
    private void LoadGameCards()
    {
        Object[] cards = Resources.LoadAll("Cards", typeof(Card));
        gameCards = new Card[cards.Length];
        foreach (Card card in cards)
        {
            gameCards[card.Id] = card;
        }
    }
}
