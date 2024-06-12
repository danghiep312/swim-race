using System;
using Framework.DesignPattern.Observer;
using Framework.DesignPattern.Singleton;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    
    [SerializeField] private GameplayUI gameplayUI;

    private void Start()
    {
        this.RegisterListener(EventID.Play, OnPlayGame);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Play, OnPlayGame);
    }

    private void OnPlayGame(object obj)
    {
        gameplayUI.Setup(QuestionSystem.PrepareQuestion());
    }
}