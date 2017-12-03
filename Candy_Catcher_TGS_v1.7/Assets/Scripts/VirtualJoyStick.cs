using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private Image bgImg;
	private Image joystickImg;

	public virtual void OnDrag(PointerEventData pointerEd)
	{

	}
	public virtual void OnPointerDown(PointerEventData pointerEd)
	{
		OnDrag (pointerEd);
	}
	public virtual void OnPointerUp(PointerEventData pointerEd)
	{

	}

}
