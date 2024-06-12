using TMPro;
using UnityEngine;


public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    
    public void SetTime(float time)
    {
        timeText.text = time.ToString("mm:ss");
    }
    
}