using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalEmission;
    private bool isHighlighted = false;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && objectRenderer.material.HasProperty("_EmissionColor"))
            originalEmission = objectRenderer.material.GetColor("_EmissionColor");
    }

    public void Highlight(bool enable)
    {
        if (objectRenderer == null || !objectRenderer.material.HasProperty("_EmissionColor")) return;

        if (enable && !isHighlighted)
        {
            objectRenderer.material.EnableKeyword("_EMISSION");
            objectRenderer.material.SetColor("_EmissionColor", Color.yellow * 2f); // Hard-coded glow
            isHighlighted = true;
        }
        else if (!enable && isHighlighted)
        {
            objectRenderer.material.SetColor("_EmissionColor", originalEmission);
            isHighlighted = false;
        }
    }
}