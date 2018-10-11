using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class DropedSkill : MonoBehaviour
{
	[SerializeField] private ESkillType dropedSkillType;
	[SerializeField] private SpriteRenderer spriteIcon;

	private void Awake()
	{
		var playerPos = GameWorld.Instance.Player.transform.position;
		// Debug.Log("playerPos:" + playerPos.ToString());
		// Debug.Log("dropedPos:" + transform.position.ToString());
		var dv = (transform.position - playerPos).normalized;
		DOTweenAnimation[] doTweenAnims = GetComponents<DOTweenAnimation>();
		foreach (var anim in doTweenAnims)
		{
			if (anim.id == "3")
			{
				var l = Random.Range(0.5f, 1.0f);
				anim.endValueV3 = new Vector3(dv.x * l, dv.y * l, 0.0f);
				// Debug.Log(anim.endValueV3.ToString());
				break;
			}
		}
	}

	public void Init(ESkillType skillType)
	{
		this.dropedSkillType = skillType;

		switch (dropedSkillType)
		{
			case ESkillType.SKILL_01:
				spriteIcon.sprite = SpriteManager.Instance.GetSprite("Sprites/sSkillIcon01");
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