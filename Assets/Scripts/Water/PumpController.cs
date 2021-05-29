using UnityEngine;

/// <summary>
/// Controls a pump
/// <author> Joe Koelbel </author>
/// </summary>
public class PumpController : WaterObjectController
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
    private ConnectionController[] outputConnections = new ConnectionController[2];

    // TODO: Multiplier changeable by lever or buttons
    /// <summary>
    /// How much the pressure is increased in the pump
    /// </summary>
    private float pumpMultiplier = 3.0f;

    protected override void UpdateWaterPressure()
    {
        // The input water pressure is the output water pressure of the input connection
        InputWaterPressure = inputConnection.OutputWaterPressure;

        // The output water pressure is the input water pressure multiplied by the pump multiplier and divided by the amount of open output connections
        int openOutputs = 0;
        foreach (ConnectionController outputConnection in outputConnections)
        {
            if (outputConnection.IsOpen)
            {
                openOutputs++;
            }
        }

        OutputWaterPressure = openOutputs == 0 ? 0.0f : InputWaterPressure * pumpMultiplier / openOutputs;

        // Update the water pressures of the output connections manually
        foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }
    }
}
