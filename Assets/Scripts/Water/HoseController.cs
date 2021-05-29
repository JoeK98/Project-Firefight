using UnityEngine;


// TODO: REMOVE [ExecuteAlways] (Only neccessary for LineRenderer in Edit Mode)


/// <summary>
/// Class that controls a hose
/// <author> Joe Koelbel </author>
/// </summary>
[ExecuteAlways]
public class HoseController : WaterObjectController
{
    /// <summary>
    /// The size of the hose
    /// </summary>
    [SerializeField]
    private HoseTypes hoseType = HoseTypes.C;

    // TODO: Remove when Model is ready
    [SerializeField]
    private LineRenderer lineRenderer = null;
    [SerializeField]
    private Transform[] lineEnds = new Transform[2];

    /// <summary>
    /// The connections of the hose
    /// </summary>
    [SerializeField]
    private HoseConnectionController[] connections = new HoseConnectionController[2];

    /// <summary>
    /// The amount of water pressure that is lost in the hose
    /// </summary>
    private float pressureLoss = 0.0f;

    /// <summary>
    /// We only allow one connection to be the input connection
    /// and save its index here
    /// </summary>
    private int inputConnectionIndex = -1;

    /// <summary>
    /// Start is called before the first frame Update
    /// Set the pressureLoss according to the hose size
    /// </summary>
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

    // TODO: Remove when hose is not a line renderer anymore
    protected override void Update()
    {
        lineRenderer.SetPosition(0, lineEnds[0].position);
        lineRenderer.SetPosition(1, lineEnds[1].position);

        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        // if no connection is set as the input connection yet
        if (inputConnectionIndex == -1)
        {
            // Check if one of the connections gets water
            for (int i = 0; inputConnectionIndex == -1 && i < connections.Length; i++)
            {
                // if one of the connections gets water then set it as input connection
                // and tell the other that it will receive its water via the hose
                if (connections[i].OutputWaterPressure > 0.0f)
                {
                    inputConnectionIndex = i;
                    connections[(i + 1) % connections.Length].WaterPressureViaConnection = false;
                }
            }
        }
        // if a connection is already declared as the input connection
        else
        {
            // The input water pressure is the output water pressure of the input connection
            InputWaterPressure = connections[inputConnectionIndex].OutputWaterPressure;

            // The output water pressure is the input water pressure minus the pressure loss
            OutputWaterPressure = InputWaterPressure - pressureLoss;

            // Update the water pressure of the output connection manually
            connections[(inputConnectionIndex + 1) % connections.Length].UpdateWaterPressure(OutputWaterPressure);
        }
    }

}
