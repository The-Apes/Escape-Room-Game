using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Subtitle : MonoBehaviour
{
    private TextMeshProUGUI _subtitleText;
    [SerializeField] private bool player; // Indicates if the subtitle is for the player

    private void Awake()
    {
        _subtitleText = GetComponent<TextMeshProUGUI>();
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
