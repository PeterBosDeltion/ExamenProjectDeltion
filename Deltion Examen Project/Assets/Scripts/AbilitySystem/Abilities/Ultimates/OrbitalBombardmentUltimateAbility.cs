using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBombardmentUltimateAbility : Ability
{
    public GameObject shipShadowPrefab;
    public GameObject targetingSprite;
    public Vector3 offsetFromPlayer = new Vector3();
    private GameObject spawnedShip;

    public float targetRadius;
    public float damageRadius;
    public float damage;


    private Vector3 centerPoint;
    public GameObject centerPointLineRendererPrefab;
    private GameObject centerLineRend;
    private LineRenderer myLineRenderer;

    [Range(3, 256)]
    private int nSegments = 128;
    protected override void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null)
    {
        centerPoint = (Vector3)mPos;
        centerLineRend = Instantiate(centerPointLineRendererPrefab, centerPoint, Quaternion.identity);
        spawnedShip = Instantiate(shipShadowPrefab, centerPoint + offsetFromPlayer, Quaternion.identity);
        myLineRenderer = centerLineRend.GetComponent<LineRenderer>();
        myLineRenderer.startColor = GameManager.instance.playerColors[myPlayer.GetComponent<PlayerController>().playerNumber];
        myLineRenderer.endColor = GameManager.instance.playerColors[myPlayer.GetComponent<PlayerController>().playerNumber];
        spawnedShip.transform.localEulerAngles += new Vector3(0, -90, 0);
        DrawRangeCircle();
        BombardLocation(centerPoint + new Vector3(0, .2F, 0));
        InvokeRepeating("Bombard", .1F, 0.75F);
    
        active = true;
    }

    private Vector3 FindPoint()
    {
        Vector3 point = new Vector2(centerPoint.x, centerPoint.z) + Random.insideUnitCircle * targetRadius;
        point.z = point.y;
        point.y = 1.6F;
        return point;
    }

    private void Bombard()
    {
        GameObject spr = Instantiate(targetingSprite, FindPoint(), Quaternion.identity);
        spr.GetComponent<OrbitalBombardmentUltimate>().Initialize(damageRadius, damage, myPlayer);
    }
       

    private void BombardLocation(Vector3 position)
    {
        GameObject spr = Instantiate(targetingSprite, position, Quaternion.identity);
        spr.GetComponent<OrbitalBombardmentUltimate>().Initialize(damageRadius, damage, myPlayer);
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
        StartCooldown();
    }
}
