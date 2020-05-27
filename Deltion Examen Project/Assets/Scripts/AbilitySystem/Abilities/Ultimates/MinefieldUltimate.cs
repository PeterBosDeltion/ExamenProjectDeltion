using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinefieldUltimate : Ability
{
    public GameObject aircraftPrefab;
    public GameObject minePrefab;
    public int amountOfMines = 30;
    public Vector3 offsetFromPlayer = new Vector3();
    private GameObject spawnedShip;

    public float targetRadius;


    private Vector3 centerPoint;
    public GameObject centerPointLineRendererPrefab;
    private GameObject centerLineRend;
    private LineRenderer myLineRenderer;

    [Range(3, 256)]
    private int nSegments = 128;

    private List<GameObject> deployedMines = new List<GameObject>();
    private bool waiting;
    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        deployedMines.Clear();

        centerPoint = (Vector3)mPos;
        centerLineRend = Instantiate(centerPointLineRendererPrefab, centerPoint, Quaternion.identity);
        spawnedShip = Instantiate(aircraftPrefab, centerPoint + offsetFromPlayer, Quaternion.identity);
        myLineRenderer = centerLineRend.GetComponent<LineRenderer>();
        myLineRenderer.startColor = GameManager.instance.playerColors[myPlayer.GetComponent<PlayerController>().playerNumber];
        myLineRenderer.endColor = GameManager.instance.playerColors[myPlayer.GetComponent<PlayerController>().playerNumber];
        
        //spawnedShip.transform.localEulerAngles += new Vector3(0, -90, 0);

        DrawRangeCircle();
        DeployLocation(centerPoint);
        for (int i = 0; i < amountOfMines; i++)
        {
            Deploy();
        }

        if (!waiting)
            StartCoroutine(DestroyWronglyPlacedMines());
        active = true;
    }

    private Vector3 FindPoint()
    {
        Vector3 point = new Vector2(centerPoint.x, centerPoint.z) + Random.insideUnitCircle * targetRadius;
        point.z = point.y;
        point.y = 1.6F;
        return point;
    }

    private void Deploy()
    {
        GameObject mine = Instantiate(minePrefab, FindPoint() + new Vector3(0, offsetFromPlayer.y, 0), Quaternion.identity);
        deployedMines.Add(mine);
    }


    private void DeployLocation(Vector3 position)
    {
        GameObject mine = Instantiate(minePrefab, position + new Vector3(0, offsetFromPlayer.y, 0), Quaternion.identity);
        deployedMines.Add(mine);
    }

    private IEnumerator DestroyWronglyPlacedMines()
    {
        waiting = true;
        yield return new WaitForSeconds(3F);
        foreach (GameObject mine in deployedMines)
        {
            if(mine != null)
            {
                if (mine.transform.position.y >= myPlayer.transform.position.y + 1)
                    mine.GetComponent<ExplosiveProp>().Explode();
            }
          
        }

        waiting = false;
    }


    private void DrawRangeCircle()
    {

        myLineRenderer.enabled = true;
        myLineRenderer.startWidth = 0.25F;
        myLineRenderer.endWidth = 0.25F;
        myLineRenderer.positionCount = nSegments + 1;
        myLineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / nSegments;
        float theta = 0f;

        for (int i = 0; i < nSegments + 1; i++)
        {
            float x = targetRadius * Mathf.Cos(theta);
            float z = targetRadius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, .5F, z);
            myLineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }



    }



    public override IEnumerator AfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(centerLineRend);
        CancelInvoke();
        Destroy(spawnedShip);
        spawnedShip = null;
        deployedMines.Clear();
        StartCooldown();
    }
}
