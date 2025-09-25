using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        
       [SerializeField] private ReadingPanel readingPanel;
        
        

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }
        private void Start()
        {
            if(!readingPanel) Debug.LogWarning("No Reading Panel assigned in UIManager, bozo");
        }

        public void Read(string textStr)
        {
            readingPanel.Read(textStr);
        }
    }
}
