using TMPro;
using UnityEngine;

namespace Objects.Puzzles
{
    public class DialBox : MonoBehaviour, IClickable
    {
        //This code is used both on the box and it's dials
        [SerializeField] private int[] empty;
        [SerializeField] private bool isDial;
        [SerializeField] private int dialIndex;
        
        private Dial _parentBox;
        //private int[] _currentCode = {0, 0, 0};
        
        //for dials
        private int _currentNumber;
        private TextMeshPro _textMesh;

        private void Awake()
        {
            if (!isDial) return;
            _parentBox = GetComponentInParent<Dial>();
            _textMesh = GetComponentInChildren<TextMeshPro>();
            
            _currentNumber = 0;
            //_parentBox._currentCode[dialIndex] = _currentNumber;
        }

        /*public void Check()
        {
            bool isCorrect = true;
            for (int i = 0; i < correctCode.Length; i++)
            {
                if (_currentCode[i] != correctCode[i])
                {
                    isCorrect = false;
                    print(isCorrect);
                    break;
                }
            }
            print(isCorrect ? "Box Unlocked!" : "Box Locked!");
        }*/

        public void OnClick(GameObject heldObject)
        {
            if (!isDial) return;
            
            // Increment the dial number if it's less than 9, otherwise reset to 0
            if (_currentNumber < 9)
            {
                _currentNumber++;
            }
            else
            {
                _currentNumber = 0;
            }
            print(_currentNumber);
            // Change the text
            _textMesh.text = _currentNumber.ToString();
            // Update the parent box's current code array
           // _parentBox._currentCode[dialIndex] = _currentNumber;
            // run a check on the parent box
            _parentBox.Check();
        }
    }
}
