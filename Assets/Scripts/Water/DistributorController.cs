using UnityEngine;

/// <summary>
/// Class that controls a distributor
/// <author> Joe Koelbel </author>
/// </summary>
public class DistributorController : WaterObjectController
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
}
