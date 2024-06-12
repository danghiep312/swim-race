using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public CharacterStatus characterStatus;

    public int Score => characterStatus.score;
    
    public void ReInit()
    {
        characterStatus.Reset();
    }

    public void AddScore(int score = 1)
    {
        characterStatus.AddScore(score);        
    }

    public void RandomAdd()
    {
        characterStatus.AddScore(UnityEngine.Random.Range(0, 2));
    }
    
    
}

[Serializable]
public class CharacterStatus
{
    public int id;
    public int score;


    public void Reset()
    {
        score = 0;
    }
    
    public void AddScore(int score)
    {
        this.score += score;
    }
}