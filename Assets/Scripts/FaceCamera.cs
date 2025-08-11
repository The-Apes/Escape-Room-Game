using UI;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private RectTransform _rectTransform;
    private MeshRenderer _meshRenderer;
    private NpcSubtitle _npcSubtitle;
    private Camera _cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _npcSubtitle = FindFirstObjectByType<NpcSubtitle>();
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_cam.transform);
        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        
        float textSize = Vector3.Distance(transform.position, _cam.transform.position)/ 10;
        
        _rectTransform.localScale = new Vector3(textSize, textSize, textSize);
   
        if (_npcSubtitle) _npcSubtitle.shouldShow = !_meshRenderer.isVisible;
    }
}
