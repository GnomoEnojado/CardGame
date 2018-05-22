using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragg : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {


    public bool canOrder = true;
    public Card card;
  
    public bool isDragging;
    public Vector3 draggingPosition;
    public bool draggingInWorld;
    public Vector3 delta = Vector3.zero;
    private Vector3 lastPos  = Vector3.zero;
    int angleClamp=40;
    public Vector3 rotate;
    bool lookingCard;

    public void Start()
    {
        card = GetComponent<CardController>().card;


    }
    private void Update()
    {
       
        if (isDragging)
        {
            DragCard();
        }
        if (draggingInWorld)
        {
            DraggingInWorld();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (MatchController.instance.isPlaying() && !MatchController.instance.activatingCard &&( MatchController.instance.idDraggingCard==GetComponent<CardController>().idSpawnCard|| MatchController.instance.idDraggingCard ==-1))
        {
            lookingCard = false;
            isDragging = false;
            canOrder = false;
            MatchController.instance.draggingCard = true;
            MatchController.instance.idDraggingCard = GetComponent<CardController>().idSpawnCard;
            draggingInWorld = true;

            if (BoardController.instance.floorOver != null)
            {
                MatchController.instance.playerController.EnemyDraggingCard(GetComponent<CardController>().idSpawnCard, 2, BoardController.instance.floorOver.idFloor);

            }


        }

    }
    private void DraggingInWorld()
    {
		Vector2 input = Input.mousePosition;
        

        
		delta =  Camera.main.transform.worldToLocalMatrix * Input.mousePosition.ScreenToWorld(Camera.main,CMainCanvas.Inst.GetCanvas().planeDistance) - 
				 Camera.main.transform.worldToLocalMatrix * lastPos.ScreenToWorld(Camera.main,CMainCanvas.Inst.GetCanvas().planeDistance);
		delta *= 100;
        float mousePosY = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        float dist = 1 - mousePosY;
       

        if (dist >= 0.6f && dist <= 1)
        {
            input.x += ((transform.RT().rect.width * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2) * dist;
            input.y += ((transform.RT().rect.height * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2) * dist;
            transform.localScale = new Vector3(dist, dist, dist);
        }
        else if(dist < 0.6f)
        {
            input.x += ((transform.RT().rect.width * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2) * 0.6f;
            input.y += ((transform.RT().rect.height * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2) * 0.6f;
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else
        {
            input.x += ((transform.RT().rect.width * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2);
            input.y += ((transform.RT().rect.height * CMainCanvas.Inst.GetCanvas().scaleFactor) / 2);
            transform.localScale = new Vector3(1,1,1);
        }
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Mathf.Clamp(delta.y,-angleClamp, angleClamp), Mathf.Clamp(-delta.x,-60,60), 0), 5 * Time.deltaTime);

		Vector3 worldPos = input.ScreenToWorld (Camera.main,CMainCanvas.Inst.GetCanvas().planeDistance);
		transform.RT().position = Vector3.Lerp(transform.RT().position, worldPos, 7 * Time.deltaTime);


        lastPos = Input.mousePosition;
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (MatchController.instance.draggingCard) {

            MatchController.instance.idDraggingCard = -1;
            canOrder = true;
            draggingInWorld = false;

            if (BoardController.instance.floorOver != null)
            {
                if (MatchController.instance.SummonCard(card, BoardController.instance.floorOver))
                {                    
                    MatchController.instance.playerController.DestroyEnemyCard(GetComponent<CardController>().idSpawnCard, BoardController.instance.floorOver.idFloor);
                    Destroy(gameObject);
                }

                MatchController.instance.draggingCard = false;


            }
            else
            {

                MatchController.instance.draggingCard = false;

            }
           
            MatchController.instance.playerController.EnemyDraggingCard(GetComponent<CardController>().idSpawnCard, 0, -1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

      //menos de 100 me asegura que esta la carta por debajo de la pantalla ya que el learp va hasta 55
        if (!MatchController.instance.draggingCard)
        {     
			if (transform.RT().anchoredPosition3D.y <= 100) {
                isDragging = true;
                canOrder = false;
                lookingCard = true;

                if (transform.GetSiblingIndex() == HandController.instance.cardsInHand.Count - 1)
                {
					draggingPosition = new Vector3(transform.RT().anchoredPosition3D.x, 125,0);

                }
                else
                {
					draggingPosition = new Vector3(transform.RT().anchoredPosition3D.x - 45, 125,0);
                }
                transform.SetAsLastSibling();
                MatchController.instance.playerController.EnemyLookingCard(GetComponent<CardController>().idSpawnCard, 1);
            }

        }

    }
    private void DragCard ()
    {

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), 5 * Time.deltaTime);
        if (lookingCard)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0,0,0), 5 * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotate), 5 * Time.deltaTime);
        }
      

        transform.RT().anchoredPosition3D = Vector3.Lerp(transform.RT().anchoredPosition3D, draggingPosition, 5 * Time.deltaTime);
     
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!MatchController.instance.draggingCard|| MatchController.instance.idDraggingCard!=GetComponent<CardController>().idSpawnCard)
        {
            lookingCard = false;
            isDragging = false;
            canOrder = true;
			MatchController.instance.playerController.EnemyLookingCard (GetComponent<CardController>().idSpawnCard,0);
        }
    }


}
