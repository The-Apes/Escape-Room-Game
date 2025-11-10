using Managers;
using UnityEngine;

namespace ItemDescriptions
{
    public class InspectSimpleDescription : MonoBehaviour, IItemDescription
    {
        [SerializeField] private string description; 
        public void Describe()
        {
            if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
            DialogueManager.instance.SayLine(description, true);
        }
    }
}
