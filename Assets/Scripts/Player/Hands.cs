using Objects;
using UnityEngine;

namespace Player
{
    public class Hands : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [Header("First Person Hands")]
        [SerializeField] private GameObject fpLeftHand;
        [SerializeField] private GameObject fpRightHand;
        [Header("Third Person Hands")]
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
    
        private bool _usingRightHand;
        private bool _usingLeftHand;
    
        private void Update()
        {
            FindAnimationState();
        
            if (_usingRightHand)
            {
                fpRightHand.SetActive(true);
                rightHand.SetActive(false);
            } else
            {
                fpRightHand.SetActive(false);
                rightHand.SetActive(true);
            }
            if (_usingLeftHand)
            {
                fpLeftHand.SetActive(true);
                leftHand.SetActive(false);
            }
            else
            {
                fpLeftHand.SetActive(false);
                leftHand.SetActive(true);
            }
        }

        public void Grab()
        {
            animator.SetTrigger("Grab");
        }
        public void Drop()
        {
            animator.SetTrigger("Drop");
        }

        public void Place()
        {
            animator.SetTrigger("Place");
        }

        public void ChangeStyle(ItemHoldDetails.HoldStyle style)
        {
            //Reset
            animator.SetBool("Handle", false);
            animator.SetBool("Lantern", false);
            animator.SetBool("Cup", false);
            animator.SetBool("Palm", false);
            animator.SetBool("Key", false);
            animator.SetBool("Paper", false);
            
            switch (style)
            {
                case ItemHoldDetails.HoldStyle.Handle:
                    animator.SetBool("Handle", true);
                    break;
                case ItemHoldDetails.HoldStyle.Lantern:
                    animator.SetBool("Lantern", true);
                    break;
                case ItemHoldDetails.HoldStyle.Cup:
                    animator.SetBool("Cup", true);
                    break;
                case ItemHoldDetails.HoldStyle.Palm:
                    animator.SetBool("Palm", true);
                    break;
                case ItemHoldDetails.HoldStyle.Key:
                    animator.SetBool("Key", true);
                    break;
                case ItemHoldDetails.HoldStyle.Paper:
                    animator.SetBool("Paper", true);
                    break;
                default:
                    animator.SetBool("Handle", true);
                    break;
            }
        }

        private void FindAnimationState()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("None"))
            {
                _usingLeftHand = false;
                _usingRightHand = false;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wakeup"))
            {
                _usingLeftHand = true;
                _usingRightHand = true;
            } if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grab"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }if (animator.GetCurrentAnimatorStateInfo(0).IsName("Place"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Handle Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cup Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }if (animator.GetCurrentAnimatorStateInfo(0).IsName("Palm Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }if (animator.GetCurrentAnimatorStateInfo(0).IsName("Key Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Paper Hold"))
            {
                _usingLeftHand = false;
                _usingRightHand = true;
            }
        
        }
    }
}
