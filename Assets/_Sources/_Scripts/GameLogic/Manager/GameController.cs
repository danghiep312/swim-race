using System;
using Framework.DesignPattern.Observer;
using Framework.DesignPattern.Singleton;
using Sirenix.OdinInspector;

public class GameController : Singleton<GameController>
{
    private void Start()
    {
        this.RegisterListener(EventID.Answer, OnAnswer);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Answer, OnAnswer);
    }

    private void OnAnswer(object obj)
    {
        AnswerOption answer = (AnswerOption) obj;

        bool correct = QuestionSystem.CheckAnswer(answer);
        
        CharacterController.Instance.Answer(correct);
    }

    
    [Button]
    public void PlayGame()
    {
        this.PostEvent(EventID.Play);
    }
    
}