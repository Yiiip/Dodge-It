using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpritePrefabGenerator
{
    [MenuItem("Tools/UI/Generate prefabs by selected Texture2Ds")]
    private static void GeneratePrefabByTexture2Ds()
    {
        if (Selection.objects.Length > 0)
        {
            string prefabPath = "Assets/Resources/Sprites/";

            foreach (Texture2D pngTexture in Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets))
            {
                GameObject existPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + pngTexture.name + ".prefab");
                if (existPrefab == null)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(pngTexture));
                    if (sprite != null)
                    {
                        GameObject gameObj = new GameObject();
                        SpriteRenderer spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
                        spriteRenderer.sprite = sprite;
                        GameObject newPrefab = PrefabUtility.CreatePrefab(prefabPath + sprite.name + ".prefab", gameObj);
                        GameObject.DestroyImmediate(gameObj);

                        Debug.Log("Prefab of sprite generated: " + pngTexture.name, newPrefab);
                    }
                    else
                    {
                        Debug.LogError(pngTexture.name + "is not a sprite!", pngTexture);
                    }
                }
                else
                {
                    Debug.LogWarning("Prefab of sprite has already exists! " + pngTexture.name, existPrefab);
                }
            }
        }
        else
        {
            Debug.LogError("Please select one or mutilple .png files or folder first!");
        }
    }
}