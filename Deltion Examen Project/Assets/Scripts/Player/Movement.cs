using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;

    private void Update()
    {
        Rotate();
    }

    public void Move(float xAxis, float yAxis)
    {
        Vector3 toMove = new Vector3(Mathf.Ceil(xAxis), 0, Mathf.Floor(yAxis));
        toMove *= Speed;
        transform.Translate(toMove);
    }

    public void Rotate()
    {

    }
}
