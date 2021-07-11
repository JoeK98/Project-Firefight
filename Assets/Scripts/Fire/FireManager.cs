using UnityEngine;

/// <summary>
/// Class for activating the first Fire
/// <author> Vincent Holtorf </author>
/// </summary>
public class FireManager : MonoBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// Array of all the Fires in the scene
    /// </summary>
    [SerializeField, Tooltip("Array of all the Fires in the scene")] 
    private FireController[] fires;

    #endregion

    #region MonoBehavior implementation

    /// <summary>
    /// Activate random the first fire at the start of the level
    /// </summary>
    private void Start()
    {
        int windowIndex = Random.Range(0, fires.Length);
        fires[windowIndex].gameObject.SetActive(true);
    }

    #endregion

}
