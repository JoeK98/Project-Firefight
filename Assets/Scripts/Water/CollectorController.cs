using UnityEngine;

/// <summary>
/// Class the controls a collector
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CollectorController : MovableParentWaterObject
{
    /// <summary>
    /// Array including all input connections
    /// </summary>
    [SerializeField]
    private ConnectionController[] inputConnections = new ConnectionController[2];

    /// <summary>
    /// The output connection
    /// </summary>
    [SerializeField]
    private ConnectionController outputConnection = null;

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
        // Assert that only one connection can be connected to fixated connection
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            foreach (ConnectionController inputConnection in inputConnections)
            {
                inputConnection.UnFixate();
            }

            outputConnection.UnFixate();
        }
    }
}
