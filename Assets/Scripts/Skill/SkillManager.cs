using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillType
{
	NONE = -1,
	SKILL_01,
	SKILL_02,
	Count
}

public class SkillManager : MonoBehaviour
{
	[SerializeField] private int mSkillCount = 0;
	[SerializeField] private ESkillType mSkillType = ESkillType.NONE;

	private Dictionary<ESkillType, BaseSkill> mSkillFactory = new Dictionary<ESkillType, BaseSkill>();

	private void Start()
	{
		this.Init();
	}

	private void Init()
	{
		mSkillFactory.Clear();
		mSkillFactory.Add(ESkillType.SKILL_01, new Skill01());
	}

	public void AddSkill(int count = 1, ESkillType skillType = ESkillType.NONE)
	{
		if (mSkillType == ESkillType.NONE && skillType == ESkillType.NONE)
		{
			return;
		}

		if (skillType != ESkillType.NONE && skillType != mSkillType)
		{
			mSkillType = skillType;
		}
		mSkillCount += count;
	}

	public bool UseSkill()
	{
		if (mSkillCount <= 0 || mSkillType == ESkillType.NONE)
		{
			return false;
		}

		mSkillCount--;
		mSkillFactory[mSkillType].UseSkill(GameWorld.Instance.Player.transform);
		return true;
	}

	public int GetSkillCount()
	{
		return mSkillCount;
	}

	public BaseSkill GetSkillData()
	{
		if (mSkillFactory.ContainsKey(mSkillType))
		{
			return mSkillFactory[mSkillType];
		}
		else
		{
			return null;
		}
	}
}