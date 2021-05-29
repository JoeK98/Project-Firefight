using UnityEngine;

/// <summary>
/// Class the controls a collector
/// <author> Joe Koelbel </author>
/// </summary>
public class CollectorController : WaterObjectController
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
}
