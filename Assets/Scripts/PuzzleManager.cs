using System;
using UnityEngine;

public class PuzzleManager: MonoBehaviour
{
    public static PuzzleManager instance;
    
    public int puzzleStage; //Number representing how far the player is in terms of completing the escape room

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
