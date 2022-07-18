using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;
    float xRotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // get the input
        float mouseX = Input.GetAxis("Mouse X") * sensHori * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensVert * Time.deltaTime;

        //invert the look
        if(invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }
        //clamp the angle the camera can rotate to
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);
        //rotate the camera on the X Axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // rotate the transform
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
