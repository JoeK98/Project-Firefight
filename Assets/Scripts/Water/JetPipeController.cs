using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel, Vincent Holtorf </author>
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

    [SerializeField]
    private ParticleSystem waterParticleSystem = null;

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }


    /// <summary>
    /// Callback for the Activated Event of the XR Interactable
    /// </summary>
    public void OnActivate()
    {
        //isActivated = true;
        if (OutputWaterPressure > 0.5f)
        {
            waterParticleSystem.Play();
        }
        else
        {
            waterParticleSystem.Stop();
        }
    }

    /// <summary>
    /// Callback for the Deactivated Event of the XR Interactable
    /// </summary>
    public void OnDeactivate()
    {
        //isActivated = false;
        waterParticleSystem.Stop();
    }
}
