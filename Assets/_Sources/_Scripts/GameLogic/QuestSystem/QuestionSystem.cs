using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class QuestionSystem
{
    public static Question CurrentQuestion;

    public static bool CheckAnswer(AnswerOption answerOption)
    {
        return CurrentQuestion.question == answerOption.Value;
    }
    
    
    [Button]
    public static Question PrepareQuestion()
    {
        return CurrentQuestion = Question.CreateQuestion(1, 10, 3);
    }
}

[Serializable]
public class Question
{
    public int question;
    public List<int> questionOptions;
    
    public Question(int question, List<int> questionOptions)
    {
        this.question = question;
        this.questionOptions = questionOptions;
    }
    
    public static Question CreateQuestion(int start, int to, int quantity)
    {
        HashSet<int> options = new HashSet<int>();

        while (options.Count < quantity)
        {
            int option = Random.Range(start, to + 1);
            options.Add(option);
        }

        List<int> questionOptions = new List<int>(options);
        int question = questionOptions[Random.Range(0, questionOptions.Count)];
        
        return new Question(question, questionOptions);
    }
}

[Serializable]
public class AnswerOption
{
    public int Value { get; set; }
}