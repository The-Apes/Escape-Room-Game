using UnityEngine;

public class CameraMover : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * Time.deltaTime); 
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up, -90 * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, 90 * Time.deltaTime);
        
        // if(Input.anyKey == false)
        // {
        //     // If no keys are pressed, reset the camera position
        //     transform.position = new Vector3(0, 10, -10);
        //     transform.rotation = Quaternion.Euler(30, 0, 0);
        // }
        
    }
}
