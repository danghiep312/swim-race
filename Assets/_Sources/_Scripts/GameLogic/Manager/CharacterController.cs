using System;
using System.Collections.Generic;
using Framework.DesignPattern.Observer;
using Framework.DesignPattern.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterController : Singleton<CharacterController>
{
    public List<Character> characters;
    
    [ReadOnly] public Character currentCharacter;
    
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    private float _distanceStep;
    
    [ReadOnly] public List<CharacterStatus> dashboard;

    public void Start()
    {
        _distanceStep = (rightPivot.position.x - leftPivot.position.x) / 10;
        
        foreach (Character character in characters)
        {
            dashboard.Add(character.characterStatus);
        }
        
        SetCurrentCharacter(characters[0]);
        
        this.RegisterListener(EventID.Play, OnPlayGame);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Play, OnPlayGame);
    }

    private void OnPlayGame(object obj)
    {
        ReInit();
        SetCharacterPosition();
    }


    public void SetCurrentCharacter(Character character)
    {
        currentCharacter = character;
    }

    public void ReInit()
    {
        foreach (Character character in characters)
        {
            character.ReInit();
        }
    }

    public void Answer(bool characterCorrect)
    {
        if (characterCorrect)
        {
            currentCharacter.AddScore();
        }
        
        OptimizeScoreOpponent();
        SetCharacterPosition();
    }
    
    public void OptimizeScoreOpponent()
    {
        
    }

    public void SetCharacterPosition()
    {
        int index = characters.IndexOf(currentCharacter);
        if (index != 1) characters.Swap(index, 1);
        foreach (Character character in characters)
        {
            character.transform.position = leftPivot.position + 
                                           Vector3.up * (characters.IndexOf(character) - 1) * 1f +
                                           Vector3.right * _distanceStep * character.Score;
        }
    }
}