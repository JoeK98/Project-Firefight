using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System.Linq;

public class FireSound : MonoBehaviour
{
    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the Fires")]
    private AudioSource fireSound;

    public FireManager fM;

    public void MusicController()
    {
        int fireWindows = fM.Windows.Where(x => x.State == FireStates.ONFIRE).ToList().Count;

        if(fireWindows <= 0)
        {
            fireSound.Stop();
        }
    }

}
