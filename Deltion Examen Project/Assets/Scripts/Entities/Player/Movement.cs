﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;

    private Vector3 forward;
    private Vector3 right;

    private Vector3 lastRecordedPosition;
    public float distanceTillRetarget;

    private void Awake()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(0, 90, 0) * forward;
        lastRecordedPosition = transform.position;
    }

    private void Update()
    {
        CheckIfMoved();
    }

    public void Move(float xAxis, float yAxis)
    {
        Vector3 rightMovement = right * movementSpeed * Time.deltaTime * xAxis;
        Vector3 upMovement = forward * movementSpeed * Time.deltaTime * yAxis;

        Vector3 toMove = rightMovement + upMovement;
        transform.position += toMove;
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

    private void CheckIfMoved()
    {
        if(Vector3.Distance(lastRecordedPosition,transform.position) > distanceTillRetarget)
        {
            lastRecordedPosition = transform.position;
            EntityManager.instance.RetargetPlayer();
        }
    }
}