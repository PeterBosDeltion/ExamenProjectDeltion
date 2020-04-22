using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Player myPlayer;
    public new string name;
    [TextArea(15, 20)]
    public  string description;
    public Sprite uiIcon;
    public int requiredLevel;
    public bool ultimate;
    public bool onCooldown;
    public bool active = false;
    public float cooldownTime;
    public float duration;
    [HideInInspector]
    public float currentUltCharge;
    public float ultChargeIncrement;
    public float deployableRadius = 5F;
    public GameObject deployableGhost;
    public Material canDeployMaterial;
    public Material cannotDeployMaterial;
    public Material deployLineMaterial;
    private GameObject activeGhost;
    private Quaternion ghostRotation;
    public Vector3 deployableOffset;
    [Range(3, 256)]
    private int numSegments = 128;
    [HideInInspector]
    public bool returned;

    private bool deploying;
    private LineRenderer lineRenderer;
    private bool ultActive;
    private bool checkUltCharged;
    public enum DeployType
    {
        Instant,
        Deployed,
        LaserTarget
    }
    public DeployType myDeployType;

    protected Coroutine afterDurCoroutine;

    private void Start()
    {
        InputManager.rightMouseButtonEvent += CancelDeploy;
        InputManager.leftMouseButtonEvent += Deploy;

        lineRenderer = myPlayer.gameObject.GetComponent<LineRenderer>();
    }

    private void OnDestroy()
    {
        InputManager.rightMouseButtonEvent -= CancelDeploy;
        InputManager.leftMouseButtonEvent -= Deploy;
    }
    public void UseAbility()
    {
        if(ultimate && currentUltCharge < 100)
        {
            return;
        }
        if (!onCooldown && !active && !deploying)
        {
            returned = false;
            switch (myDeployType)
            {
                case DeployType.Instant:
                    AbilityMechanic();
                    InputManager.delayedAbilityEvent.Invoke(myPlayer.GetComponent<PlayerController>().abilities.IndexOf(this));
                    break;
                case DeployType.Deployed:
                    activeGhost = null;
                    if (!deploying && !active)
                    {
                        DrawDeployCircle();
                    }
                    break;
                case DeployType.LaserTarget:
                    break;
            }

            if (ultimate)
            {
                currentUltCharge = 0;
                ultActive = true;
                checkUltCharged = false;
            }
            afterDurCoroutine = StartCoroutine(AfterDuration());
        }
       
    }

    private void Update()
    {
        if (deploying && !active)
        {
            if (activeGhost)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    Vector3 mPos = hit.point + deployableOffset;
                    activeGhost.transform.position = mPos;
                    ghostRotation = Quaternion.LookRotation(activeGhost.transform.position - myPlayer.transform.position);
                    activeGhost.transform.rotation = ghostRotation;
                    Vector3 eul = activeGhost.transform.localEulerAngles;
                    activeGhost.transform.localEulerAngles = new Vector3(0, eul.y, 0);


                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                    {
                        if(activeGhost.GetComponentInChildren<Renderer>().material != canDeployMaterial)
                        {
                            activeGhost.GetComponentInChildren<Renderer>().material = canDeployMaterial;
                        }
                    }
                    else
                    {
                        if (activeGhost.GetComponentInChildren<Renderer>().material != cannotDeployMaterial)
                        {
                            activeGhost.GetComponentInChildren<Renderer>().material = cannotDeployMaterial;
                        }
                    }

                    float radius = deployableRadius;
                    Vector3 centerPosition = myPlayer.transform.position; //center of *black circle*
                    float distance = Vector3.Distance(activeGhost.transform.position, centerPosition); //distance from ~green object~ to *black circle*

                    if (distance > radius) //If the distance is less than the radius, it is already within the circle.
                    {
                        Vector3 fromOriginToObject = activeGhost.transform.position - centerPosition; //~GreenPosition~ - *BlackCenter*
                        fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
                        activeGhost.transform.position = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
                    }
                }
            }
        }
    }

    private void Deploy()
    {
        if(deploying && !active)
        {
            if (myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot)
            {
                myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot = false;
            }
            RaycastHit hit;
            Vector3 center = myPlayer.transform.position;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    Vector3 mPos = hit.point + deployableOffset;
                    if (Vector3.Distance(mPos, center) <= deployableRadius)
                    {
                        AbilityMechanic(mPos, new Quaternion(0, ghostRotation.y, 0, ghostRotation.w));
                        deploying = false;
                        myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot = true;
                        lineRenderer.enabled = false;
                        Destroy(activeGhost);
                        activeGhost = null;
                        InputManager.delayedAbilityEvent.Invoke(myPlayer.GetComponent<PlayerController>().abilities.IndexOf(this));
                    }
                }
                else
                {
                    onCooldown = false;
                    returned = true;
                    deploying = false;
                    myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot = true;
                    lineRenderer.enabled = false;
                    Destroy(activeGhost);
                    activeGhost = null;
                    active = false;
                    return;
                }

            }
        }
    }

    private void CancelDeploy()
    {
        if (deploying && !active)
        {
            lineRenderer.enabled = false;
            if (!myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot)
            {
                myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot = true;
            }

            onCooldown = false;
            returned = true;
            Destroy(activeGhost);
            activeGhost = null;

            active = false;
            deploying = false;
        }
       
    }

    protected abstract void AbilityMechanic(Vector3? mPos = null, Quaternion? deployRotation = null);

    private void DrawDeployCircle()
    {
        if(!active)
        {
            deploying = true;
            lineRenderer.enabled = true;
            lineRenderer.material = deployLineMaterial;
            lineRenderer.startWidth = 0.25F;
            lineRenderer.endWidth = 0.25F;
            lineRenderer.positionCount = numSegments + 1;
            lineRenderer.useWorldSpace = false;

            float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
            float theta = 0f;

            for (int i = 0; i < numSegments + 1; i++)
            {
                float x = deployableRadius * Mathf.Cos(theta);
                float z = deployableRadius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, .5F, z);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }

            if (!activeGhost)
            {
                activeGhost = Instantiate(deployableGhost);
            }
        }
      
    }
    private IEnumerator Cooldown(float seconds)
    {
        onCooldown = true;
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

    protected void StartCooldown()
    {
        if (!this.onCooldown)
        {
            active = false;
            if (ultActive)
            {
                ultActive = false;
            }
            StartCoroutine(Cooldown(cooldownTime));
        }
    }

    protected abstract IEnumerator AfterDuration();

    public void IncrementUltCharge()
    {
        if (!ultActive)
        {
            if (currentUltCharge < 100)
            {
                currentUltCharge += ultChargeIncrement;
            }
            else
            {
                currentUltCharge = 100;
            }

            if(currentUltCharge >= 100)
            {
                if (!checkUltCharged)
                {
                    AudioClipManager.instance.PlayClipOneShotWithSource(myPlayer.mySource, AudioClipManager.instance.GetRandomUltReadyVL(myPlayer));
                    checkUltCharged = true;
                }
            }
        }
       

    }
}
