using UnityEngine;

public class SuctionBasketController : MovableParentWaterObject
{
    [SerializeField]
    private ConnectionController outputConnection = null;

    public void SetConnectionToLake(bool isConnected)
    {
        InputWaterPressure = isConnected ? 3.0f : 0.0f;
        OutputWaterPressure = InputWaterPressure;

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

                outputConnection.Fixate();
            }
        }
    }

    protected override void UpdateWaterPressure()
    {
        
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
