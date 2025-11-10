using System;
using Managers;
using Npc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Torch : MonoBehaviour
    {
        public bool torchEnabled;
        
        [SerializeField] private AudioClip activateSound;
        [SerializeField] private AudioClip[] torchSounds;
        [SerializeField] private GameObject objectToHide;
        
        private Light _light;
        private bool _on;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _light.enabled = false;
            objectToHide.SetActive(false);
        }

        public void ActivateTorch()
        {
            FindFirstObjectByType<NpcAgent>().DestroyHeldObject();
            torchEnabled = true;
            AudioSource.PlayClipAtPoint(activateSound, transform.position);
            TutorialManager.instance.TorchTutorial();
            objectToHide.SetActive(true);
        }

        public void ToggleTorch(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!torchEnabled) return;
            _on = !_on;
            _light.enabled = _on;
            AudioSource.PlayClipAtPoint(torchSounds[UnityEngine.Random.Range(0, torchSounds.Length)], transform.position);
            PlayerFlagsManager.instance.usedTorch = true;
        }
    }
}
