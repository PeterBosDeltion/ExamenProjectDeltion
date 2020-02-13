using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    public Vector3 forward;

    void Awake()
    {
        forward = Camera.main.transform.forward;
    }

    public void Move(float xAxis, float yAxis)
    {
        Vector3 toMove = new Vector3(xAxis, 0, yAxis);
        toMove *= movementSpeed;
        toMove *= Time.deltaTime;
        transform.Translate(toMove);
    }

    public void Rotate(float xAxis, float zAxis)
    {
        //Set the direction
        Vector3 targetPosition = new Vector3(xAxis, transform.position.y, zAxis);
        Vector3 direction = (targetPosition - transform.position).normalized;

        //Get the rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //Set the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}