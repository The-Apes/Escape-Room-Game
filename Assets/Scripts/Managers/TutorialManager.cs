using System;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;

namespace Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager instance;
        [SerializeField] private TextMeshProUGUI text;
        private Logs _logs;
        
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

            _logs = FindFirstObjectByType<Logs>();
        }
        
        private void Start()
        {
            text.gameObject.SetActive(false);
        }
        
        private IEnumerator ShowTutorial(string message, Func<bool> condition)
        {
            while (!condition())
            { 
                text.text = message;
                text.gameObject.SetActive(true);
                yield return null;
            }
            
            text.text = "";
            text.gameObject.SetActive(false);
            _logs.LogTutorial(message);
        }
        
        public void InteractTutorial()
        {
            StartCoroutine(ShowTutorial("Left Click to Interact/Pick Up Objects", () => PlayerFlagsManager.instance.PickedUpItem));
        }
        public void DropTutorial()
        {
            StartCoroutine(ShowTutorial("Press G to Drop Objects", () => PlayerFlagsManager.instance.DroppedAnItem));
        }
        
        public void InspectTutorial()
        {
            StartCoroutine(ShowTutorial("Press F to Inspect held Objects", () => PlayerFlagsManager.instance.InspectedAnItem));
        }
        
        public void ReadTutorial()
        {
            if(PlayerFlagsManager.instance.PickedUpItem && PlayerFlagsManager.instance.InspectedAnItem)
            {
                StartCoroutine(ShowTutorial("Items with text can be read if you click on them while inspecting", () => PlayerFlagsManager.instance.ReadAnItem));
            }
        }
        
        public void NpcTutorial()
        {
            //if(PlayerFlagsManager.instance.PickedUpItem && PlayerFlagsManager.instance.InspectedAnItem)
            {
                StartCoroutine(ShowTutorial("Left click the Lamp to talk to him", () => PlayerFlagsManager.instance.InteractedWithNpc));
            }
        }
        public void NpcTalkTutorial()
        {
            if(!PlayerFlagsManager.instance.SelectedChoice)
            {
                StartCoroutine(ShowTutorial("Use scroll wheel to scroll between choices, press space to select", () => PlayerFlagsManager.instance.SelectedChoice));
            }
        }
        public void NpcGiveTutorial()
        {
            if(!PlayerFlagsManager.instance.SelectedChoice)
            {
                StartCoroutine(ShowTutorial("Press G While looking at the Lamp to give him items", () => PlayerFlagsManager.instance.GaveNpcAnItem));
            }
        }
        
        public void CrouchTutorial()
        {
            StartCoroutine(ShowTutorial("Press C to Crouch", () => Input.GetKeyDown(KeyCode.C)));
        }
    }
}