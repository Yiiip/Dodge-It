using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing.Utilities;

public class PlayingUIScript : MonoBehaviour
{
	private const string STR_SCORE = "SCORE:";

	public GameObject MainMenu;
	public TextMeshProUGUI TextHitStart;
	private float mTextHitStartAlpha;
	private bool mTextHitStartAlphaFlag;
	public GameObject MainEnemies;
	private PostProcessingController ppController;

	public Player PlayerRef;
	public GameObject MouseCursor;

	public GameObject PlayingHUD;
	public TextMeshProUGUI TextSocre;
	public GameObject ParentLifeIcons;
	public GameObject ParentSkillIcons;
	public GameObject PrefabLifeIcon;
	public GameObject PrefabSkillIcon;

	private List<GameObject> mLifeIcons;
	private List<GameObject> mSkillIcons;
	private byte mLifeIconVisibleCount = 0;
	private byte mSkillIconVisibleCount = 0;

	void Start()
	{
		InitData();
		InitViews();
	}

	private void InitData()
	{
		mTextHitStartAlpha = TextHitStart.transform.localScale.y;
		if (mTextHitStartAlpha >= 1.0f) mTextHitStartAlphaFlag = true;
		else if (mTextHitStartAlpha <= 0.04f) mTextHitStartAlphaFlag = false;

		ppController = Camera.main.GetComponent<PostProcessingController>();
	}

	private void InitViews()
	{
		mLifeIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.DEFAULT_LIFE; i++)
		{
			GameObject lifeIcon = UIUtils.InstantiatePrefab(PrefabLifeIcon, ParentLifeIcons);
			lifeIcon.transform.position = new Vector3(lifeIcon.transform.position.x - i * 17, lifeIcon.transform.position.y, lifeIcon.transform.position.z);
			UIUtils.SetVisibility(lifeIcon, true);
			mLifeIconVisibleCount++;
			mLifeIcons.Add(lifeIcon);
		}

		mSkillIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.MAX_SKILL; i++)
		{
			GameObject skillIcon = UIUtils.InstantiatePrefab(PrefabSkillIcon, ParentSkillIcons);
			skillIcon.transform.position = new Vector3(skillIcon.transform.position.x - i * 17, skillIcon.transform.position.y, skillIcon.transform.position.z);
			UIUtils.SetVisibility(skillIcon, false);
			mLifeIcons.Add(skillIcon);
		}
	}

	void Update()
	{
		if (GameWorld.Instance.State == GameState.MAIN_MENU)
		{
			if (!Cursor.visible || MouseCursor.active)
			{
				Cursor.visible = true;
				UIUtils.SetVisibility(MouseCursor, false);
			}
			if (!UIUtils.IsVisibility(MainMenu))
			{
				UIUtils.SetVisibility(PlayingHUD, false);
				UIUtils.SetVisibility(MainMenu, true);
				ppController.enableDepthOfField = true;
			}
			UpdateMainMenuUIEffects();

			if (Input.GetMouseButtonDown(0))
			{
				GameWorld.Instance.State = GameState.PLAYING;
			}
		}
		else if (GameWorld.Instance.State == GameState.PLAYING)
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
				ppController.enableDepthOfField = false;
				Destroy(MainEnemies);
			}
			MouseCursor.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
			UpdateScoreUI();
			UpdateLifeIcons();
			UpdateSkillIcons();
		}
	}

	private void UpdateMainMenuUIEffects()
	{
		if (mTextHitStartAlpha >= 1.0f) mTextHitStartAlphaFlag = true;
		else if (mTextHitStartAlpha <= 0.04f) mTextHitStartAlphaFlag = false;

		if (mTextHitStartAlphaFlag) mTextHitStartAlpha -= Time.deltaTime * 1.2f;
		else mTextHitStartAlpha += Time.deltaTime * 1.2f;

		// TextHitStart.transform.localScale = new Vector2(1.0f, mTextHitStartScaleY);
		TextHitStart.color = new Color(TextHitStart.color.r, TextHitStart.color.g, TextHitStart.color.b, mTextHitStartAlpha);
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
			STR_SCORE + (score == 0 ? "" : score + "") + "<#A0A0A0>" + zeroStrs + "</color>"
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
		if (mSkillIconVisibleCount != (byte) PlayerRef.Skill)
		{
			mSkillIconVisibleCount = (byte) PlayerRef.Skill;
			for (int i = mSkillIcons.Count; i >= 1; --i)
			{
				UIUtils.SetVisibility(mSkillIcons[i - 1], i <= PlayerRef.Skill);
			}
		}
		else
		{
			return;
		}
	}
}