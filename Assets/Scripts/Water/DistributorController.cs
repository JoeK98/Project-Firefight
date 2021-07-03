using UnityEngine;

/// <summary>
/// Class that controls a distributor
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DistributorController : ParentWaterObject
{
    /// <summary>
    /// The input connection
    /// </summary>
    [SerializeField]
    private ConnectionController inputConnection = null;

    /// <summary>
    /// Array of the output connections
    /// </summary>
    [SerializeField]
    private ConnectionController[] outputConnections = new ConnectionController[3];

    [SerializeField]
    private Rigidbody rigidBody = null;

    private void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    private void LateUpdate()
    {
        if (setTransform)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    protected override void UpdateWaterPressure()
    {
        // The input water pressure is the output water pressure of the input connection
        InputWaterPressure = inputConnection.OutputWaterPressure;

        // The output water pressure is the input water pressure divided by the amount of open output connections
        int openOutputConnections = 0;
        foreach (ConnectionController outputConnection in outputConnections)
        {
            if (outputConnection.IsOpen)
            {
                openOutputConnections++;
            }
        }
        OutputWaterPressure = openOutputConnections == 0 ? 0.0f : InputWaterPressure / openOutputConnections;

        // Update the water pressures of the output connections manually
        foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }
    }

    public override void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        if (!isHose)
        {
            base.AdjustTransformOnConnection(currentConnectionTransform, targetTransform, fixedConnection, isHose);

            if (fixedConnection)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                foreach (ConnectionController outputConnection in outputConnections)
                {
                    outputConnection.Fixate();
                }

                inputConnection.Fixate();
            }
        }
    }

    public override void OnClearConnection(bool wasFixedConnection)
    {
        // Assert that only one connection can be connected to fixated connection
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            foreach (ConnectionController outputConnection in outputConnections)
            {
                outputConnection.UnFixate();
            }

            inputConnection.UnFixate();
        }
    }
}
