/*
Title: PickUpTutorial
Author: JonDevTutorial
Date: 10/08/2025
Availability: https://github.com/JonDevTutorial/PickUpTutorial or https://www.youtube.com/watch?v=pPcYr3tL3Sc
*/
// a lopt o0f this code is commented out because im not sure if we will use a lot of it, don't delete out the commented
//Code for this exact reason

using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdTransform;
    
    [SerializeField] private Vector3 holdPos;
    [SerializeField] private Vector3 inspectPos;
    
   // public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 3f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private bool inspecting;
    private int LayerNumber; //layer index
    
    private FPController _FPController;
    private float _cachedLookSensitivity;
    private float _cachedMoveSpeed;

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        
        _FPController = FindFirstObjectByType<FPController>();
        _cachedLookSensitivity = _FPController.lookSensitivity;
        _cachedMoveSpeed = _FPController.moveSpeed;

        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update()
    {
        if (!heldObj) return; //if player is holding object
        MoveObject(); //keep object position at holdPos
        if(inspecting) RotateObject();
        if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
        {
            StopClipping();
            //ThrowObject();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return; 
        
        //perform raycast to check if player is looking at object within pickuprange
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            //make sure pickup tag is attached
            
            //if you add more tags, turn this into a switch statement habibi
            if (hit.transform.gameObject.tag == "canPickUp")
            {
                if (heldObj != null) return; 
                //pass in object hit into the PickUpObject function
                PickUpObject(hit.transform.gameObject);
            }
            else if(hit.transform.gameObject.tag == "Interactable")
            {
                print("Interactable object hit");
                hit.transform.gameObject.GetComponent<IInteractable>()?.OnInteract(heldObj); //call Interactable script on object
            }
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (heldObj == null) return;
        if (!context.started) return;
        if (!canDrop) return;
        StopClipping(); //prevents object from clipping through walls
        DropObject();

    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (!pickUpObj.GetComponent<Rigidbody>()) return; //make sure the object has a RigidBody
        heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
        heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
        heldObjRb.isKinematic = true;
        heldObjRb.transform.parent = holdTransform.transform; //parent object to holdposition
        heldObjRb.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        heldObj.layer = LayerNumber; //change the object layer to the holdLayer
        
        //make sure object doesnt collide with player, it can cause weird bugs
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    public void Inspect(InputAction.CallbackContext context)
    {
        print(context);
        if (!context.started) return;
        if (!heldObj) return;
        
        print("Inspect");
        
        if (!inspecting)
        {
            _FPController.moveSpeed = 0f;
            _FPController.lookSensitivity = 0f;
            holdTransform.localPosition = inspectPos;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true; 
            inspecting = true;
        }
        else
        {
            _FPController.moveSpeed = _cachedMoveSpeed;
           _FPController.lookSensitivity = _cachedLookSensitivity;
            holdTransform.localPosition = holdPos;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
            inspecting = false;
        }
    }
    public void PlaceObject(Transform placePos)
    {
        print("place");
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        //heldObjRb.isKinematic = false;
        heldObj.transform.parent = placePos;
        heldObj.transform.position = placePos.position; //new Vector3(0f, 0f, 0f);
        heldObj.transform.rotation = placePos.rotation;
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdTransform.transform.position;
    }
    void RotateObject()
    {
        if(!inspecting) return; 
        if (Input.GetKey(KeyCode.Mouse1))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating
            
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 

            float xAxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float yAxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, xAxisRotation*3);
            heldObj.transform.Rotate(Vector3.right, yAxisRotation*3);
        }
        else
        {
            canDrop = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    // void ThrowObject()
    // {
    //     //same as drop function, but add force to object before undefining it
    //     Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
    //     heldObj.layer = 0;
    //     heldObjRb.isKinematic = false;
    //     heldObj.transform.parent = null;
    //     heldObjRb.AddForce(transform.forward * throwForce);
    //     heldObj = null;
    // }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}