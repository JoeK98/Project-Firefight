using UnityEngine;

public class PumpController : WaterObjectController
{

    [SerializeField]
    private ConnectionController inputConnection = null;

    [SerializeField]
    private ConnectionController[] outputConnections = new ConnectionController[2];

    // TODO: Multiplier changeable by lever or buttons
    private float pumpMultiplier = 3.0f;

    private void Update()
    {
        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        int openOutputs = 0;
        foreach (ConnectionController outputConnection in outputConnections)
        {
            if (outputConnection.IsOpen)
            {
                openOutputs++;
            }
        }

        OutputWaterPressure = openOutputs == 0 ? 0.0f : InputWaterPressure * pumpMultiplier / openOutputs;

        foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }
    }
}
