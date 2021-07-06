using System.Collections;
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

    [SerializeField]
    private Transform[] outputOpener = new Transform[2];

    [SerializeField]
    private float openingClosingAnimationLength = 1.0f;

    private bool[] isOpenOutputConnection;

    private bool[] isOpeningOrClosing;

    // TODO: Multiplier changeable by lever or buttons
    /// <summary>
    /// How much the pressure is increased in the pump
    /// </summary>
    private float pumpMultiplier = 3.0f;

    private void Start()
    {
        int length = outputConnections.Length;
        isOpenOutputConnection = new bool[length];
        isOpeningOrClosing = new bool[length];
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
    }

    public void OnToggleConnection(int index)
    {
        if (!isOpeningOrClosing[index])
        {
            isOpeningOrClosing[index] = true;
            isOpenOutputConnection[index] = !isOpenOutputConnection[index];
            StartCoroutine(RotateOpener(index, isOpenOutputConnection[index]));
        }
    }

    private IEnumerator RotateOpener(int index, bool isOpen)
    {
        float rotationPerSecond = (isOpen ? -360.0f : 360.0f) / openingClosingAnimationLength;

        float wholeAnimationTime = 0.0f;

        while (wholeAnimationTime < openingClosingAnimationLength)
        {
            float animationTime = Time.deltaTime;
            wholeAnimationTime += animationTime;

            if (wholeAnimationTime > openingClosingAnimationLength)
            {
                animationTime -= wholeAnimationTime % openingClosingAnimationLength;
            }
            outputOpener[index].Rotate(0.0f, 0.0f, rotationPerSecond * animationTime, Space.Self);

            yield return null;
        }

        isOpeningOrClosing[index] = false;
    }
}
