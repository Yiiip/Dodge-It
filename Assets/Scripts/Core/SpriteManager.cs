using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager
{
	private static SpriteManager sInstance;
	private Dictionary<string, Sprite> mSpriteCache = new Dictionary<string, Sprite>();

	public static SpriteManager Instance
	{
		get
		{
			if (sInstance == null)
			{
				sInstance = new SpriteManager();
			}
			return sInstance;
		}
	}

	public Sprite GetSprite(string filepath)
	{
		if (!mSpriteCache.ContainsKey(filepath))
		{
			mSpriteCache.Add(filepath, Resources.Load<SpriteRenderer>(filepath).sprite);
		}
		return mSpriteCache[filepath];
	}

	public void OnDestory()
	{
		mSpriteCache.Clear();
		sInstance = null;
	}
}