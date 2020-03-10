using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public Camera targetCam;
    public bool targetMainCamera = true;
    void Start()
    {
        if (targetMainCamera)
        {
            targetCam = Camera.main;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(targetCam.transform);
    }
}
