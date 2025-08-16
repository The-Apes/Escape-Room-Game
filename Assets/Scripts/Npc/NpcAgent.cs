using System;
using System.Collections;
using Npc.State_Machine;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NpcAgent : MonoBehaviour
{
    public TextMeshPro text;
    public Transform holdTransform;
    
    [NonSerialized]public IState ActiveState;
    [NonSerialized] public IState CachedState;
    [NonSerialized] public NavMeshAgent Agent;
    [NonSerialized] public GameObject HeldObj;
    [NonSerialized] public bool Busy;
    
    [SerializeField] private GameObject choicesPanel;
    
    private Camera _cam;
    private bool _walk;
    private bool _talking;
    private Rigidbody _heldObjRb; //rigidbody of object we pick up
    private PickUpScript _pickUpScript;
    
    

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
        _pickUpScript = FindFirstObjectByType<PickUpScript>();

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
        CachedState = ActiveState;
        ActiveState = null;
        Busy = true; //make a seperate script bool?
    }
    public void ScriptEnd()
    {
        ActiveState = CachedState;
        Busy = false; //make a seperate script bool?
    }

    public void Interact()
    {
        if (Busy) return;
        Busy = true;
        CachedState = ActiveState;
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
        ChangeState(CachedState);
        choicesPanel.SetActive(false);
        CachedState = null;
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
        GameObject objectToPickUp = _pickUpScript.heldObj;
        _pickUpScript.heldObj = null;
            HeldObj = objectToPickUp; //assign heldObj to the object that was hit by the raycast (no longer == null)
            HeldObj.transform.position = holdTransform.transform.position; //set position to hold position
            _heldObjRb = _pickUpScript.heldObjRb.GetComponent<Rigidbody>(); //assign Rigidbody
            _pickUpScript.heldObjRb = null;
            _heldObjRb.isKinematic = true;
            _heldObjRb.transform.parent = holdTransform.transform; //parent object to holdposition
            _heldObjRb.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            HeldObj.tag = "Untagged";
            HeldObj.layer = 0; //object assigned back to default layer
        
            //make sure object doesn't collide with player, it can cause weird bugs
            // Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        
        
    }

    public void GiveObject()
    {
        if (_pickUpScript.heldObj)
        {
            DropObject();
        }
        else
        {
            _pickUpScript.PickUpObject(HeldObj);
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
        ActiveState.exitState();
        ActiveState = newState;
        ActiveState.enterState();
    }
}
