using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour {
    public Card card;
    public Text Name;
    public Text Effect;
	public int idSpawnCard;

    public Image artImage;
    public Text seCost;
    public Text attack;
    public Text defense;
    public Text hp;
    public Text velocity;
    public Image[] typeAttack;
    float timer;
    private bool dontDestroyCard;

    // Use this for initialization
    void Start () {
        if (card.TypeCard==0)
        {
            Name.text = card.name.ToString();
            Effect.text = card.Effect.ToString();
            artImage.sprite = card.artImage;
            seCost.text = card.seCost.ToString();
            attack.text = card.attack.ToString();
            defense.text = card.defense.ToString();
            hp.text = card.hp.ToString();
            velocity.text = card.velocity.ToString();
            if (card.typeAttack>0)
            {
                //control para sir rotem, si se agrega un icono exclusivo para su
                //tipo de ataque, sacar este if else y dejar solo la linea del else
                if (card.typeAttack==3)
                {
                    typeAttack[0].enabled = true;
                }
                else
                {
                    typeAttack[card.typeAttack - 1].enabled = true;
                }
            }
        }
        else if (card.TypeCard==1 || card.TypeCard == 2)
        {
            Name.text = card.name.ToString();
            Effect.text = card.Effect.ToString();
            seCost.text = card.seCost.ToString();
            artImage.sprite = card.artImage;
        }

    }
    private void Update()
    {
        //si dragg esta desactivado es porque el objeto es una carta que se esta mostrando
        //porque se termina de jugar
        if (!GetComponent<Dragg>().enabled && !dontDestroyCard)
        {
            timer += Time.deltaTime;
            if (timer>=2)
            {
                MatchController.instance.playerController.showingCard = false;
                Destroy(this.gameObject);
            }

        }
    }
    public void DontDestroyCard()
    {
        dontDestroyCard = true;
    }
    public bool GetDontDestroyCard()
    {
        return dontDestroyCard;
    }
}
