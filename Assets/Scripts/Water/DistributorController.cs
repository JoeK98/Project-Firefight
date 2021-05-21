using UnityEngine;

public class DistributorController : WaterObjectController
{
    [SerializeField]
    private ConnectionController inputConnection = null;

    [SerializeField]
    private ConnectionController[] outputConnections = new ConnectionController[3];

    private void Update()
    {
        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        int openOutputConnections = 0;
        foreach (ConnectionController outputConnection in outputConnections)
        {
            if (outputConnection.IsOpen)
            {
                openOutputConnections++;
            }
        }
        OutputWaterPressure = openOutputConnections == 0 ? 0.0f : InputWaterPressure / openOutputConnections;

        foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }
    }
}
