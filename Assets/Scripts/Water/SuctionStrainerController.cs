using UnityEngine;

/// <summary>
/// Class that controls the functionality of the suction strainer object
/// <author> Joe Koelbel </author>
/// </summary>
public class SuctionStrainerController : MovableParentWaterObject
{
    /// <summary>
    /// The output connection
    /// </summary>
    [SerializeField, Tooltip("The output connection")]
    private ConnectionController outputConnection = null;

    /// <summary>
    /// How much Pressure should the suction strainer have, when in the lake
    /// </summary>
    [SerializeField, Tooltip("How much Pressure should the suction strainer have, when in the lake")]
    private float connectedPressure = 3.0f;

    /// <summary>
    /// Flag whether the suction strainer is in the lake
    /// </summary>
    private bool isConnectedToLake = false;

    /// <summary>
    /// Sets the flag accordingly and updates the water pressure
    /// </summary>
    /// <param name="isConnected"> whether the suction strainer is in the lake </param>
    public void SetConnectionToLake(bool isConnected)
    {
        isConnectedToLake = isConnected;
        UpdateWaterPressure();
    }

    public override void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        // If connecting to a hose, let the hose do the transformation
        if (!isHose)
        {
            base.AdjustTransformOnConnection(currentConnectionTransform, targetTransform, fixedConnection, isHose);

            if (fixedConnection)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                outputConnection.Fixate();
            }
        }
    }

    public override void UpdateWaterPressure()
    {
        InputWaterPressure = isConnectedToLake ? connectedPressure : 0.0f;
        OutputWaterPressure = InputWaterPressure;

        outputConnection.UpdateWaterPressure(OutputWaterPressure);
    }

    public override void OnClearConnection(bool wasFixedConnection)
    {
        // Assert that only one connection can be connected to fixated connection
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            outputConnection.UnFixate();
        }
    }
}
