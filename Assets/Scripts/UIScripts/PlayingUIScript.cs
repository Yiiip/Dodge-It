using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayingUIScript : MonoBehaviour
{
	private const string STR_SCORE = "SCORE:";

	public Player PlayerRef;
	public GameObject MouseCursor;
	public TextMeshProUGUI textSocre;
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
		Cursor.visible = false;

		InitViews();
	}

	private void InitViews()
	{
		mLifeIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.DEFAULT_LIFE; i++)
		{
			GameObject lifeIcon = UIUtils.InstantiatePrefab(PrefabLifeIcon, ParentLifeIcons);
			lifeIcon.transform.position = new Vector3(lifeIcon.transform.position.x - i * 22, lifeIcon.transform.position.y, lifeIcon.transform.position.z);
			UIUtils.SetVisibility(lifeIcon, true);
			mLifeIconVisibleCount++;
			mLifeIcons.Add(lifeIcon);
		}

		mSkillIcons = new List<GameObject>();
		for (int i = 0; i < (int) Player.MAX_SKILL; i++)
		{
			GameObject skillIcon = UIUtils.InstantiatePrefab(PrefabSkillIcon, ParentSkillIcons);
			skillIcon.transform.position = new Vector3(skillIcon.transform.position.x - i * 22, skillIcon.transform.position.y, skillIcon.transform.position.z);
			UIUtils.SetVisibility(skillIcon, false);
			mLifeIcons.Add(skillIcon);
		}
	}

	void Update()
	{
		MouseCursor.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		UpdateScoreUI();
		UpdateLifeIcons();
		UpdateSkillIcons();
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
			textSocre,
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