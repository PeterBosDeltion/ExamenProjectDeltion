using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
           Shoot();
        }
        if (Input.GetKeyDown("r"))
        {
            Reload();
        }
    }
}
