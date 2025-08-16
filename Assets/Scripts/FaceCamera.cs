using UI;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private RectTransform _rectTransform;
    private MeshRenderer _meshRenderer;
    private Subtitle _subtitle;
    private Camera _cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        Subtitle[] subs = FindObjectsByType<Subtitle>(FindObjectsSortMode.None);

        foreach (Subtitle sub in subs)
        {
            if (sub.player) continue;
            _subtitle = sub;
            break;
        }
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_cam.transform);
        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        
        float textSize = Vector3.Distance(transform.position, _cam.transform.position)/ 10;

        if (!_rectTransform) return;
            _rectTransform.localScale = new Vector3(textSize, textSize, textSize);
        if (_subtitle) _subtitle.shouldShow = !_meshRenderer.isVisible;
    }
}
