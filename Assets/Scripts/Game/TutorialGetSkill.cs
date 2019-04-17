using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGetSkill : MonoBehaviour
{
	private void OnDestroy()
	{
		//专门掉落技能
		GameObject dropedObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Droped_Skill_ball"), this.transform.position, Quaternion.identity);
		dropedObj.transform.parent = null;
		dropedObj.GetComponent<DropedSkill>().Init(ESkillType.SKILL_01);
	}
}