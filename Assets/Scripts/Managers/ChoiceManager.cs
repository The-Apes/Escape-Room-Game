using Npc;
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
            if (ScriptManager.instance.MostRecentScript.choices[choice]) // If there is a script associated with this choice
            {
                ScriptManager.instance.RunScript(ScriptManager.instance.MostRecentScript.choices[choice], true);
            }
            else // No script associated, just continue
            {
                ScriptManager.instance.NextLine();
            }

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
