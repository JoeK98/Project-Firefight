using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for activating the first Fire
/// <author> Vincent Holtorf </author>
/// </summary>
public class FireManager : MonoBehaviour
{

    /// <summary>
    /// Array of all the Fires in the scene
    /// </summary>
    [SerializeField] private FireRules[] fires;

    public FireRules[] Fires { get => fires; set => fires = value; }

    /// <summary>
    /// Activate random the first fire at the start of the level
    /// </summary>
    void Start()
    {
        int windowIndex = Random.Range(0, Fires.Length);
        Fires[windowIndex].gameObject.SetActive(true);
    }

}
