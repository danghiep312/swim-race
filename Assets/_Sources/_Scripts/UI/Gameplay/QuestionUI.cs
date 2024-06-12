using TMPro;
using UnityEngine;

public class QuestionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    
    public void Setup(string question)
    {
        questionText.text = question;
    }
}