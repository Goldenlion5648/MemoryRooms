using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //for (int i = 0; i < Mathf.Pow(SceneScript.cubeDim, 3); i++)
        //{
        //    MeshRenderer mesh = GameObject.Find(SceneScript.textPartName + i).GetComponent<MeshRenderer>();
        //    mesh.enabled = !mesh.enabled;
        //}
        //Cursor.visible = false;
    }


    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            if (pitch - speedV * Input.GetAxis("Mouse Y") > -90 && pitch - speedV * Input.GetAxis("Mouse Y") < 90)
                pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        



    }


}
