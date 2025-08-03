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
        int choices = 0;
        while (remainder.Contains(','))
        {
            print("s");
            print(remainder);
            choices++;
            currentSample = remainder.Substring(0, remainder.IndexOf(','));
            print(currentSample);
            choiceBox.AddChoice(currentSample, choices);
            remainder = remainder.Substring(remainder.IndexOf(',')+1);
            print("loop end");
        }

        choices++;
        choiceBox.AddChoice(remainder, choices);
        
        choiceBox.ShowChoices(true);
    }

    public void ChosenChoice(int choice)
    {
        StartCoroutine(ScriptManager.instance.RunScript(ScriptManager.instance.currentScript.choices[choice]));
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
