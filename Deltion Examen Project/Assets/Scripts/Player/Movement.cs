using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    private Vector3 forward;
    private Vector3 Right;

    void Awake()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        Right = Quaternion.Euler(0, 90, 0) * forward; 
    }

    public void Move(float xAxis, float yAxis)
    {
        //Vector3 toMove = new Vector3(xAxis, 0, yAxis);
        //toMove *= movementSpeed;
        //toMove *= Time.deltaTime;
        //transform.Translate(toMove);
        Vector3 direction = new Vector3(xAxis, 0, yAxis);
        Vector3 rightMovement = Right * movementSpeed * Time.deltaTime * xAxis;
        Vector3 upMovement = forward * movementSpeed * Time.deltaTime * yAxis;

        transform.position += rightMovement;
        transform.position += upMovement;
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