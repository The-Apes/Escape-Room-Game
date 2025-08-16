using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NpcSubtitle : MonoBehaviour
    {
        public bool shouldShow; // Controls whether the subtitle should be shown or not
        [SerializeField] private bool player; // Indicates if the subtitle is for the player

        private TextMeshProUGUI _subtitleText;
        private Image _backgroundImage; 
        private CanvasGroup _canvasGroup;
        private bool _transitioning;

        private void Awake()
        {
            _subtitleText = GetComponentInChildren<TextMeshProUGUI>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _backgroundImage = GetComponent<Image>();
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0.0f);
        }

        private void Update()
        {
            int targetAlpha = shouldShow ? 1 : 0;
            if(_canvasGroup) _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, targetAlpha, Time.deltaTime * 10);
        }

        public void SetSubtitle(string line, bool player, float duration)
        {
            if (player == this.player) {
                StartCoroutine(SetSubtitleCoroutine(line, duration));
            }
            // else
            // {
            //     _subtitleText.text = "";
            // }
        }

        private IEnumerator SetSubtitleCoroutine(string line, float duration)
        {
            if (_subtitleText)
            {
                _subtitleText.text = line;
                _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0.3f);
                yield return new WaitForSeconds(duration);
                _subtitleText.text = "";
                _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0.0f);

            }else
            {
                Debug.LogError("Subtitle TextMeshPro component is not assigned.");
            }
        }

    }
}
