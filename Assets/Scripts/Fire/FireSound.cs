using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class that controls when the Firesound need to stop
/// <author> Vincent Holtorf </author>
/// </summary>
public class FireSound : MonoBehaviour
{
    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the Fires")]
    private AudioSource fireSound;

    /// <summary>
    /// The connection to the FireManager
    /// </summary>
    public FireManager fM;

    /// <summary>
    /// Check if any Fire in the Scene has the State ONFIRE, when none Fire hasn´t the state, stop the music
    /// </summary>
    public void MusicController()
    {
        int fireWindows = fM.Fires.Where(x => x.State == FireStates.ONFIRE).ToList().Count;

        if(fireWindows <= 0)
        {
            fireSound.Stop();
        }
    }

}
