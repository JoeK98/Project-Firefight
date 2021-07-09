using UnityEngine;

public class SuctionBasketController : MovableParentWaterObject
{
    [SerializeField]
    private ConnectionController outputConnection = null;

    private bool isConnectedToLake = false;

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
        InputWaterPressure = isConnectedToLake ? 3.0f : 0.0f;
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
