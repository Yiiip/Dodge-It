using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet01 : MonoBehaviour
{
    public float Speed = 14f;

    private Vector2 mStartPos;
    private Vector2 mTargetPos;

    private void Start() {
        InitSelf();
    }

    protected void InitSelf()
    {
        this.mStartPos = this.transform.position;
        this.mTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float toTargetAngle = Mathf.Atan2(mTargetPos.y - mStartPos.y, mTargetPos.x - mStartPos.x) * Mathf.Rad2Deg;
        if (toTargetAngle < -90 || toTargetAngle > 90) { toTargetAngle = -toTargetAngle; }
        
        float farX = mTargetPos.x + 20.0f * Mathf.Sign(mTargetPos.x - mStartPos.x);
        float farY = mTargetPos.y + 20.0f * Mathf.Tan(toTargetAngle * Mathf.Deg2Rad);
        this.mTargetPos = new Vector2(farX, farY);
    }

    private void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, mTargetPos, Speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, mTargetPos) <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

}