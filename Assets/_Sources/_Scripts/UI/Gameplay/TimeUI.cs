using System;
using TMPro;
using UnityEngine;


public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    
    public void SetTime(float time)
    {
        timeText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
    }
    
}