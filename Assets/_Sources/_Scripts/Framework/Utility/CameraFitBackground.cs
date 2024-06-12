    using System;
using UnityEngine;


public class CameraFitBackground : MonoBehaviour
{
    public Camera[] cam;
    public SpriteRenderer risk;
    public FitMode fitMode;

    private void Start()
    {
        float orthographicSize = fitMode == FitMode.Width ? risk.bounds.size.x * Screen.height / Screen.width * 0.5f : risk.bounds.size.y * 0.5f;
        foreach (Camera c in cam)
        {
            c.orthographicSize = orthographicSize;
        }
    }
}