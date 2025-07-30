using System;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;
    
    private ChoiceBox choiceBox;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        choiceBox = FindFirstObjectByType<ChoiceBox>();
    }

    public void Ask(string question)
    {
        string remainder = question;
        string currentSample;
        int count = 0;
        while (remainder.Contains(','))
        {
            print(remainder);
            count++;
            currentSample = remainder.Substring(0, remainder.IndexOf(','));
            print(currentSample);
            choiceBox.AddChoice(currentSample, count);
            remainder = remainder.Substring(remainder.IndexOf(','));
            print("loop end");
        }
        choiceBox.ShowChoices(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
