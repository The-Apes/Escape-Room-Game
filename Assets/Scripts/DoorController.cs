using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private string openAnimTrigger = "Open";

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
        {
            animator.SetTrigger(openAnimTrigger);
        }
        else
        {
            Debug.LogWarning($"{name}: No Animator assigned for door.");
        }
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;

        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
    }
}
