using UnityEngine;

/// <summary>
/// Class that controls a hose
/// <author> Joe Koelbel </author>
/// </summary>
[ExecuteAlways] // to update the line renderer in edit and play mode
public class HoseController : WaterObjectController
{

    #region Serialized Fields

    /// <summary>
    /// The connections of the hose
    /// </summary>
    [SerializeField, Tooltip("The connections of the hose")]
    private HoseConnectionController[] connections = new HoseConnectionController[2];

    /// <summary>
    /// The size of the hose
    /// </summary>
    [SerializeField, Tooltip("The size of the hose")]
    private HoseTypes hoseType = HoseTypes.C;

    /// <summary>
    /// The line renderer
    /// </summary>
    [SerializeField, Tooltip("The line renderer")]
    private LineRenderer lineRenderer = null;

    /// <summary>
    /// The positions of the line ends
    /// </summary>
    [SerializeField, Tooltip("The positions of the line ends")]
    private Transform[] lineEnds = new Transform[2];

    #endregion

    #region Private Attributes

    /// <summary>
    /// The amount of water pressure that is lost in the hose
    /// </summary>
    private float pressureLoss = 0.0f;

    /// <summary>
    /// We only allow one connection to be the input connection
    /// and save its index here
    /// </summary>
    private int inputConnectionIndex = -1;

    #endregion

    #region MonoBehaviour implementation

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

    /// <summary>
    /// Update is called once per frame
    /// Updates the line renderer
    /// </summary>
    private void Update()
    {
        lineRenderer.SetPosition(0, lineEnds[0].position);
        lineRenderer.SetPosition(1, lineEnds[1].position);

        UpdateWaterPressure();
    }

    #endregion

    #region Abstract class implementation

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

    #endregion

}
