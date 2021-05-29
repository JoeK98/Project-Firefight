using UnityEngine;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel </author>
/// </summary>
public class JetPipeController : WaterObjectController
{
    /// <summary>
    /// Jet pipe increases pressure (we use therefor a multiplier)
    /// </summary>
    private const float JET_PIPE_MULTIPLIER = 1.5f;

    /// <summary>
    /// The input connection of the jet pipe
    /// </summary>
    [SerializeField]
    private ConnectionController inputConnection = null;

    // TODO: REMOVE WHEN WATER PARTICLES ARE WORKING
    [SerializeField]
    private Renderer debugRenderer = null;
    protected override void Update()
    {
        UpdateWaterPressure();

        //TODO: Only for testing purpose:
        if (OutputWaterPressure > 0.5f)
        {
            debugRenderer.materials[2].color = Color.green;
        }
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }
}
