using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        float targetAspect = 20f / 10f;

        float currentAspect = (float)Screen.width / Screen.height;

        float scaleHeight = currentAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
    }
}
