using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Parts")]
    [SerializeField] private Transform doorLeft;   // Door 1
    [SerializeField] private Transform doorRight;  // Door 2

    [Header("Slide Settings")]
    [SerializeField] private float slideDistance = 2f;  
    [SerializeField] private float slideSpeed = 3f;     

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        // Save starting closed positions
        leftClosedPos = doorLeft.position;
        rightClosedPos = doorRight.position;

        // Define open positions
        leftOpenPos = leftClosedPos + Vector3.left * slideDistance;
        rightOpenPos = rightClosedPos + Vector3.right * slideDistance;
    }

    public void ToggleDoor()
    {
        if (!isMoving)
        {
            Debug.Log("Toggling door: " + (isOpen ? "Closing" : "Opening"));
            StartCoroutine(SlideDoor());
        }
    }

    private System.Collections.IEnumerator SlideDoor()
    {
        isMoving = true;

        Vector3 targetLeft = isOpen ? leftClosedPos : leftOpenPos;
        Vector3 targetRight = isOpen ? rightClosedPos : rightOpenPos;

        while (Vector3.Distance(doorLeft.position, targetLeft) > 0.01f)
        {
            doorLeft.position = Vector3.MoveTowards(
                doorLeft.position, targetLeft, slideSpeed * Time.deltaTime
            );
            doorRight.position = Vector3.MoveTowards(
                doorRight.position, targetRight, slideSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Snap to target
        doorLeft.position = targetLeft;
        doorRight.position = targetRight;

        isOpen = !isOpen;
        isMoving = false;

        Debug.Log("Door finished moving. State isOpen = " + isOpen);
    }
}
