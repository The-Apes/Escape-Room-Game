using UnityEngine;
using UnityEngine.UI;

public class LightPulseUI : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Range(0f, 1f)] public float minAlpha = 0.5f;
    [Range(0f, 1f)] public float maxAlpha = 1f; 
    public float pulseSpeed = 2f; 

    private Image lightImage;
    private Color startColor;

    void Start()
    {
        lightImage = GetComponent<Image>();
        startColor = lightImage.color;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color newColor = startColor;
        newColor.a = alpha;
        lightImage.color = newColor;
    }
}
