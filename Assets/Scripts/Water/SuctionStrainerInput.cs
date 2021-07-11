using UnityEngine;

/// <summary>
/// Class that reacts for the suction strainer to entering the lake
/// <author> Joe Koelbel </author>
/// </summary>
public class SuctionStrainerInput : MonoBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// The suction strainer
    /// </summary>
    [SerializeField, Tooltip("The suction strainer")]
    private SuctionStrainerController suctionStrainer;

    #endregion

    #region MonoBehaviour implementation

    /// <summary>
    /// OnTriggerEnter is called when another Collider enters the trigger
    /// </summary>
    /// <param name="other"> the entering collider </param>
    private void OnTriggerEnter(Collider other)
    {
        // If the other collider is the lake -> tell the suction strainer that it is connected to the lake
        if (other.CompareTag("Lake"))
        {
            suctionStrainer.SetConnectionToLake(true);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when another Collider exits the trigger
    /// </summary>
    /// <param name="other"> the exiting collider </param>
    private void OnTriggerExit(Collider other)
    {
        // If the other collider is the lake -> tell the suction strainer that it is not connected to the lake
        if (other.CompareTag("Lake"))
        {
            suctionStrainer.SetConnectionToLake(false);
        }
    }

    #endregion

}
