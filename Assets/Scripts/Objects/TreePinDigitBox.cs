using System;
using UnityEngine;

namespace Objects
{
    public class ThreePinDigitBox : MonoBehaviour, IClickable
    {
        //This code is used both on the box and it's dials
        [SerializeField] private int[] correctCode = {3,2,1};
        [SerializeField] private bool isDial;
        [SerializeField] private int dialNumber;
        
        private ThreePinDigitBox _parentBox;
        private int[] _currentCode = {0, 0, 0};
        
        //for dials
        private int _currentNumber;
        private TextMesh _textMesh;

        private void Awake()
        {
            if (!isDial) return;
            _parentBox = GetComponentInParent<ThreePinDigitBox>();
            _textMesh = GetComponentInChildren<TextMesh>();
            _currentNumber = 0;
        }

        public void Check()
        {
            
        }

        public void OnClick()
        {
            if (!isDial) return;
            
            // Increment the dial number if it's less than 9, otherwise reset to 0
            if(_currentNumber < 9) _currentNumber++;
            else _currentNumber = 0;
            
            // Change the text
            _textMesh.text = _currentNumber.ToString();
            
            // Update the parent box's current code array
            _parentBox._currentCode[dialNumber] = _currentNumber;
            
            // run a check on the parent box
        }
        
    }
}
