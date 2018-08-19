using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseSkill
{
	public ESkillType type;
	public string spriteRes;
	public string prefabRes;

	public abstract void UseSkill(Transform playerTrans);
}