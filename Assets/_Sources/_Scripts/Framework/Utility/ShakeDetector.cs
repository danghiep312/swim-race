using Framework.DesignPattern.Singleton;
using Unity.Mathematics;
using UnityEngine;

public class ShakeDetector : Singleton<ShakeDetector>
{
    public bool isShaking;

    public float ShakeDetectionThreshold;
    public float MinShakeInterval;

    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;
    

    void Start () 
    {
        sqrShakeDetectionThreshold = math.pow(ShakeDetectionThreshold, 2);
    }
	
    void Update () 
    {
//        Debug.Log(Input.acceleration.sqrMagnitude);
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold
            && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval)
        {
            Debug.Log(Input.acceleration.sqrMagnitude);
           
            isShaking = true;
            timeSinceLastShake = Time.unscaledTime;
        }
        else
        {
            isShaking = false;
        }
    }
}
