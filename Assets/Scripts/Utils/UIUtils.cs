using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class UIUtils
{
	public static void SetText(Text textView, string text)
	{
		if (textView != null) textView.text = text;
	}

	public static void SetText(InputField inputField, string text)
	{
		if (inputField != null) inputField.text = text;
	}

	public static void SetText(TextMeshProUGUI textPro, string text)
	{
		if (textPro != null) textPro.text = text;
	}

	public static void SetImage(Image imageView, string spritePath)
	{
		SetImage(imageView, (Resources.Load<SpriteRenderer>(spritePath)).sprite);
	}

	public static void SetImage(Image imageView, Sprite sprite)
	{
		if (imageView != null) imageView.sprite = sprite;
	}

	public static void SetImageFill(Image imageView, float fillAmount)
	{
		if (imageView != null) imageView.fillAmount = fillAmount;
	}

	public static void SetSliderValues(Slider slider, float min, float max, float value)
	{
		if (slider != null)
		{
			slider.minValue = min;
			slider.maxValue = max;
			slider.value = Mathf.Clamp(value, min, max);
		}
	}

	public static void SetColor(Graphic ui, Color color)
	{
		if (ui != null) ui.color = color;
	}

	public static void SetInteractable(Selectable ui, bool interactable)
	{
		if (ui != null) ui.interactable = interactable;
	}

	public static void SetVisibility(GameObject gameObject, bool visible)
	{
		if (gameObject != null) gameObject.SetActive(visible);
	}

	public static bool IsVisibility(GameObject gameObject)
	{
		if (gameObject != null) return gameObject.activeSelf;
		else return false;
	}

	public static void SetParent(GameObject view, Transform parentTrans)
	{
		if (view != null) view.transform.SetParent(parentTrans, false);
	}

	public static GameObject InstantiatePrefab(string path, GameObject parentGameObject)
	{
		return InstantiatePrefab(path, parentGameObject == null ? null : parentGameObject.transform);
	}
	
	public static GameObject InstantiatePrefab(string path, Transform parentTrans)
	{
		GameObject instantiation = InstantiatePrefab(Resources.Load<GameObject>(path), parentTrans);
		instantiation.name = path.Substring(path.LastIndexOf('/') + 1);
		return instantiation;
	}

	public static GameObject InstantiatePrefab(GameObject prefab, GameObject parentGameObject)
	{
		return InstantiatePrefab(prefab, parentGameObject == null ? null : parentGameObject.transform);
	}

	public static GameObject InstantiatePrefab(GameObject prefab, Transform parentTrans)
	{
		GameObject instantiation = GameObject.Instantiate(prefab) as GameObject;
		instantiation.SetActive(true);
		SetParent(instantiation, parentTrans);
		return instantiation;
	}

	public static void ScrollToTop(RectTransform rectTransform)
    {
        if (rectTransform != null) rectTransform.anchoredPosition = Vector2.zero;
    }

    public static void ScrollToTop(GridLayoutGroup gridLayoutGroup)
    {
        if (gridLayoutGroup != null) ScrollToTop(gridLayoutGroup.GetComponent<RectTransform>());
    }

    public static void ScrollToTop(VerticalLayoutGroup verticalLayoutGroup)
    {
        if (verticalLayoutGroup != null) ScrollToTop(verticalLayoutGroup.GetComponent<RectTransform>());
    }

    // 获取Camera的快照
    public static Texture2D RenderRTImage(Camera cam)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D tex2d = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        tex2d.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        tex2d.Apply();
        RenderTexture.active = currentRT;
        return tex2d;
    }

    // 获取三维模型的实际大小
    public static Vector3 Get3DModelSize(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        if (mesh == null) return Vector3.zero;

        Vector3 meshSize = mesh.bounds.size; //模型网格的大小
        Vector3 scale = gameObject.transform.lossyScale; //放缩
        return new Vector3(meshSize.x * scale.x, meshSize.y * scale.y, meshSize.z * scale.z); //游戏中的实际大小
    }
}