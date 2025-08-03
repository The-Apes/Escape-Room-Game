using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private RectTransform rectTransform;
    private Camera _cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_cam.transform);
        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        
        float textSize = Vector3.Distance(transform.position, _cam.transform.position)/ 10;
        
        rectTransform.localScale = new Vector3(textSize, textSize, textSize);
   
    }
}
