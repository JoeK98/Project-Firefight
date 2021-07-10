using UnityEngine;

/// <summary>
/// Class that controls when the Firesound need to stop
/// <author> Vincent Holtorf, Joe Koelbel </author>
/// </summary>
public class FireSoundController : MonoBehaviour
{
    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the Fires")]
    private AudioSource fireSound;

    [SerializeField]
    private TutorialManager tM;

    private int activeFires = 0;

    public void AddActiveFire()
    {
        activeFires++;
        if (!fireSound.isPlaying)
        {
            fireSound.Play();
        }
    }

    public void RemoveActiveFire()
    {
        activeFires--;
        if (activeFires == 0)
        {
            fireSound.Stop();
            tM.TutProgress += 1;
        }
    }
}
