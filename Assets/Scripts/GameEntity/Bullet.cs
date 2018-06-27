using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;

    private Vector2 mTargetPos;

    private void Start() {
        InitSelf();
    }

    protected void InitSelf()
    {
        this.Speed = 10.0f + Random.Range(-2, 2);
        this.mTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) *2;
    }

    private void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, mTargetPos, Speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, mTargetPos) <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("MapBound"))
        {
            Destroy(this.gameObject);
        }
    }
}