using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardController : MonoBehaviour {
	public int idSpawnCard;
	public int idCard;
	public bool canOrder=true;
	float originalXPos;
	public bool isDragging;
	public Vector3 dragPosition;
    public Vector3 rotate;
    bool draggingInWorld;

    void Update()
	{
		if(isDragging)
		{
			Drag();
		}

	}
	public void LookingCard(int aState)
	{
		if (aState == 1) 
		{
            isDragging = true;
			canOrder = false;
			originalXPos = transform.RT ().anchoredPosition3D.x;
			dragPosition = new Vector3 (transform.RT ().anchoredPosition3D.x, -70,0);
			
		} 
		else if(aState==0)
		{
            isDragging = false;
            canOrder = true;
		}
	}
	public void DragginCard(int aState, int aIdFloor)
	{
		if (aState == 2) 
		{
            draggingInWorld = true;
            canOrder = false;
			RectTransform rtf = transform.RT();
		
			Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(BoardController.instance.groundList[aIdFloor].transform.position);
			Vector2 WorldObject_ScreenPosition=new Vector2(
				((ViewportPosition.x*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.x*0.5f)),
				((ViewportPosition.y*CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y)-(CMainCanvas.Inst.GetCanvas().transform.RT().sizeDelta.y)));
            WorldObject_ScreenPosition.x += 40;
            WorldObject_ScreenPosition.y += 60;


            isDragging = true;
			dragPosition = WorldObject_ScreenPosition;

        } 
        //no se esta usando
		else if (aState == 1) 
		{			
			isDragging = true;
			canOrder = false;
			RectTransform rtf = transform.RT();

			originalXPos = transform.RT ().anchoredPosition.x;
			
            dragPosition= new Vector3(transform.RT().anchoredPosition3D.x, -130,0);
        } 
		else if(aState==0)
		{
            draggingInWorld = false;
            isDragging = false;
			canOrder = true;
		}
	}
	private void Drag()
	{
        if (draggingInWorld)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0,0,0), 5 * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotate), 5 * Time.deltaTime);
        }

        transform.RT ().anchoredPosition3D= Vector3.Lerp(transform.RT ().anchoredPosition3D, dragPosition,6 * Time.deltaTime);

	}
}
