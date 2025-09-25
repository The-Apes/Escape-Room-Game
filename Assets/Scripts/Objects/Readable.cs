using System;
using Managers;
using UI;
using UnityEngine;

namespace Objects
{
    public class Readable : MonoBehaviour, IClickable
    {
        [SerializeField] private String text;
        
        public void OnClick(GameObject heldObject)
        {
            UIManager.instance.Read(text);
        }
    }
}
