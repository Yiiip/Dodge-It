using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill01 : BaseSkill
{

	public Skill01()
	{
		type = ESkillType.SKILL_01;
		spriteRes = "Sprites/sSkillIcon01";
		prefabRes = "Prefabs/Bullet_Skill01";
	}

	public override void UseSkill(Transform playerTrans)
	{
		float r = 2.5f;
		for (int i = 0; i < 36; i++)
		{
			float angle = i * 10.0f;
			Vector2 skillBulletPos = new Vector2(
				playerTrans.position.x + r * Mathf.Sin(angle * Mathf.Deg2Rad),
				playerTrans.position.y + r * Mathf.Cos(angle * Mathf.Deg2Rad)
			);
			GameObject skillBulletObj = GameObject.Instantiate(Resources.Load<GameObject>(prefabRes) as GameObject, skillBulletPos, Quaternion.identity);
			skillBulletObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
		}
		AudioManager.Instance.PlaySound((int) AudioConstant.SKILL01);
	}
}