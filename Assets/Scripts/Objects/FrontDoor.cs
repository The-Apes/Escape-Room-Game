using UnityEngine;

namespace Objects
{
    public class FrontDoorInteractable : MonoBehaviour, IInteractable
    {
        public void OnInteract(GameObject heldObject)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            
        }
    }
}
