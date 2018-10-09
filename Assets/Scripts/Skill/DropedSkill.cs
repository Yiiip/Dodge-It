using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedSkill : MonoBehaviour
{
	[SerializeField] private ESkillType dropedSkillType;
	[SerializeField] private SpriteRenderer spriteIcon;

	public void Init(ESkillType skillType)
	{
		this.dropedSkillType = skillType;

		switch (dropedSkillType)
		{
			case ESkillType.SKILL_01:
				spriteIcon.sprite = Resources.Load<SpriteRenderer>("sSkillIcon01").sprite;
				break;
			case ESkillType.SKILL_02:
			case ESkillType.NONE:
			default:
				Debug.LogWarning("init DropedSkill failed! The type is NONE.");
				Destroy(this.gameObject);
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			GameWorld.Instance.Player.GetSkillMgr().AddSkill(1, this.dropedSkillType);
			AudioManager.Instance.PlaySound((int) AudioConstant.GET01);
			Destroy(this.gameObject);
		}
	}
}