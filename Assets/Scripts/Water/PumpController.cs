using System.Collections.Generic;
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

    private bool[] isOpenOutputConnection;

    // TODO: Multiplier changeable by lever or buttons
    /// <summary>
    /// How much the pressure is increased in the pump
    /// </summary>
    private float pumpMultiplier = 3.0f;

    private void Start()
    {
        isOpenOutputConnection = new bool[outputConnections.Length];
    }

    protected override void UpdateWaterPressure()
    {
        // The input water pressure is the output water pressure of the input connection
        InputWaterPressure = inputConnection.OutputWaterPressure;
        OutputWaterPressure = InputWaterPressure * pumpMultiplier;

        // The output water pressure is the input water pressure multiplied by the pump multiplier and divided by the amount of open output connections
        List<int> openOutputConnectionIndices = new List<int>();
        for (int i = 0; i < isOpenOutputConnection.Length; i++)
        {
            if (isOpenOutputConnection[i])
            {
                openOutputConnectionIndices.Add(i);
            }
        }

        float outputPressurePerOpenedConnection = OutputWaterPressure / openOutputConnectionIndices.Count;

        // Update the water pressures of the output connections manually
        foreach (int outputIndex in openOutputConnectionIndices)
        {
            outputConnections[outputIndex].UpdateWaterPressure(outputPressurePerOpenedConnection);
        }
        /*foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }*/
    }

    public void OnToggleConnection(int index)
    {
        isOpenOutputConnection[index] = !isOpenOutputConnection[index];
    }
}
