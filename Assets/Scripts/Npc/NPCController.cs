using System;
using System.Collections;
using ItemDescriptions;
using ItemDescriptions.Custom_Descriptions;
using Managers;
using Npc.State_Machine;
using Objects;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Util;

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
        [SerializeField] private AudioClip[] talkSound;
    
        private Camera _cam;
        private bool _walk;
        private bool _talking; //dont touch, breaks game
        private bool _textVisible;
        private Rigidbody _heldObjRb; //rigidbody of object we pick up
        private ObjectInteractor _objectInteractor;
        private Animator _animator;
        private FpController _fpController;
        private AudioSource _audioSource;
        private ItemNpcHoldDetails _heldObjDetails;
        private int _lastTalkIndex = -1;
    
    

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
            //dont touch, breaks game
            _talking = value;
            if(ScriptManager.instance) {ScriptManager.instance.NpcTalking = value;}
        }

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _fpController = FindFirstObjectByType<FpController>();
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
            HandleAnimation();
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
            //SetTalking(true);
            text.SetText(line);
            //play random sound from array
            
            // I got freaky here cuz of OCD and I kinda regret it
            if (talkSound == null || talkSound.Length == 0 || _audioSource == null) {}
            else
            {
                int newIndex;
                if (talkSound.Length == 1)
                {
                    newIndex = 0;
                }
                else
                {
                    do
                    {
                        newIndex = UnityEngine.Random.Range(0, talkSound.Length);
                    } while (newIndex == _lastTalkIndex);
                }

                _lastTalkIndex = newIndex;
                _audioSource.PlayOneShot(talkSound[newIndex]);                  _textVisible = true;
            }


            yield return new WaitForSeconds(duration);
            text.SetText("");
            _textVisible = false;
            SetTalking(false);
        }
        public void ScriptStart()
        {
            if(ActiveState != null) 
            {
                _cachedState = ActiveState;
                ActiveState = null;
            }
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
            if (Camera.main) ActiveState = new FollowState(this, Camera.main.transform);
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Start Dialogue"), true);
            PlayerFlagsManager.instance.InteractedWithNpc = true;
            TutorialManager.instance.NpcTalkTutorial();
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
                // case "custom":
                //     switch (parameter)
                //     {
                //         case "torch":
                //             FindFirstObjectByType<Torch>().ActivateTorch();
                //             break;
                //         default:
                //             break;// Add custom conditions here
                //     }
                //     break;

            }
        }

        public void UnlockRoom()
        {
            
        }

        public void TakeObject()
        {
            if (_objectInteractor.HeldObj) DropObject();
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
            SetMultipleLayers.SetLayerRecursively(HeldObj, 0);

            //reset hold transform
            holdTransform.localPosition = Vector3.zero;
            holdTransform.localRotation = Quaternion.identity;
            holdTransform.localScale = Vector3.one;
            
            _heldObjDetails = HeldObj.GetComponent<ItemNpcHoldDetails>();
            if (_heldObjDetails)
            {
                holdTransform.localPosition = _heldObjDetails.holdPositionOffset;
                holdTransform.localRotation = Quaternion.Euler(_heldObjDetails.holdRotationOffset);
                //holdTransform.localScale = _heldObjDetails.holdScaleOffset;
            }
            _animator.SetTrigger("Hold");
        }

        public void GiveObject()
        {
            if (_objectInteractor.HeldObj)
            {
                DropObject();
            }
            else
            {
                var cache = HeldObj;
                DropObject();
                _objectInteractor.PickUpObject(cache);
                HeldObj.tag = "canPickUp"; //reset tag to canPickUp
                HeldObj = null;
                _heldObjRb = null;
                _heldObjDetails = null;
            }
        }

        private void DropObject()
        {
            if (HeldObj == null) return;
            //re-enable collision with player
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            HeldObj.layer = 0; //object assigned back to default layer
            _heldObjRb.isKinematic = false;
            _heldObjRb.transform.parent = null; //unparent object
            _heldObjRb.transform.position = new Vector3(_heldObjRb.transform.position.x, Mathf.Max(0.25f, _heldObjRb.transform.position.y), _heldObjRb.transform.position.z);
            HeldObj.tag = "canPickUp"; //reset tag to canPickUp
            HeldObj = null; //undefine game object
            _heldObjRb = null;
            _heldObjDetails = null;
        }

        public void DestroyHeldObject()
        {
            var cache = HeldObj;
            DropObject();
            Destroy(cache);
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
            var description = HeldObj.GetComponent<SimpleDescription>();
            var scriptDescription = HeldObj.GetComponent<ScriptDescription>();
            var customDescription = HeldObj.GetComponent<ICustomDescription>();
            if (description)
            {
                description.Describe();
            }
            else if (scriptDescription)
            {
                scriptDescription.Describe();
            }
            else if (customDescription != null)
            {
                customDescription.Describe();
            }
            else
            {
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Unknown Item"));
            }
        }

        private void ChangeState(IState newState)
        {
            if(ActiveState == null) return;
            ActiveState.ExitState();
            ActiveState = newState;
            ActiveState.EnterState();
        }
        private void HandleAnimation()
        {
            _animator.SetBool("Moving", Agent.velocity.magnitude > 0.1f);
            _animator.SetBool("Talking", _textVisible);
            _animator.SetBool("Holding", _heldObjRb);

            if (_heldObjDetails)
            {
                _animator.SetFloat("Size", _heldObjDetails.itemSize);
                _animator.SetBool("Small", _heldObjDetails.small);
            }
            
            //face the player when talking
            if (_textVisible && !(Agent.velocity.magnitude > 0.1f)) FacePlayer();
        }

        private void FacePlayer()
        {
            float defaultXRotation = Agent.transform.rotation.eulerAngles.x;
            float defaultZRotation = Agent.transform.rotation.eulerAngles.z;

            Agent.transform.LookAt(_fpController.cameraTransform);
            Agent.transform.rotation = Quaternion.Euler(defaultXRotation, Agent.transform.rotation.eulerAngles.y, defaultZRotation);
        }
    }
    }

