using TMPro;
using UnityEngine;

public class Subtitle : MonoBehaviour
{
    private TextMeshProUGUI _subtitleText;

    private void Awake()
    {
        _subtitleText = GetComponent<TextMeshProUGUI>();
    }

    public void SetSubtitle(string line)
    {
        if (_subtitleText)
        {
            _subtitleText.text = line;
        }else
        {
            Debug.LogError("Subtitle TextMeshPro component is not assigned.");
        }
    }
}
