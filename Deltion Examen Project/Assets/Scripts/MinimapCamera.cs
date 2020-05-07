using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Vector3 startPos;
    private Camera cam;
    private float startSize;
    public float minSize = 25;
    public float maxSize = 100;
    private bool tabbed;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        cam = GetComponent<Camera>();
        startSize = cam.orthographicSize;
        InputManager.scrollEvent += Scroll;
        InputManager.tabEvent += SetTabbedTrue;
        InputManager.tabUpEvent += SetTabbedFalse;
    }


    private void OnDestroy()
    {
        InputManager.scrollEvent -= Scroll;
        InputManager.tabEvent -= SetTabbedTrue;
        InputManager.tabUpEvent -= SetTabbedFalse;
    }

    private void SetTabbedTrue()
    {
        tabbed = true;
    }
    private void SetTabbedFalse()
    {
        tabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tabbed)
        {
            float x = Input.GetAxis("Mouse X");
            float z = Input.GetAxis("Mouse Y");

            Vector3 movement = new Vector3(-z, 0, x);
            transform.position = transform.position + movement * 120 * Time.deltaTime;
        }

        if (!tabbed)
        {
            transform.localPosition = startPos;
            cam.orthographicSize = startSize;
        }
    }

    void Scroll(float f)
    {
        if (tabbed)
        {
            if(cam.orthographicSize >= minSize && cam.orthographicSize <= maxSize)
                cam.orthographicSize += -f * 10000 * Time.deltaTime;
            if(cam.orthographicSize > maxSize)
            {
                cam.orthographicSize = maxSize;
            }
            if (cam.orthographicSize < minSize)
            {
                cam.orthographicSize = minSize;
            }
        }
    }
}
