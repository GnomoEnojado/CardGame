using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class CMainCanvas : MonoBehaviour {

	public static CMainCanvas Inst;
	private Canvas _canvas;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public void Awake()
	{
		Inst = this;
		_canvas = GetComponent<Canvas> ();
	}
    void Start()
    {

        m_Raycaster = GetComponent<GraphicRaycaster>();
  
        m_EventSystem = GetComponent<EventSystem>();
    }
    public Canvas GetCanvas()
	{
		return _canvas;
	}
    public List<RaycastResult> GetGraphicsElements()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
  
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        
        m_Raycaster.Raycast(m_PointerEventData, results);
        return results;
    }
}
