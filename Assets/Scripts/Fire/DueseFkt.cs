using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class DueseFkt : MonoBehaviour
{

    [System.Serializable]
    public class ButtonPressedEvent : UnityEvent { }
    [System.Serializable]
    public class ButtonReleasedEvent : UnityEvent { }

    public ButtonPressedEvent OnButtonPressed;
    public ButtonReleasedEvent OnButtonReleased;

    bool pressed = false;
 
    void Update()
    {
       
        /*if(Input.GetButton("SecondaryIndexTrigger"))
        {
            HitFire();
        }*/

    }

    // Update is called once per frame
    void HitFire()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.right;

        Debug.DrawRay(origin, direction * 10f, Color.blue);
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            raycastHit.collider.GetComponent<FireRules>().fireHP -= 2;
        }

    }
}
