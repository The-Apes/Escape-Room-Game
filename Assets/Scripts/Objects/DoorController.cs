using UnityEngine;

namespace Objects
{
    public class DoorController : MonoBehaviour
    {
        [Header("Door Parts")]
        [SerializeField] private Transform doorLeft;   // Door 1
        [SerializeField] private Transform doorRight;  // Door 2

        [Header("Slide Settings")]
        [SerializeField] private float slideDistance = 2f;  
        [SerializeField] private float slideSpeed = 3f;     

        private Vector3 _leftClosedPos;
        private Vector3 _rightClosedPos;
        private Vector3 _leftOpenPos;
        private Vector3 _rightOpenPos;

        private bool _isOpen;
        private bool _isMoving;

        private void Start()
        {
            // Save starting closed positions
            _leftClosedPos = doorLeft.position;
            _rightClosedPos = doorRight.position;

            // Define open positions
            _leftOpenPos = _leftClosedPos + Vector3.left * slideDistance;
            _rightOpenPos = _rightClosedPos + Vector3.right * slideDistance;
        }

        public void ToggleDoor()
        {
            if (!_isMoving)
            {
                Debug.Log("Toggling door: " + (_isOpen ? "Closing" : "Opening"));
                StartCoroutine(SlideDoor());
            }
        }

        private System.Collections.IEnumerator SlideDoor()
        {
            _isMoving = true;

            Vector3 targetLeft = _isOpen ? _leftClosedPos : _leftOpenPos;
            Vector3 targetRight = _isOpen ? _rightClosedPos : _rightOpenPos;

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

            _isOpen = !_isOpen;
            _isMoving = false;

            Debug.Log("Door finished moving. State isOpen = " + _isOpen);
        }
    }
}
