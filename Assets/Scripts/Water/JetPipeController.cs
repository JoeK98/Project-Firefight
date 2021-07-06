using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel, Vincent Holtorf </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class JetPipeController : MovableParentWaterObject
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

    [SerializeField]
    private Rigidbody rigidBody = null;

    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the JetPipe")]
    private AudioSource waterSound;

    private void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }

    }

    private void LateUpdate()
    {
        if (setTransform)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }

    public override void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        // Only do something when connecting to a fixated object
        // When not connecting to such an object, let the other object adjust
        if (fixedConnection)
        {
            base.AdjustTransformOnConnection(currentConnectionTransform, targetTransform, fixedConnection, isHose);

            rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            inputConnection.Fixate();
        }
    }



    /// <summary>
    /// Callback for the Activated Event of the XR Interactable
    /// </summary>
    public void OnActivate()
    {

        if (OutputWaterPressure > 0.5f)
        {
            waterParticleSystem.Play();
            waterSound.Play();
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
        waterParticleSystem.Stop();
        waterSound.Stop();
    }

    public override void OnClearConnection(bool wasFixedConnection)
    {
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            inputConnection.UnFixate();
        }
    }
}
