﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayingUIScript : MonoBehaviour
{
	private const string STR_SCORE = "SCORE:";

	public GameObject MainMenu;
	public TextMeshProUGUI TextHitStart;
	public TextMeshProUGUI TextHighScore;
	public TextMeshProUGUI TextLastScore;
	private float mTextHitStartAlpha;
	private bool mTextHitStartAlphaFlag;
	public GameObject KeyboardHitArea;
	private Vector3 mKeyboardHitAreaOriginPos;
	public GameObject MainEnemies;
	public CustomImageEffect CustomRGBGlitchEffect;

	public Player PlayerRef;
	public GameObject MouseCursor;

	public GameObject PlayingHUD;
	public TextMeshProUGUI TextSocre;
	public TextMeshProUGUI TextNoSkill;
	public Image ImgSkill;
	public GameObject ParentLifeIcons;
	public GameObject ParentSkillIcons;
	public GameObject PrefabLifeIcon;
	public GameObject PrefabSkillIcon;

	public Image IconSound, IconMusic;
	public Slider SliderSound, SliderMusic;
	public Image AreaSoundSetting, AreaMusicSetting;

	private List<GameObject> mLifeIcons;
	private List<GameObject> mSkillIcons;
	private byte mLifeIconVisibleCount = 0;
	private byte mSkillIconVisibleCount = 0;

	void Start()
	{
		InitData();
		InitViews();
		InitEvents();
	}

	private void InitData()
	{
		mTextHitStartAlpha = TextHitStart.transform.localScale.y;
		if (mTextHitStartAlpha >= 1.0f) mTextHitStartAlphaFlag = true;
		else if (mTextHitStartAlpha <= 0.2f) mTextHitStartAlphaFlag = false;
	}

	private void InitViews()
	{
		mLifeIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.DEFAULT_LIFE; i++)
		{
			GameObject lifeIcon = UIUtils.InstantiatePrefab(PrefabLifeIcon, ParentLifeIcons);
			lifeIcon.transform.position = new Vector3(lifeIcon.transform.position.x - i * 18, lifeIcon.transform.position.y, lifeIcon.transform.position.z);
			UIUtils.SetVisibility(lifeIcon, true);
			mLifeIconVisibleCount++;
			mLifeIcons.Add(lifeIcon);
		}

		mSkillIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.MAX_SKILL; i++)
		{
			GameObject skillIcon = UIUtils.InstantiatePrefab(PrefabSkillIcon, ParentSkillIcons);
			skillIcon.transform.position = new Vector3(skillIcon.transform.position.x - i * 18, skillIcon.transform.position.y, skillIcon.transform.position.z);
			UIUtils.SetVisibility(skillIcon, false);
			mSkillIcons.Add(skillIcon);
		}

		mKeyboardHitAreaOriginPos = KeyboardHitArea.transform.position;

		SliderSound.value = AudioManager.Instance.SoundVolume;
		SliderMusic.value = AudioManager.Instance.MusicVolume;

		UIUtils.SetVisibility(MainMenu, true);
		UIUtils.SetVisibility(PlayingHUD, false);

		UIUtils.SetText(TextHighScore, "Best Score: <#FFFF00>" + PlayerPrefs.GetInt("_High_Score_", 0) + "</color>");
		UIUtils.SetText(TextLastScore, "Last Score: <#FFFF00>" + PlayerPrefs.GetInt("_Last_Score_", 0) + "</color>");
	}

	private void InitEvents()
	{
		UIEventListener.Bind(IconSound.gameObject).OnClick += OnClickEvent;
		UIEventListener.Bind(IconMusic.gameObject).OnClick += OnClickEvent;
		UIEventListener.Bind(IconSound.gameObject).OnMouseEnter += OnMouseEnterEvent;
		UIEventListener.Bind(IconMusic.gameObject).OnMouseEnter += OnMouseEnterEvent;
		UIEventListener.Bind(AreaSoundSetting.gameObject).OnMouseExit += OnMouseExitEvent;
		UIEventListener.Bind(AreaMusicSetting.gameObject).OnMouseExit += OnMouseExitEvent;
		UIEventListener.BindListener(SliderSound, OnSliderSoundListener);
		UIEventListener.BindListener(SliderMusic, OnSliderMusicListener);
	}

	private void OnClickEvent(GameObject go)
	{
		if (go == IconSound.gameObject)
		{
			UIUtils.SetVisibility(SliderMusic.gameObject, false);
			UIUtils.SetVisibility(SliderSound.gameObject, !SliderSound.gameObject.activeSelf);
		}
		else if (go == IconMusic.gameObject)
		{
			UIUtils.SetVisibility(SliderSound.gameObject, false);
			UIUtils.SetVisibility(SliderMusic.gameObject, !SliderMusic.gameObject.activeSelf);
		}
	}

	private void OnMouseEnterEvent(GameObject go)
	{
		if (go == IconSound.gameObject)
		{
			UIUtils.SetVisibility(SliderMusic.gameObject, false);
			UIUtils.SetVisibility(SliderSound.gameObject, true);
		}
		else if (go == IconMusic.gameObject)
		{
			UIUtils.SetVisibility(SliderSound.gameObject, false);
			UIUtils.SetVisibility(SliderMusic.gameObject, true);
		}
	}

	private void OnMouseExitEvent(GameObject go)
	{
		if (go == AreaSoundSetting.gameObject)
		{
			UIUtils.SetVisibility(SliderSound.gameObject, false);
		}
		else if (go == AreaMusicSetting.gameObject)
		{
			UIUtils.SetVisibility(SliderMusic.gameObject, false);
		}
	}

	private void OnSliderSoundListener(float value)
	{
		AudioManager.Instance.ChangeSoundVolume(value);
		AudioManager.Instance.PlaySound((int) AudioConstant.LIFE_GET);
	}

	private void OnSliderMusicListener(float value)
	{
		AudioManager.Instance.ChangeMusicVolume(value);
	}

	void Update()
	{
		switch (GameWorld.Instance.State)
		{
			case EGameState.MAIN_MENU:
			{
				if (!Cursor.visible || MouseCursor.activeSelf)
				{
					Cursor.visible = true;
					MouseCursor.transform.rotation = Quaternion.identity;
					UIUtils.SetVisibility(MouseCursor, false);
				}
				if (!UIUtils.IsVisibility(MainMenu))
				{
					UIUtils.SetVisibility(PlayingHUD, false);
					UIUtils.SetVisibility(MainMenu, true);
				}
				UpdateMainMenuUIEffects();

				if (Input.GetKeyDown(KeyCode.Escape))
				{
					#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
					#else
						Application.Quit();
					#endif
				}

				if (Input.GetKeyDown(KeyCode.Space))
				{
					GameWorld.Instance.State = EGameState.PLAYING;
					AudioManager.Instance.PlaySound((int) AudioConstant.START);
				}
				break;
			}
			case EGameState.PLAYING:
			{
				if (Cursor.visible)
				{
					Cursor.visible = false;
					UIUtils.SetVisibility(MouseCursor, true);
				}
				if (!UIUtils.IsVisibility(PlayingHUD))
				{
					UIUtils.SetVisibility(MainMenu, false);
					UIUtils.SetVisibility(PlayingHUD, true);
					GameWorld.Instance.postProcessProfile.RemoveSettings<DepthOfField>();
					GameWorld.Instance.postProcessProfile.GetSetting<ChromaticAberration>().intensity.value = 0.0f;
					CustomRGBGlitchEffect.enabled = false;
					Destroy(MainEnemies);
				}

				MouseCursor.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);
				MouseCursor.transform.Rotate(0.0f, 0.0f, 1.0f);

				UpdateScoreUI();
				UpdateLifeIcons();
				UpdateSkillIcons();
				break;
			}
			default:
				break;
		}
	}

	private void UpdateMainMenuUIEffects()
	{

		if (mTextHitStartAlphaFlag) mTextHitStartAlpha -= Time.deltaTime * 1.2f;
		else mTextHitStartAlpha += Time.deltaTime * 1.2f;
		TextHitStart.color = new Color(TextHitStart.color.r, TextHitStart.color.g, TextHitStart.color.b, mTextHitStartAlpha);
		
		if (mTextHitStartAlpha >= 1.0f)
		{
			mTextHitStartAlpha = 1.0f;
			mTextHitStartAlphaFlag = true;
		}
		else if (mTextHitStartAlpha <= 0.2f)
		{
			mTextHitStartAlpha = 0.2f;
			mTextHitStartAlphaFlag = false;
		}
		
		if (mTextHitStartAlphaFlag) TextHitStart.transform.localScale -= new Vector3(mTextHitStartAlpha * 0.002f, mTextHitStartAlpha * 0.002f, 0.0f);
		else TextHitStart.transform.localScale += new Vector3(mTextHitStartAlpha * 0.002f, mTextHitStartAlpha * 0.002f, 0.0f);

		//Move ui around mouse
		Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		mouseViewportPos = mouseViewportPos * 2 - Vector2.one;
		Vector3 newPos = KeyboardHitArea.transform.position + new Vector3(mouseViewportPos.x, mouseViewportPos.y, 0.0f);
		if (Mathf.Abs(newPos.x - mKeyboardHitAreaOriginPos.x) < 10.0f)
		{
			KeyboardHitArea.transform.position = new Vector3(newPos.x, KeyboardHitArea.transform.position.y, KeyboardHitArea.transform.position.z);
		}
		if (Mathf.Abs(newPos.y - mKeyboardHitAreaOriginPos.y) < 10.0f)
		{
			KeyboardHitArea.transform.position = new Vector3(KeyboardHitArea.transform.position.x, newPos.y, KeyboardHitArea.transform.position.z);
		}
	}

	private void UpdateScoreUI()
	{
		int score = PlayerRef.Score;
		int zeorNum = score == 0 ? (Player.MAX_SCORE.ToString().Length) : (Player.MAX_SCORE.ToString().Length - score.ToString().Length);
		string zeroStrs = "";
		for (int i = 0; i < zeorNum; i++)
		{
			zeroStrs += "0";
		}
		UIUtils.SetText(
			TextSocre,
			STR_SCORE + (score == 0 ? "" : score + "") + "<#BDBDBD>" + zeroStrs + "</color>"
		);
	}

	private void UpdateLifeIcons()
	{
		if (mLifeIconVisibleCount != (byte) PlayerRef.Life)
		{
			mLifeIconVisibleCount = (byte) PlayerRef.Life;
			for (int i = mLifeIcons.Count; i >= 1; --i)
			{
				UIUtils.SetVisibility(mLifeIcons[i - 1], i <= PlayerRef.Life);
			}
		}
		else
		{
			return;
		}
	}

	private void UpdateSkillIcons()
	{
		UIUtils.SetVisibility(TextNoSkill.gameObject, PlayerRef.SkillCount == 0);
		
		BaseSkill data = PlayerRef.SkillData;
		UIUtils.SetVisibility(ImgSkill.gameObject, data != null);
		if (data != null)
		{
			ImgSkill.SetImage(data.spriteRes);
		}

		if (mSkillIconVisibleCount != (byte) PlayerRef.SkillCount)
		{
			mSkillIconVisibleCount = (byte) PlayerRef.SkillCount;
			for (int i = mSkillIcons.Count; i >= 1; --i)
			{
				UIUtils.SetVisibility(mSkillIcons[i - 1], i <= PlayerRef.SkillCount);
			}
		}
		else
		{
			return;
		}
	}
}