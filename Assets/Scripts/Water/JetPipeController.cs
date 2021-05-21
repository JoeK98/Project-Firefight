using UnityEngine;

public class JetPipeController : WaterObjectController
{
    private const float JET_PIPE_MULTIPLIER = 1.5f;

    [SerializeField]
    private ConnectionController inputConnection = null;

    // Update is called once per frame
    void Update()
    {
        UpdateWaterPressure();

        //TODO: Only for testing purpose:
        if (OutputWaterPressure > 0.5f)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }
}
