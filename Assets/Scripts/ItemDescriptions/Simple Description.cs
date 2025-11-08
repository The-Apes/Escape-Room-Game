using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions
{
    public class SimpleDescription : MonoBehaviour, IItemDescription
    {
        [SerializeField] private string description; 
        [SerializeField] private bool player;
        public void Describe()
        {
            DialogueManager.instance.SayLine(description, player);
        }
    }
}
