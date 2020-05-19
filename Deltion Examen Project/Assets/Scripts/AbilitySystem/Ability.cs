using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Player myPlayer;
    public PlayerController myPlayerController;
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
    private Vector3 joy = new Vector3(0, 0, 0);
    [Range(3, 256)]
    private int numSegments = 128;
    [HideInInspector]
    public bool returned;

    private bool deploying;
    private LineRenderer lineRenderer;
    private bool ultActive;
    private bool checkUltCharged;
    public float controllerLaserStartDist = 5;
    public float controllerLaserMaxDist = 12.5F;
    public float controllerLaserMinDist = 3F;
    private float controllerLaserDist;
    public enum DeployType
    {
        Instant,
        Deployed,
        LaserTarget
    }
    public DeployType myDeployType;

    public Coroutine afterDurCoroutine;
    private GameObject laserTargetObject;
    private bool lasering;

    private void Start()
    {
        lineRenderer = myPlayer.gameObject.GetComponent<LineRenderer>();

        myPlayerController.myInputManager.rightMouseButtonEvent += CancelDeploy;
        myPlayerController.myInputManager.rightMouseButtonEvent += CancelLaser;
        myPlayerController.myInputManager.leftMouseButtonEvent += Deploy;
        myPlayerController.myInputManager.leftMouseButtonEvent += UseLaserTarget;
        myPlayerController.myInputManager.bumperEvent += ControllerLaserDistance;
    }

    private void OnDestroy()
    {
        myPlayerController.myInputManager.rightMouseButtonEvent -= CancelDeploy;
        myPlayerController.myInputManager.rightMouseButtonEvent -= CancelLaser;
        myPlayerController.myInputManager.leftMouseButtonEvent -= Deploy;
        myPlayerController.myInputManager.leftMouseButtonEvent -= UseLaserTarget;
        myPlayerController.myInputManager.bumperEvent -= ControllerLaserDistance;
    }
    public void UseAbility()
    {
        if(ultimate && currentUltCharge < 100)
        {
            return;
        }
        if (!onCooldown && !active && !deploying && !lasering)
        {
            returned = false;
            switch (myDeployType)
            {
                case DeployType.Instant:
                    AbilityMechanic();
                    myPlayerController.myInputManager.delayedAbilityEvent.Invoke(myPlayer.GetComponent<PlayerController>().abilities.IndexOf(this));
                    break;
                case DeployType.Deployed:
                    activeGhost = null;
                    joy = Vector3.zero;
                    if (!deploying && !active)
                    {
                        DrawDeployCircle();
                    }
                    break;
                case DeployType.LaserTarget:
                    if (!lasering)
                    {
                        laserTargetObject = new GameObject("lasertarget");
                        controllerLaserDist = controllerLaserStartDist;
                        laserTargetObject.transform.position = myPlayer.transform.position;
                        lasering = true;
                        myPlayerController.currentWeapon.canShoot = false;
                        myPlayerController.canSwitch = false;

                    }
                    break;
            }

            //if (ultimate)
            //{
            //    currentUltCharge = 0;
            //    ultActive = true;
            //    checkUltCharged = false;
            //}

            if(myDeployType != DeployType.Deployed) //Deployables need to call this manually in the deployed object's Initialize function
                afterDurCoroutine = StartCoroutine(AfterDuration());
        }
       
    }

    private void UseLaserTarget()
    {
        if (laserTargetObject)
        {
            Vector3 laserPos = laserTargetObject.transform.position;
            AbilityMechanic(laserPos);

            StartCoroutine(WaitTillUnlockGun());
            Destroy(laserTargetObject);
            laserTargetObject = null;
            myPlayerController.currentWeapon.CancelLaser();
            lasering = false;
            myPlayerController.canSwitch = true;

            if (ultimate)
            {
                currentUltCharge = 0;
                ultActive = true;
                checkUltCharged = false;
            }
        }
    }

    private void CancelLaser()
    {
        if (lasering)
        {
            myPlayerController.currentWeapon.CancelLaser();
            Destroy(laserTargetObject);
            laserTargetObject = null;
            myPlayerController.currentWeapon.canShoot = true;
            myPlayerController.canSwitch = true;
            lasering = false;

        }

    }

    private void ControllerLaserDistance(float f)
    {
        if (lasering)
        {
            if (controllerLaserDist > controllerLaserMaxDist)
            {
                controllerLaserDist = controllerLaserMaxDist;
            }
            if (controllerLaserDist < controllerLaserMinDist)
            {
                controllerLaserDist = controllerLaserMinDist;
            }

            if (controllerLaserDist <= controllerLaserMaxDist && controllerLaserDist >= controllerLaserMinDist)
            {
                controllerLaserDist += f;
            }
           
        }
    }

    private void Update()
    {
        if (lasering)
        {
            if (myPlayerController.myInputManager.controllerIndex < 0)
            {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                        laserTargetObject.transform.position = hit.point;
                    myPlayerController.currentWeapon.SetLaser(laserTargetObject);

            }
            else if (myPlayerController.myInputManager.controllerIndex >= 0)
            {
                if (hinput.gamepad[myPlayerController.myInputManager.controllerIndex].isConnected)
                {
                    laserTargetObject.transform.position = myPlayer.transform.position + myPlayer.transform.forward * controllerLaserDist;
                    myPlayerController.currentWeapon.SetLaser(laserTargetObject);
                }

            }
           

        }
       
        if (deploying && !active)
        {
            if (activeGhost)
            {
                RaycastHit hit;
                if (myPlayerController.myInputManager.mouseKeyBoard)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                    {
                        Vector3 mPos = hit.point + deployableOffset;
                        activeGhost.transform.position = mPos;
                        ghostRotation = Quaternion.LookRotation(activeGhost.transform.position - myPlayer.transform.position);
                        activeGhost.transform.rotation = ghostRotation;
                        Vector3 eul = activeGhost.transform.localEulerAngles;
                        activeGhost.transform.localEulerAngles = new Vector3(0, eul.y, 0);


                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor") && mPos.y - deployableOffset.y <= myPlayer.transform.position.y + 0.1F && mPos.y - deployableOffset.y >= myPlayer.transform.position.y - 0.1F)
                        {
                            Renderer[] rends = activeGhost.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rends)
                            {
                                if (r.material != canDeployMaterial)
                                {
                                    r.material = canDeployMaterial;
                                }
                            }
                           
                        }
                        else
                        {
                            Renderer[] rends = activeGhost.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rends)
                            {
                                if (r.material != cannotDeployMaterial)
                                {
                                    r.material = cannotDeployMaterial;
                                }
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
                else
                {
                    joy.x += -myPlayerController.myInputManager.padRSAxisX / 1.5F;
                    joy.z += -myPlayerController.myInputManager.padRSAxisY / 1.5F;


                    Vector3 gPos = myPlayer.transform.position + (joy + deployableOffset);
                    activeGhost.transform.position = gPos;
                    ghostRotation = Quaternion.LookRotation(activeGhost.transform.position - myPlayer.transform.position);
                    activeGhost.transform.rotation = ghostRotation;
                    Vector3 eul = activeGhost.transform.localEulerAngles;
                    activeGhost.transform.localEulerAngles = new Vector3(0, eul.y, 0);


                    if(Physics.Raycast(gPos + new Vector3(0, 1.5F, 0), -myPlayer.transform.up, out hit, Mathf.Infinity))
                    {
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor") && gPos.y - deployableOffset.y <= myPlayer.transform.position.y + 0.1F && gPos.y - deployableOffset.y >= myPlayer.transform.position.y - 0.1F)
                        {
                            Renderer[] rends = activeGhost.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rends)
                            {
                                if (r.material != canDeployMaterial)
                                {
                                    r.material = canDeployMaterial;
                                }
                            }
                              
                        }
                        else
                        {
                            Renderer[] rends = activeGhost.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rends)
                            {
                                if (r.material != cannotDeployMaterial)
                                {
                                    r.material = cannotDeployMaterial;
                                }
                            }
                        }

                        float radius = deployableRadius;
                        Vector3 centerPosition = myPlayer.transform.position; //center of *black circle*
                        float distance = Vector3.Distance(gPos - deployableOffset, centerPosition); //distance from ~green object~ to *black circle*

                        if (distance > radius) //If the distance is less than the radius, it is already within the circle.
                        {
                            Vector3 fromOriginToObject = (gPos - deployableOffset) - centerPosition; //~GreenPosition~ - *BlackCenter*
                            fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
                            activeGhost.transform.position = centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
                            joy = activeGhost.transform.position - myPlayer.transform.position;

                            Renderer[] rends = activeGhost.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in rends)
                            {
                                if (r.material != cannotDeployMaterial)
                                {
                                    r.material = cannotDeployMaterial;
                                }
                            }
                        }
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
            if (myPlayerController.myInputManager.mouseKeyBoard)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                    {
                        Vector3 mPos = hit.point + deployableOffset;
                        if (Vector3.Distance(mPos, center) <= deployableRadius && mPos.y - deployableOffset.y <= myPlayer.transform.position.y + 0.1F && mPos.y - deployableOffset.y >= myPlayer.transform.position.y - 0.1F)
                        {
                            AbilityMechanic(mPos, new Quaternion(0, ghostRotation.y, 0, ghostRotation.w));
                            deploying = false;
                            StartCoroutine(WaitTillUnlockGun());
                            lineRenderer.enabled = false;
                            Destroy(activeGhost);
                            activeGhost = null;
                            myPlayerController.myInputManager.delayedAbilityEvent.Invoke(myPlayer.GetComponent<PlayerController>().abilities.IndexOf(this));
                        }
                    }
                    else
                    {
                        onCooldown = false;
                        returned = true;
                        deploying = false;
                        StartCoroutine(WaitTillUnlockGun());
                        lineRenderer.enabled = false;
                        Destroy(activeGhost);
                        activeGhost = null;
                        active = false;
                        return;
                    }

                }
            }
            else
            {
                if (Physics.Raycast(activeGhost.transform.position + new Vector3(0, 1.5F, 0), -myPlayer.transform.up, out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
                    {
                        Vector3 mPos = hit.point + deployableOffset;
                        if (Vector3.Distance(mPos, center) <= deployableRadius && mPos.y - deployableOffset.y <= myPlayer.transform.position.y + 0.1F && mPos.y - deployableOffset.y >= myPlayer.transform.position.y - 0.1F)
                        {
                            AbilityMechanic(mPos, new Quaternion(0, ghostRotation.y, 0, ghostRotation.w));
                            deploying = false;
                            StartCoroutine(WaitTillUnlockGun());
                            lineRenderer.enabled = false;
                            Destroy(activeGhost);
                            activeGhost = null;
                            myPlayerController.myInputManager.delayedAbilityEvent.Invoke(myPlayer.GetComponent<PlayerController>().abilities.IndexOf(this));
                        }
                    }
                    else
                    {
                        onCooldown = false;
                        returned = true;
                        deploying = false;
                        StartCoroutine(WaitTillUnlockGun());
                        lineRenderer.enabled = false;
                        Destroy(activeGhost);
                        activeGhost = null;
                        active = false;
                        return;
                    }

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
            //lineRenderer.startWidth = 0.25F;
            //lineRenderer.endWidth = 0.25F;
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

    public abstract IEnumerator AfterDuration();

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

    public IEnumerator WaitTillUnlockGun()
    {
        yield return new WaitForSeconds(0.1f);
        myPlayer.GetComponent<PlayerController>().currentWeapon.canShoot = true;
    }
}
