using System;
using Managers;
using UI;
using UnityEngine;

namespace Objects
{
    public class Readable : MonoBehaviour, IClickable
    {
        [SerializeField] [TextArea(5,10)] private string text;
        
        public void OnClick(GameObject heldObject)
        {
            UIManager.instance.Read(text);
            PlayerFlagsManager.instance.ReadAnItem = true;
        }
    }
}
