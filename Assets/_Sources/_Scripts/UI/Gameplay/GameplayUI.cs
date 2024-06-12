using System;
using Framework.DesignPattern.Observer;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class GameplayUI : MonoBehaviour
{
    private Question _currentQuestion;
    private TimeController _timer;
    
    
    [SerializeField] private TimeUI delayTimeUI;
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private QuestionUI questionUI;
    [SerializeField] private AnswerHolder answerHolder;

    private void Start()
    {
        _timer = new TimeController();
        
        
        this.RegisterListener(EventID.Answer, OnAnswer);
        this.RegisterListener(EventID.Play, OnPlay);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Answer, OnAnswer);
        this.RemoveListener(EventID.Play, OnPlay);
    }

    private void LateUpdate()
    {
        _timer.UpdateTime();
        timeUI.SetTime(_timer.TimeRemain);
        delayTimeUI.SetTime(_timer.DelayTime);
        
        if (_timer.DelayTime < 0)
        {
            _timer.DelayTime = 0;
            Setup(QuestionSystem.PrepareQuestion());
        } 
    }

    [Button]
    public void OnPlay(object obj)
    {
        _timer.Setup(90f);
    }

    public void Setup(Question question)
    {
        _currentQuestion = question;
        questionUI.Setup(question.question.ToString());
        answerHolder.Setup(question.questionOptions);
    }



    private void OnAnswer(object obj)
    {
        _timer.ResetDelay();
    }
}
