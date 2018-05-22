using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrapController : NetworkBehaviour
{
    [SyncVar]
    public int idCard;
    [SyncVar]
    public int idFloor;
    [SyncVar]
    public int playerOwner;
	public int typeCard;
    GameObject cardShowing;
    bool showingCard;
    public bool goToCementery = true;

    void Start()
	{
		MatchController.instance.AddTrapInGame (this);
	}

    public void SetStats(int aIdCard,  int aIdFloor, int aPlayerOwner, int aTypeCard)
    {
        idCard = aIdCard;
        idFloor = aIdFloor;
        playerOwner = aPlayerOwner;
		typeCard = aTypeCard;

    }
    public void DestroyTrap()
    {
        if (cardShowing!=null)
        {
            Destroy(cardShowing.gameObject);
            MatchController.instance.playerController.SetOcupateFloor(idFloor, false, 1);
        }
        Destroy(gameObject);

    }
//este codigo esta repetido en playercontroller porque desde ahi se instancia a ambos jugadores
//sin embargo la logica de las trampas corren a nivel local en ambos jugadores
//entonces cada uno muestra la carta trampa que piso al ser activada sin enviar datos por la red
//si uso el showcard de playercontroller tener cuidado porque se va a mostrar 2 veces la carta
//porque tanto cliente como servidor van a ejecutar el codigo.
    public void ShowCard()
    {
        if (!showingCard) {
            cardShowing = Instantiate(MatchController.instance.playerController.prefabCard[typeCard], HandController.instance.transform.position, Quaternion.identity);
            cardShowing.GetComponent<Dragg>().enabled = false;
            cardShowing.GetComponent<CardController>().card = MatchController.instance.playerController.cards[idCard];
            cardShowing.transform.SetParent(CMainCanvas.Inst.transform);


            cardShowing.transform.localScale = new Vector3(1, 1, 1);

            cardShowing.transform.RT().anchorMin = new Vector2(0.5f, 0.5f);
            cardShowing.transform.RT().anchorMax = new Vector2(0.5f, 0.5f);
            cardShowing.transform.RT().anchoredPosition = new Vector2(0, 0);

            showingCard = true;
        }
    }
    private void OnDestroy()
    {
		CementeryController.instance.AddCardToCementery(idCard,playerOwner);
        MatchController.instance.activatingCard = false;
        if (cardShowing != null)
        {
            Destroy(cardShowing.gameObject);
        }
    }

}
