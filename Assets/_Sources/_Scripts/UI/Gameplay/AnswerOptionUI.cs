using System;
using Framework.DesignPattern.Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOptionUI : MonoBehaviour
{
    private AnswerOption _answer;

    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI answerText;

    private void Start()
    {
        btn.onClick.AddListener(OnAnswerSelected);
    }

    public void Setup(AnswerOption answer)
    {
        _answer = answer;
        answerText.text = answer.Value.ToString();
    }

    private void OnAnswerSelected()
    {
        this.PostEvent(EventID.Answer, _answer);
    }
}