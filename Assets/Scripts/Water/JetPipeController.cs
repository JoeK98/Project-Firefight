using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

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
    private Transform openerLever = null;

    [SerializeField]
    private float openingClosingAnimationLength = 0.1f;

    [SerializeField]
    private ParticleSystem waterParticleSystem = null;

    [SerializeField]
    private Rigidbody rigidBody = null;

    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the JetPipe")]
    private AudioSource waterSound;

    private Queue<IEnumerator> rotationQueue = new Queue<IEnumerator>();

    private bool isOpeningOrClosing = false;

    private Quaternion initialRotationOfOpener;

    private void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        initialRotationOfOpener = openerLever.localRotation;
    }

    protected override void Update()
    {
        UpdateWaterPressure();

        if (!isOpeningOrClosing && rotationQueue.Count > 0)
        {
            StartCoroutine(rotationQueue.Dequeue());
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

        rotationQueue.Enqueue(RotateOpener(true));
    }

    /// <summary>
    /// Callback for the Deactivated Event of the XR Interactable
    /// </summary>
    public void OnDeactivate()
    {
        waterParticleSystem.Stop();
        waterSound.Stop();

        rotationQueue.Enqueue(RotateOpener(false));
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

    private IEnumerator RotateOpener(bool isOpen)
    {
        float rotationPerSecond = (isOpen ? -90.0f : 90.0f) / openingClosingAnimationLength;

        float wholeAnimationTime = 0.0f;

        while (wholeAnimationTime < openingClosingAnimationLength)
        {
            float animationTime = Time.deltaTime;
            wholeAnimationTime += animationTime;

            if (wholeAnimationTime > openingClosingAnimationLength)
            {
                animationTime -= wholeAnimationTime % openingClosingAnimationLength;
            }
            openerLever.Rotate(0.0f, rotationPerSecond * animationTime, 0.0f, Space.Self);

            yield return null;
        }

        // Reset the rotation to its initial value to fix small errors occuring during rotations
        /*if (!isOpen)
        {
            openerLever.localRotation = initialRotationOfOpener;
        }*/

        isOpeningOrClosing = false;
    }
}
