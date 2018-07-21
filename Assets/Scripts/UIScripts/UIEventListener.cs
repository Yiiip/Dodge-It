using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventListener : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	// 定义事件代理
	public delegate void UIEventProxy(GameObject gameObject);
	public delegate UnityEngine.Events.UnityAction<float> UIOnValueChangedProxy(float value);

	// 鼠标点击事件
	public event UIEventProxy OnClick;

	// 鼠标进入事件
	public event UIEventProxy OnMouseEnter;

	// 鼠标移出事件
	public event UIEventProxy OnMouseExit;

	public static UIEventListener Bind(GameObject gameObject)
	{
		UIEventListener listener = gameObject.GetComponent<UIEventListener>();
		if (listener == null)
		{
			listener = gameObject.AddComponent<UIEventListener>();
		}
		return listener;
	}

	public static UIEventListener Bind(Button button)
	{
		return Bind(button.gameObject);
	}

	public static void BindListener(Slider slider, UnityEngine.Events.UnityAction<float> valueChangedListener)
	{
		if (slider == null) return;
		slider.onValueChanged.RemoveAllListeners();
		slider.onValueChanged.AddListener(valueChangedListener);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnClick != null) OnClick(this.gameObject);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (OnMouseEnter != null) OnMouseEnter(this.gameObject);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (OnMouseExit != null) OnMouseExit(this.gameObject);
	}

}