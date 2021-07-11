using UnityEngine;

/// <summary>
/// Class the controls a collector
/// <author> Joe Koelbel </author>
/// </summary>
public class CollectorController : MovableParentWaterObject
{

    #region Serialized Fields

    [Header("Connections")]

    /// <summary>
    /// Array including all input connections
    /// </summary>
    [SerializeField, Tooltip("Array including all input connections")]
    private ConnectionController[] inputConnections = new ConnectionController[2];

    /// <summary>
    /// The output connection
    /// </summary>
    [SerializeField, Tooltip("The output connection")]
    private ConnectionController outputConnection = null;

    #endregion

    #region Abstract Class implementation

    public override void UpdateWaterPressure()
    {
        // The water pressures are the sum of the pressures of all input connections
        float inputWaterPressure = 0.0f;
        foreach(ConnectionController input in inputConnections)
        {
            inputWaterPressure += input.OutputWaterPressure;
        }
        InputWaterPressure = inputWaterPressure;
        OutputWaterPressure = InputWaterPressure;

        // Manually Update the pressure of the output connection
        outputConnection.UpdateWaterPressure(OutputWaterPressure);
    }

    public override void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        // If connecting to a hose, let the hose do the transformation
        if (!isHose)
        {
            base.AdjustTransformOnConnection(currentConnectionTransform, targetTransform, fixedConnection, isHose);

            // If connecting to a fixated Object, fixate this Object and its connections
            if (fixedConnection)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                foreach (ConnectionController inputConnection in inputConnections)
                {
                    inputConnection.Fixate();
                }

                outputConnection.Fixate();
            }
        }
    }

    public override void OnClearConnection(bool wasFixedConnection)
    {
        // We assert that only one connection can be connected to a fixated connection
        if (wasFixedConnection)
        {
            // Dont move the object to its fixed position anymore
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            // Unfixate the connections
            foreach (ConnectionController inputConnection in inputConnections)
            {
                inputConnection.UnFixate();
            }

            outputConnection.UnFixate();
        }
    }

    #endregion

}
