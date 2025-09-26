using System.Collections;
using Managers;
using Npc.State_Machine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Npc
{
    public class NpcInteraction : MonoBehaviour
    {
        private NpcAgent _npcAgent;

        private void Awake()
        {
            _npcAgent = FindFirstObjectByType<NpcAgent>();
            if (_npcAgent == null)
            {
                Debug.LogError("NpcAgent component not found on the GameObject.");
            }
        }

        public void CallNpcInput(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (_npcAgent.Busy) return;
            StartCoroutine(CallNpcCoroutine());
        }

        private IEnumerator CallNpcCoroutine()
        {
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Call"),true);
            yield return new WaitForSeconds(1f);
        
            // Check if the NPC is already following the player
            if (_npcAgent.ActiveState is FollowState)
            {
                Debug.Log("NPC is already following the player.");
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Redundant Call"));
            }
            else
            {
                FollowState followState = (FollowState)(_npcAgent.ActiveState = new FollowState(_npcAgent, transform, 15f));
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Call"));

                while (!followState.Waiting)
                {
                    yield return null; // Wait until the NPC starts Waiting
                }
            
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Arrival"));

            }
        }
        public void DialogueOption(string action)
        {
            StartCoroutine(DialogueOptionCoroutine(action));
        }

        private IEnumerator DialogueOptionCoroutine(string action)
        {
            switch (action)
            {
                case "Talk":
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Talk"), true);
                    yield return new WaitForSeconds(0.5f);
                    PuzzleManager.instance.TalkHelp();
                    break;
                case "Ask about item":
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Inquire Item"), true);
                    yield return new WaitForSeconds(0.5f);
                    _npcAgent.DescribeObject();
                    _npcAgent.StopInteraction();
                    break;
                case "Give item":
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Give Item"), true);
                    yield return new WaitForSeconds(0.5f);
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Affirmative"));
                    _npcAgent.TakeObject();
                    _npcAgent.StopInteraction();
                    break;
                case "Take item":
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Take Item"), true);
                    yield return new WaitForSeconds(0.5f);
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Affirmative"));
                    _npcAgent.GiveObject();
                    _npcAgent.StopInteraction();
                    break;
                case "Nevermind":
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Cancel"), true);
                    yield return new WaitForSeconds(0.5f);
                    DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Cancel"));
                    _npcAgent.StopInteraction();
                    break;
            }
        }
 
    

    }
}
