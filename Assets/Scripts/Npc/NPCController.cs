using System;
using System.Collections;
using ItemDescriptions;
using Managers;
using Npc.State_Machine;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Npc
{
    public class NpcAgent : MonoBehaviour
    {
        public TextMeshPro text;
        public Transform holdTransform;
    
        [NonSerialized]public IState ActiveState;
        [NonSerialized] private IState _cachedState;
        [NonSerialized] public NavMeshAgent Agent;
        [NonSerialized] public GameObject HeldObj;
        [NonSerialized] public bool Busy;
    
        [SerializeField] private GameObject choicesPanel;
    
        private Camera _cam;
        private bool _walk;
        private bool _talking;
        private Rigidbody _heldObjRb; //rigidbody of object we pick up
        private ObjectInteractor _objectInteractor;
    
    

        // private enum States
        // {
        //     Roam,
        //     Follow,
        //     GoTo,
        //     Script
        // }
        //
        // States _currentState = States.Roam;
        private void SetTalking(bool value)
        {
            _talking = value;
            if(ScriptManager.instance){ScriptManager.instance.NpcTalking = value;}
        }

        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            _cam = Camera.main;
            ActiveState = new RoamState(this, transform);
            choicesPanel.SetActive(false);
        }
        private void Start()
        {
            text.SetText("");
            _objectInteractor = FindFirstObjectByType<ObjectInteractor>();

        }
        void Update()
        {
            var newState = ActiveState?.StateUpdate();
            if (newState != null)
            {
                ChangeState(newState);
            }
            if(HeldObj) MoveObject();
        }
        public void Speak(string line, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(SpeakCoroutine(line, duration));
        }
        private IEnumerator SpeakCoroutine(string line, float duration)
        {
            //TODO
            // For each line, display one character at a time same way we did bog wood
            //SetTalking(true); don't wanna break nothing
            text.SetText(line);
            yield return new WaitForSeconds(duration);
            text.SetText("");
            SetTalking(false);
        }
        public void ScriptStart()
        {
            _cachedState = ActiveState;
            ActiveState = null;
            Busy = true; //make a separate script bool?
        }
        public void ScriptEnd()
        {
            ActiveState = _cachedState;
            Busy = false; //make a separate script bool?
        }

        public void Interact()
        {
            if (Busy) return;
            Busy = true;
            _cachedState = ActiveState;
            ActiveState = new FollowState(this, Camera.main.transform);
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Start Dialogue"), true);
            print("Active");
            Vector3 direction = _cam.transform.position - transform.position;
            direction.y = 0; // Ignore vertical difference
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.forward = direction.normalized;
            }
            choicesPanel.SetActive(true);
            choicesPanel.GetComponentInChildren<NpcChoiceBox>().AddChoices();
        
        }
        public void StopInteraction()
        {
            if (!Busy) return;
            Busy = false;
            ChangeState(_cachedState);
            choicesPanel.SetActive(false);
            _cachedState = null;
            print("Inactive");
        }

        public void Action(string action,string parameter = null)
        {
            switch (action)
            {
                case "go to player":
                    Agent.destination = _cam.transform.position;
                    break;
                case "go to object":
                    Agent.destination = ScriptManager.instance.CurrentLine.customObject.transform.position;
                    break;
                case "go to location":
                    Agent.destination = ScriptManager.instance.CurrentLine.location;
                    break;
                case "give item":
                    if (HeldObj) GiveObject();
                    break;

            }
        }

        // public void input()
        // {
        //     switch (_currentState)
        //     {
        //         case States.Roam:
        //             break;
        //     }
        // }

        // Update is called once per frame


        public void TakeObject()
        {
            GameObject objectToPickUp = _objectInteractor.HeldObj;
            _objectInteractor.HeldObj = null;
            HeldObj = objectToPickUp; //assign heldObj to the object that was hit by the raycast (no longer == null)
            HeldObj.transform.position = holdTransform.transform.position; //set position to hold position
            _heldObjRb = _objectInteractor.HeldObjRb.GetComponent<Rigidbody>(); //assign Rigidbody
            _objectInteractor.HeldObjRb = null;
            _heldObjRb.isKinematic = true;
            _heldObjRb.transform.parent = holdTransform.transform; //parent object to hold position
            _heldObjRb.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            HeldObj.tag = "Untagged";
            HeldObj.layer = 0; //object assigned back to default layer
        
            //make sure object doesn't collide with player, it can cause weird bugs
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        
        
        }

        public void GiveObject()
        {
            if (_objectInteractor.HeldObj)
            {
                DropObject();
            }
            else
            {
                _objectInteractor.PickUpObject(HeldObj);
                HeldObj.tag = "canPickUp"; //reset tag to canPickUp
                HeldObj = null;
                _heldObjRb = null; 
            }
        }
        public void DropObject()
        {
            if (HeldObj == null) return;
            //re-enable collision with player
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            HeldObj.layer = 0; //object assigned back to default layer
            _heldObjRb.isKinematic = false;
            _heldObjRb.transform.parent = null; //unparent object
            _heldObjRb.transform.position = new Vector3(_heldObjRb.transform.position.x, Mathf.Max(0.25f, _heldObjRb.transform.position.y), _heldObjRb.transform.position.z);
            HeldObj = null; //undefine game object
            HeldObj.tag = "canPickUp"; //reset tag to canPickUp
        }
        void MoveObject()
        {
            //keep object position the same as the holdPosition position
            HeldObj.transform.position = holdTransform.transform.position;
        }
        public void DescribeObject()
        {
            if (!HeldObj) return;
            StopInteraction();
            IItemDescription description = HeldObj.GetComponent<IItemDescription>();
            if (description != null)
            {
                description.Describe();
            }
            else
            {
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Unknown Item"));
            }
        }

        private void ChangeState(IState newState)
        {
            ActiveState.ExitState();
            ActiveState = newState;
            ActiveState.EnterState();
        }
    }
}
