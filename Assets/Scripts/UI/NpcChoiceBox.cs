using System.Collections.Generic;
using Npc;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class NpcChoiceBox : MonoBehaviour
    {
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private CanvasGroup canvasGroup;

        private readonly List<string> _choices = new List<string>();
        private readonly List <NpcChoiceButton> _choiceButtons = new List<NpcChoiceButton>();
        private NpcChoiceButton _currentChoiceButton;
        private int _selectedChoice;
        private int _availableChoices;
        private NpcAgent _npcAgent;
    

        public void AddChoices()
        {
            canvasGroup.alpha = 1f;
            _npcAgent = GetComponentInParent<NpcAgent>();

            CreateChoice("Talk");
            if(_npcAgent.HeldObj) CreateChoice("Ask about item");
            if(FindFirstObjectByType<PickUpScript>().HeldObj && !_npcAgent.HeldObj) CreateChoice("Give item");
            if(!FindFirstObjectByType<PickUpScript>().HeldObj && _npcAgent.HeldObj) CreateChoice("Take item");
       
            //Todo
            //if (_npcAgent.CachedState is RoamState) CreateChoice("Follow");
            //if (_npcAgent.CachedState is FollowState) CreateChoice("Explore");
        
            CreateChoice("Nevermind");

        }

        private void CreateChoice(string text)
        {
            _currentChoiceButton = Instantiate(choiceButtonPrefab, gameObject.transform).GetComponent<NpcChoiceButton>();
            _currentChoiceButton.text.SetText(text); 
            _currentChoiceButton.ID = _choiceButtons.Count; 
            _choiceButtons.Add(_currentChoiceButton);
            _choices.Add(text);

        }
        public void Navigate(InputAction.CallbackContext context)
        {
            if (_choiceButtons.Count == 0) return;
            if (!context.started) return;
            _choiceButtons[_selectedChoice].Deselected();
            if (context.ReadValue<float>().Equals(1))
            {
                _selectedChoice--;
            }
            else
            {
                _selectedChoice++;
            }
            _selectedChoice = Mathf.Clamp(_selectedChoice, 0, _choiceButtons.Count - 1);
            _choiceButtons[_selectedChoice].Selected();

        }
        public void SelectChoice(InputAction.CallbackContext context)
        {
            if (_choiceButtons.Count == 0) return;
            if (!context.started) return;
            FindFirstObjectByType<NpcInteraction>().DialogueOption(_choices[_selectedChoice]);

            foreach (NpcChoiceButton choiceButton in _choiceButtons)
            {
                Destroy(choiceButton.gameObject);
            }

            _choiceButtons.Clear();
            _choices.Clear();
            //ShowChoices(false);
            _selectedChoice = 0;
            canvasGroup.alpha = 0f;
        }
    }
}
