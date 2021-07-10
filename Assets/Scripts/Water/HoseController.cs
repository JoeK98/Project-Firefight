using UnityEngine;

/// <summary>
/// Class that controls a hose
/// <author> Joe Koelbel </author>
/// </summary>
[ExecuteAlways] // to update the line renderer in edit and play mode
public class HoseController : WaterObjectController
{
    /// <summary>
    /// The size of the hose
    /// </summary>
    [SerializeField]
    private HoseTypes hoseType = HoseTypes.C;

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
            case HoseTypes.A:
                pressureLoss = 0.0f;
                break;
            case HoseTypes.B:
                pressureLoss = 0.2f;
                break;
            case HoseTypes.C:
                pressureLoss = 0.35f;
                break;
            default:
                Debug.LogError("This should never happen!");
                break;
        }
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, lineEnds[0].position);
        lineRenderer.SetPosition(1, lineEnds[1].position);

        UpdateWaterPressure();
    }

    public override void UpdateWaterPressure()
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
            OutputWaterPressure = Mathf.Clamp(InputWaterPressure - pressureLoss, 0.0f, float.MaxValue);

            // Update the water pressure of the output connection manually
            connections[(inputConnectionIndex + 1) % connections.Length].UpdateWaterPressure(OutputWaterPressure);

            // When the input connection was disconnected, the other could become the input connection -> Reset to initial state
            if (!connections[inputConnectionIndex].ConnectedObject)
            {
                connections[(inputConnectionIndex + 1) % connections.Length].WaterPressureViaConnection = true;
                inputConnectionIndex = -1;
            }
        }
    }

}
