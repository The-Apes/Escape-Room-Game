using TMPro;
using UnityEngine;

namespace UI
{
    public class NpcSubtitle : MonoBehaviour
    {
        private TextMeshProUGUI _subtitleText;
        private CanvasGroup _canvasGroup;
        [SerializeField] private bool player; // Indicates if the subtitle is for the player
    
        public bool shouldShow; // Controls whether the subtitle should be shown or not
        private bool _transitioning;

        private void Awake()
        {
            _subtitleText = GetComponentInChildren<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            int targetAlpha = shouldShow ? 1 : 0;
            if(_canvasGroup) _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, targetAlpha, Time.deltaTime * 10);
        }

        public void SetSubtitle(string line, bool player)
        {
            if (player == this.player) {
                if (_subtitleText)
                {
                    _subtitleText.text = line;
                }else
                {
                    Debug.LogError("Subtitle TextMeshPro component is not assigned.");
                }
            }
            else
            {
                _subtitleText.text = "";
            }
        }

    }
}
