using System;
using UnityEngine;

namespace Objects
{
    public class Readable : MonoBehaviour, IClickable
    {
        [SerializeField] private String text;
        
        public void OnClick(GameObject heldObject)
        {
            // Display the text in the UI
            //FindObjectOfType<UI.UIManager>().ShowText(text);
        }
    }
}
