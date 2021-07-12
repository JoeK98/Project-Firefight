using UnityEngine;

/// <summary>
/// Class that Activates an GameObject
/// <author> Vincent Holtorf, Joe Koelbel </author>
/// </summary>
public class SetObjectActive : MonoBehaviour
{
    /// <summary>
    /// The object that should be activated
    /// </summary>
    [SerializeField, Tooltip ("Put here the field for the task")]
    private GameObject obj;

    /// <summary>
    /// OnEnable is called when a GameObject becomes active
    /// sets the gameobject active
    /// </summary>
    private void OnEnable()
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// OnEnable is called when a GameObject becomes inactive
    /// sets the gameobject inactive
    /// </summary>
    private void OnDisable()
    {
        obj.SetActive(false);
    }



}
