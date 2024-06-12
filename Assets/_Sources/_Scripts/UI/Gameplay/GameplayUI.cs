using System;
using TMPro;
using UnityEngine;


public class GameplayUI : MonoBehaviour
{
    private TimeController _timer;
    [SerializeField] private TimeUI timeUI;
    
    private void LateUpdate()
    {
        _timer.UpdateTime();
        
        
        timeUI.SetTime(_timer.TimeRemain);
    }
}
