using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStatus characterStatus;

    public void Reinit()
    {
        characterStatus.Reset();
    }
    
}

[Serializable]
public class CharacterStatus
{
    public int Id { get; private set; }
    public int Score { get; private set; }


    public void Reset()
    {
        Score = 0;
    }
    
    public void AddScore(int score)
    {
        Score += score;
    }
}