using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 16f;

    private Vector2 mStartPos;
    private Vector2 mTargetPos;

    private void Start() {
        InitSelf();
    }

    protected void InitSelf()
    {
        this.mStartPos = this.transform.position;
        // Debug.Log("子弹起始" + mStartPos);

        this.mTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mTargetPos.y - mStartPos.y, mTargetPos.x - mStartPos.x) * Mathf.Rad2Deg;
        if (angle < -90 || angle > 90) { angle = -angle; }
        float farX = mTargetPos.x + 20.0f * Mathf.Sign(mTargetPos.x - mStartPos.x);
        float farY = mTargetPos.y + 20.0f * Mathf.Tan(angle * Mathf.Deg2Rad);
        // Debug.Log("angle: " + angle);
        this.mTargetPos = new Vector2(farX, farY);
        // Debug.Log("子弹目标" + mTargetPos);
    }

    private void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, mTargetPos, Speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, mTargetPos) <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    // protected void OnTriggerEnter2D(Collider2D other) {
    //     if (other.CompareTag("MapBound"))
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }

    // protected void OnCollisionEnter2D(Collision2D other) {
    //     if (other.collider.CompareTag("MapBound"))
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}