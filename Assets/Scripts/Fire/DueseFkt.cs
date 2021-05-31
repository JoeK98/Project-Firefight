using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DueseFkt : MonoBehaviour
{
    private int dmg = 2;
    void Update()
    {
        
    }

    // Update is called once per frame
    public void HitFire()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * 10f, Color.blue);
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 20, 1<<3))
        {
           FireRules frcollider = raycastHit.collider.GetComponent<FireRules>();
            
           if(frcollider != null)
            {
                frcollider.fireHP -= dmg;
            }
            
        }

    }
}
