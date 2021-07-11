using UnityEngine;

/// <summary>
/// Class that controls when the Firesound need to stop
/// <author> Vincent Holtorf, Joe Koelbel </author>
/// </summary>
public class FireSoundController : MonoBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the Fires")]
    private AudioSource fireSound;

    /// <summary>
    /// Connection to the tutorial manager
    /// </summary>
    [SerializeField, Tooltip("Connection to the tutorial manager")]
    private TutorialManager tutorialManager;

    #endregion

    #region Private Attributes

    /// <summary>
    /// How many fires are currently active
    /// </summary>
    private int activeFires = 0;

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds an active fire to the counter
    /// Activates the fire sound if the first fire starts
    /// </summary>
    public void AddActiveFire()
    {
        activeFires++;
        if (!fireSound.isPlaying)
        {
            fireSound.Play();
        }
    }

    /// <summary>
    /// Removes an active fire from the counter
    /// Deactivates the sound and progresses the tutorial if the last fire is removed
    /// </summary>
    public void RemoveActiveFire()
    {
        activeFires--;
        if (activeFires == 0)
        {
            fireSound.Stop();
            tutorialManager.TutProgress++;
            tutorialManager.ChangeTask();
        }
    }

    #endregion

}
