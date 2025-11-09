/*
Title: PickUpTutorial
Author: JonDevTutorial
Date: 10/08/2025
Availability: https://github.com/JonDevTutorial/PickUpTutorial or https://www.youtube.com/watch?v=pPcYr3tL3Sc
*/

using System;
using Npc;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
            DropObject();
        }
        public void PickUpObject(GameObject pickUpObj)
        {
            if (!pickUpObj.GetComponent<Rigidbody>()) return; //make sure the object has a RigidBody
            //Reset offset
            offsetTransform.transform.localPosition = Vector3.zero;
            offsetTransform.localScale = Vector3.one;
            
            HeldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            _cachedItemScale = HeldObj.transform.localScale;
            HeldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            HeldObjRb.isKinematic = true;
            HeldObjRb.transform.parent = offsetTransform.transform; //parent object to hold position
            HeldObjRb.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            HeldObj.layer = _layerNumber; //change the object layer to the holdLayer
            SetMultipleLayers.SetLayerRecursively(HeldObj, _layerNumber);
        
            //make sure object doesn't collide with player, it can cause weird bugs
            Physics.IgnoreCollision(HeldObj.GetComponentInChildren<Collider>(), player.GetComponent<Collider>(), true);
            
            //change hand animation based on object hold style
            var holdDetails = HeldObj.GetComponent<ItemHoldDetails>();
            if (holdDetails)
            {
                _hands.ChangeStyle(holdDetails.holdStyle);
                
                //adjust hold position/rotation based on holdDetails script
                offsetTransform.transform.localPosition = holdDetails.holdPositionOffset;
                offsetTransform.localScale = holdDetails.holdScaleOffset;
                HeldObjRb.transform.localRotation = Quaternion.Euler(holdDetails.holdRotationOffset);
                print(HeldObj);
            }
            _hands.Grab();
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
        public void Inspect(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (!HeldObj) return;
        
            if (!Inspecting)
            {
                _fpController.CanMove = false;
                _fpController.CanLook = false;
                _heldObjParent = holdTransform.parent.gameObject;
                //store rotation
                _initialObjPos = holdTransform.localPosition;
                _initialObjRot = HeldObj.transform.localRotation;
                holdTransform.parent = inspectObj.transform;
                holdTransform.localPosition = Vector3.zero;
                //holdTransform.localPosition = inspectPos;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true; 
                Inspecting = true;
            }
            else
            {
                _fpController.CanMove = true;
                _fpController.CanLook = true;
                holdTransform.parent = _heldObjParent.transform;
                holdTransform.localPosition = _initialObjPos;
                HeldObj.transform.localRotation = _initialObjRot;
                //apply rotation
                //holdTransform.localPosition = holdPos;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
                Inspecting = false;
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