/*
Title: PickUpTutorial
Author: JonDevTutorial
Date: 10/08/2025
Availability: https://github.com/JonDevTutorial/PickUpTutorial or https://www.youtube.com/watch?v=pPcYr3tL3Sc
*/

using System;
using System.Collections;
using ItemDescriptions;
using ItemDescriptions.Custom_Descriptions;
using Managers;
using Npc;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Player
{
    public class ObjectInteractor : MonoBehaviour
    {
        public GameObject player;
        public Transform holdTransform;
        public Transform offsetTransform;
        
        [SerializeField] private GameObject inspectObj;
    
        [SerializeField] private Vector3 holdPos;
        [SerializeField] private Vector3 inspectPos;
    
        [NonSerialized] public GameObject HeldObj; //object which we pick up
        [NonSerialized]public Rigidbody HeldObjRb; //rigidbody of object we pick up
        
        private GameObject _heldObjParent; //to store original parent
        private Vector3 _initialObjPos; //to store When inspected
        private Quaternion _initialObjRot; //to store When inspected


        public float pickUpRange = 5f; //how far the player can pick up the object from
        //private float _rotationSensitivity = 3f; //how fast/slow the object is rotated in relation to mouse movement
        private bool _canDrop = true; //this is needed so we don't throw/drop object when rotating the object
        [NonSerialized] public bool Inspecting;
        private int _layerNumber; //layer index
        private Camera _cam;
        private FpController _fpController;
        private Hands _hands;
        private float _xAxisRotation;
        private float _yAxisRotation;
        private Vector3 _cachedItemScale;
        void Start()
        {
            _layerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
            _cam = Camera.main;
            _fpController = FindFirstObjectByType<FpController>();
            _hands = FindFirstObjectByType<Hands>();

        }
        void Update()
        {
            if (!HeldObj) return; //if player is holding object
            MoveObject(); //keep object position at holdPos
            if(Inspecting) RotateObject();
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.started) return; 
        
            if(!Inspecting){
                //perform raycast to check if player is looking at object within pickup range
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickUpRange))
                {
                    //make sure tag is attached
            
                    switch (hit.transform.gameObject.tag)
                    {
                        case "canPickUp" when HeldObj != null:
                            return;
                        //make sure pickup tag is attached
                        //pass in object hit into the PickUpObject function
                        case "canPickUp":
                            PickUpObject(hit.transform.gameObject);
                            break;
                        case "Interactable":
                            print("Interactable object hit");
                            hit.transform.gameObject.GetComponent<IInteractable>()?.OnInteract(HeldObj); //call Interactable script on object
                            break;
                        case "NPC":
                            FindFirstObjectByType<NpcAgent>().Interact();
                            break;
                    }
                }
            
            }
            else
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Clicked on " + hit.collider.name);
                    //Debug.Log(hit.transform.tag);
                    hit.collider.gameObject.GetComponent<IClickable>()?.OnClick(HeldObj); //call Interactable script on object

                    if (hit.transform.gameObject.tag.Equals("Clickable"))
                    {
                        //print("clickable object hit");
                    }
                    // Example: do something
                    // hit.collider.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }

        public void Drop(InputAction.CallbackContext context)
        {
            if (HeldObj == null) return;
            if (!context.started) return;
            if (!_canDrop) return;
            if (Inspecting) return;
            StopClipping(); //prevents object from clipping through walls
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickUpRange) && hit.transform.CompareTag("NPC")) //if looking at NPC
            {
                GiveToNpc();
            }
            else
            {
                DropObject();
                PlayerFlagsManager.instance.DroppedAnItem = true;
            }
        }
        public void PickUpObject(GameObject pickUpObj)
        {
            if (!pickUpObj.TryGetComponent(out Rigidbody rb)) return;

            HeldObj = pickUpObj;
            HeldObjRb = rb;
            _cachedItemScale = HeldObj.transform.localScale;

            // Setup rigidbody
            HeldObjRb.isKinematic = true;

            // Set world position to Offset's world position first
            HeldObj.transform.position = offsetTransform.position;

            // Parent to OFFSET (child of HoldPose)
            HeldObj.transform.parent = offsetTransform;

            // Reset rotation relative to parent
            HeldObj.transform.localRotation = Quaternion.identity;

            // Apply hold offsets (position + rotation) relative to OFFSET
            var holdDetails = HeldObj.GetComponent<ItemHoldDetails>();
            if (holdDetails)
            {
                offsetTransform.localPosition = holdDetails.holdPositionOffset;
                offsetTransform.localRotation = Quaternion.Euler(holdDetails.holdRotationOffset);
                offsetTransform.localScale = holdDetails.holdScaleOffset;

                _hands.ChangeStyle(holdDetails.holdStyle);
            }

            // Layer and collision
            HeldObj.layer = _layerNumber;
            SetMultipleLayers.SetLayerRecursively(HeldObj, _layerNumber);

            _hands.Grab();

            var heldCollider = HeldObj.GetComponentInChildren<Collider>();
            var playerCollider = player.GetComponent<Collider>();
            if (heldCollider && playerCollider)
                Physics.IgnoreCollision(heldCollider, playerCollider, true);

            PlayerFlagsManager.instance.PickedUpItem = true;

            if (HeldObj.TryGetComponent(out Readable readable) && !PlayerFlagsManager.instance.ReadAnItem)
                TutorialManager.instance.ReadTutorial();

            if (!PlayerFlagsManager.instance.DroppedAnItem)
                TutorialManager.instance.DropTutorial();

            if (!PlayerFlagsManager.instance.InspectedAnItem && PlayerFlagsManager.instance.DroppedAnItem)
                TutorialManager.instance.InspectTutorial();
        }

        private void DropObject()
        {
            //re-enable collision with player
            Physics.IgnoreCollision(HeldObj.GetComponentInChildren<Collider>(), player.GetComponent<Collider>(), false);
            SetMultipleLayers.SetLayerRecursively(HeldObj, 0);
            HeldObjRb.isKinematic = false;
            HeldObj.transform.parent = null; //unparent object
            HeldObj.transform.position = new Vector3(HeldObj.transform.position.x, Mathf.Max(0.25f, HeldObj.transform.position.y), HeldObj.transform.position.z);
            HeldObj.transform.localScale = _cachedItemScale;
            HeldObj = null; //undefine game object
            _hands.Drop();
        }

        private void GiveToNpc()
        {
            StartCoroutine(GiveToNpcCoroutine());
        }

        private IEnumerator GiveToNpcCoroutine()
        {
            NpcAgent npc = FindFirstObjectByType<NpcAgent>();
            npc.TakeObject();
            _hands.Place();
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Give Item"), true);
            yield return new WaitForSeconds(0.75f);
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Affirmative"));
            yield return new WaitForSeconds(0.75f);
            DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Inquire Item"), true);
            yield return new WaitForSeconds(0.75f);
            npc.DescribeObject();
            npc.StopInteraction();
        }
        public void Inspect(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (!HeldObj) return;
        
            if (!Inspecting)
            {
                _fpController.CanMove = false;
                _fpController.CanLook = false;
                _heldObjParent = offsetTransform.parent.gameObject;
                //store rotation
                _initialObjPos = offsetTransform.localPosition;
                _initialObjRot = HeldObj.transform.localRotation;
                offsetTransform.parent = inspectObj.transform;
                offsetTransform.localPosition = Vector3.zero;
                //holdTransform.localPosition = inspectPos;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true; 
                Inspecting = true;
                _hands.HidingHands = true;
                
                PlayerFlagsManager.instance.InspectedAnItem = true;

                
                //check for description script
                var description = HeldObj.GetComponent<InspectSimpleDescription>();
                var scriptDescription = HeldObj.GetComponent<InspectScriptDescription>();
                var customDescription = HeldObj.GetComponent<IInspectCustomDescription>();
                if (description)
                {
                    if (ScriptManager.instance.CurrentScript) return;
                    description.Describe();
                }

                if (scriptDescription)
                {
                    if (ScriptManager.instance.CurrentScript) return;
                    scriptDescription.Describe();
                }
                customDescription?.Describe();
            }
            else
            {
                _fpController.CanMove = true;
                _fpController.CanLook = true;
                offsetTransform.parent = _heldObjParent.transform;
                offsetTransform.localPosition = _initialObjPos;
                HeldObj.transform.localRotation = _initialObjRot;
                //apply rotation
                //holdTransform.localPosition = holdPos;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
                Inspecting = false;
                _hands.HidingHands = false;
                _canDrop = true;
                FindFirstObjectByType<ReadingPanel>().Close(); //close reading panel if open
            }
        }
        public void PlaceObject(Transform placePos)
        {
            print("place");
            //re-enable collision with player
            Physics.IgnoreCollision(HeldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            SetMultipleLayers.SetLayerRecursively(HeldObj, 0);  //object assigned back to default layer
            //heldObjRb.isKinematic = false;
            HeldObj.transform.parent = placePos;
            HeldObj.transform.position = placePos.position; 
            HeldObj.transform.rotation = placePos.rotation;
            HeldObj = null; //undefine game object
            _hands.Place();
        }
        void MoveObject()
        {
            //keep object position the same as the holdPosition position
            HeldObj.transform.position = offsetTransform.transform.position;
        }
        public void RotateInput(InputAction.CallbackContext context)
        {
            _xAxisRotation = context.ReadValue<Vector2>().x;
            _yAxisRotation = context.ReadValue<Vector2>().y;
        }
        void RotateObject()
        {
            if(!Inspecting) return; 
            if (Input.GetKey(KeyCode.Mouse1))//hold R key to rotate, change this to whatever key you want
            {
                _canDrop = false; //make sure throwing can't occur during rotating
            
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            
                //rotate the object depending on mouse X-Y Axis
                HeldObj.transform.Rotate(Vector3.down, _xAxisRotation*3);
                HeldObj.transform.Rotate(Vector3.right, _yAxisRotation*3);
            }
            else
            {
                _canDrop = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }

        void StopClipping() //function only called when dropping
        {
            var clipRange = Vector3.Distance(HeldObj.transform.position, transform.position); //distance from holdPos to the camera
            //have to use RaycastAll as object blocks raycast in center screen
            //RaycastAll returns array of all colliders hit within the clip range
            
            // ReSharper disable once Unity.PreferNonAllocApi
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
            //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
            if (hits.Length > 1)
            {
                //change object position to camera position 
                HeldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
                //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
            }
        }
    }
}