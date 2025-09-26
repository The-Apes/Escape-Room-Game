using System;
using Managers;
using UI;
using UnityEngine;

namespace Objects
{
    public class ReadableInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private String text;
        
        public void OnInteract(GameObject heldObject)
        {
            UIManager.instance.Read(text);
        }
    }
}
