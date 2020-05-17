using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeployedSlowField : MonoBehaviour
{
    private float slowPercent;
    private float myRadius;
    private Dictionary<Enemy, float> enemies = new Dictionary<Enemy, float>();
    private LineRenderer myLineRenderer;

    [Range(3, 256)]
    private int nSegments = 128;
    public void Initialize(float slowPercentage, float radius)
    {
        slowPercent = slowPercentage;
        myRadius = radius;
        GetComponent<SphereCollider>().radius = radius;
        myLineRenderer = GetComponent<LineRenderer>();
        DrawRangeCircle();
    }
    private void DrawRangeCircle()
    {

        myLineRenderer.enabled = true;
        //myLineRenderer.startWidth = 0.25F;
        //myLineRenderer.endWidth = 0.25F;
        myLineRenderer.positionCount = nSegments + 1;
        myLineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / nSegments;
        float theta = 0f;

        for (int i = 0; i < nSegments + 1; i++)
        {
            float x = myRadius * Mathf.Cos(theta);
            float z = myRadius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, .5F, z);
            myLineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy e = other.GetComponent<Enemy>();
            if (!enemies.ContainsKey(e))
            {
                enemies.Add(e, e.speed);
                float slowPercantage = slowPercent / 100;
                float speedReduction = e.speed * slowPercantage;
                e.speed -= speedReduction;
                e.GetComponent<NavMeshAgent>().speed = e.speed;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy e = other.GetComponent<Enemy>();
            if (enemies.ContainsKey(e))
            {
                e.speed = enemies[e];
                e.GetComponent<NavMeshAgent>().speed = e.speed;
                enemies.Remove(e);
            }
        }
    }

    public void ResetValues()
    {
        foreach (var kvp in enemies)
        {
            if(kvp.Key != null)
            {
                Enemy e = kvp.Key;
                e.speed = kvp.Value;
                e.GetComponent<NavMeshAgent>().speed = e.speed;
            }
        }
    }
}
