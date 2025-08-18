using UI;
using UnityEngine;

namespace Managers
{
    public class ChoiceManager : MonoBehaviour
    {
        public static ChoiceManager instance;
    
        private ChoiceBox _choiceBox;

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
        
            _choiceBox = FindFirstObjectByType<ChoiceBox>();
        }

        public void Ask(string question)
        {
            string remainder = question;
            string currentSample;
            int choices = 0;
            while (remainder.Contains(','))
            {
                choices++;
                currentSample = remainder.Substring(0, remainder.IndexOf(','));
                _choiceBox.AddChoice(currentSample, choices);
                remainder = remainder.Substring(remainder.IndexOf(',')+1);
            }

            choices++;
            _choiceBox.AddChoice(remainder, choices);
        
            _choiceBox.ShowChoices(true);
        }

        public void ChosenChoice(int choice)
        {
            StartCoroutine(ScriptManager.instance.RunScriptCoroutine(ScriptManager.instance.CurrentScript.choices[choice]));
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
}
