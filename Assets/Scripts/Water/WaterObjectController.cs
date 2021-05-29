using UnityEngine;

/// <summary>
/// Abstract class for every Object that transports water
/// </summary>
public abstract class WaterObjectController : MonoBehaviour
{
    /// <summary> Sum of all Input Water Pressure </summary>
    public float InputWaterPressure { get; protected set; }

    /// <summary> Output Water Pressure per Output </summary>
    public float OutputWaterPressure { get; protected set; }

    /// <summary>
    /// Updates the Water Pressures of an Object
    /// </summary>
    protected abstract void UpdateWaterPressure();

}