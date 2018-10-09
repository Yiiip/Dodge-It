using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Camera curCamera;

    public bool isCanShake = true;
    public float radio = 0.2f;

    private int shakeCount = 0;
    private Vector3 mOriginalPos; //相机抖动前的位置

    private void Start()
    {
        curCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (isCanShake)
        {
            ShakeWithCount();
        }
    }

    public void ShakeCameraWithRandomCount()
    {
        mOriginalPos = curCamera.transform.position;
        shakeCount = Random.Range(5, 14);
    }

    public void ShakeCameraWithCount(int shakeCount)
    {
        mOriginalPos = curCamera.transform.position;
        this.shakeCount = shakeCount;
    }

    private void ShakeWithCount()
    {
        if (shakeCount == 0)
        {
            return;
        }
        else if (shakeCount > 0)
        {
            shakeCount--;
            float r = Random.Range(-radio, radio); //随机的震动幅度
            if (shakeCount == 0)
            {
                //保证最终回归到原始位置
                curCamera.transform.position = mOriginalPos;
            }
            else
            {
                curCamera.transform.position = mOriginalPos + Vector3.one * r;
            }
        }
    }
}