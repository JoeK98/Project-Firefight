using UnityEngine;

/// <summary>
/// Abstract class for every Object that transports water
/// <author> Joe Koelbel </author>
/// </summary>
public abstract class WaterObjectController : MonoBehaviour
{

    #region Public Attributes

    /// <summary> Sum of all Input Water Pressure </summary>
    public float InputWaterPressure { get; protected set; }

    /// <summary> Output Water Pressure per Output </summary>
    public float OutputWaterPressure { get; protected set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Updates the Water Pressures of an Object
    /// </summary>
    public abstract void UpdateWaterPressure();

    #endregion

}
