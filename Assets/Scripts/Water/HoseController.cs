using UnityEngine;

[ExecuteAlways]
public class HoseController : WaterObjectController
{
    [SerializeField]
    private HoseTypes hoseType = HoseTypes.C;

    //TODO: Remove when Model is ready
    [SerializeField]
    private LineRenderer lineRenderer = null;

    [SerializeField]
    private Transform[] lineEnds = new Transform[2];

    private float pressureLoss = 0.0f;

    [SerializeField]
    private HoseConnectionController[] connections = new HoseConnectionController[2];

    private int inputConnectionIndex = -1;

    private void Start()
    {
        switch (hoseType)
        {
            case HoseTypes.B:
                pressureLoss = 0.2f;
                break;
            case HoseTypes.C:
                pressureLoss = 0.35f;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, lineEnds[0].position);
        lineRenderer.SetPosition(1, lineEnds[1].position);

        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        if (inputConnectionIndex == -1)
        {
            for (int i = 0; inputConnectionIndex == -1 && i < connections.Length; i++)
            {
                if (connections[i].OutputWaterPressure > 0.0f)
                {
                    inputConnectionIndex = i;
                    connections[(i + 1) % connections.Length].WaterPressureViaConnection = false;
                }
            }
        }
        else
        {
            InputWaterPressure = connections[inputConnectionIndex].OutputWaterPressure;
            OutputWaterPressure = InputWaterPressure - pressureLoss;

            connections[(inputConnectionIndex + 1) % connections.Length].UpdateWaterPressure(OutputWaterPressure);
        }
    }

}
