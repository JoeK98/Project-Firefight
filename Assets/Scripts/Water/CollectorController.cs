using UnityEngine;

public class CollectorController : WaterObjectController
{
    [SerializeField]
    private ConnectionController[] inputConnections = new ConnectionController[2];

    [SerializeField]
    private ConnectionController outputConnection = null;


    private void Update()
    {
        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        float inputWaterPressure = 0.0f;
        foreach(ConnectionController input in inputConnections)
        {
            inputWaterPressure += input.OutputWaterPressure;
        }
        InputWaterPressure = inputWaterPressure;
        OutputWaterPressure = InputWaterPressure;

        outputConnection.UpdateWaterPressure(OutputWaterPressure);
    }
}
