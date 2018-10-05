using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowerScript : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Start() {
    }

    private void FixedUpdate() {
        if (GameWorld.Instance.State == EGameState.PLAYING && Camera.main.orthographicSize < 8.0f)
        {
            Camera.main.orthographicSize += Time.deltaTime * 5.0f;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;

        // transform.LookAt(target);
    }
}