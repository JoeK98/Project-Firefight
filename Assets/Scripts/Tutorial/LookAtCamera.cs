using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Function for the textfields to look at the Player
/// <author> Vincent Holtorf </author>
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    /// <summary>
    /// The connection for the Camera
    /// </summary>
    public Camera m_Camera;

    /// <summary>
    /// Orient the camera after all movement is completed this frame to avoid jittering
    /// </summary>
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            Vector3.up);
    }
}
