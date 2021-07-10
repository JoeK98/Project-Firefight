using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel, Vincent Holtorf </author>
/// </summary>
public class JetPipeController : MovableParentWaterObject
{
    /// <summary>
    /// minimal water pressure to start the particle system
    /// </summary>
    private const float MIN_WATER_PRESSURE = 0.5f;

    /// <summary>
    /// The input connection of the jet pipe
    /// </summary>
    [SerializeField]
    private ConnectionController inputConnection = null;

    /// <summary>
    /// The animated lever that shows whether the pipe is opened or not
    /// </summary>
    [SerializeField]
    private Transform openerLever = null;

    /// <summary>
    /// How much time the opening or closing animation takes
    /// </summary>
    [SerializeField]
    private float openingClosingAnimationLength = 0.1f;

    /// <summary>
    /// The water particle system
    /// </summary>
    [SerializeField]
    private ParticleSystem waterParticleSystem = null;

    /// <summary>
    /// The connection for the Audiosound
    /// </summary>
    [SerializeField, Tooltip("Audiosource from the JetPipe")]
    private AudioSource waterSound;

    /// <summary>
    /// Queue for opening and closing animations
    /// </summary>
    private Queue<IEnumerator> animationQueue = new Queue<IEnumerator>();

    /// <summary>
    /// Flag whether an animation is currently running
    /// </summary>
    private bool isOpeningOrClosing = false;

    /// <summary>
    /// Flag whether the pipe is opened or not
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// Update is called once per frame
    /// if an animation is queued and none is already running -> start a new animation
    /// </summary>
    private void Update()
    {
        if (!isOpeningOrClosing && animationQueue.Count > 0)
        {
            StartCoroutine(animationQueue.Dequeue());
        }

        if (isActive && OutputWaterPressure > MIN_WATER_PRESSURE)
        {
            waterParticleSystem.Play();
            if (!waterSound.isPlaying)
            {
                waterSound.Play();
            }
        }
        else
        {
            waterParticleSystem.Stop();
            waterSound.Stop();
        }
    }

    public override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure;
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

    public override void OnClearConnection(bool wasFixedConnection)
    {
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            inputConnection.UnFixate();
        }
    }

    /// <summary>
    /// Callback for the Activated Event of the XR Interactable
    /// </summary>
    public void OnActivate()
    {
        isActive = true;

        animationQueue.Enqueue(RotateOpener(true));
    }

    /// <summary>
    /// Callback for the Deactivated Event of the XR Interactable
    /// </summary>
    public void OnDeactivate()
    {
        isActive = false;

        animationQueue.Enqueue(RotateOpener(false));
    }

    /// <summary>
    /// Coroutine that handels the animation of opening and closing of the pipe
    /// </summary>
    /// <param name="isOpening"> whether the pipe is opening or closing </param>
    /// <returns></returns>
    private IEnumerator RotateOpener(bool isOpening)
    {
        // Calculate how much the object should rotate per second
        float rotationPerSecond = (isOpening ? -90.0f : 90.0f) / openingClosingAnimationLength;

        // how much time passed since the animation started
        float wholeAnimationTime = 0.0f;

        // while the animation is not running as long as it should -> continue the animation
        while (wholeAnimationTime < openingClosingAnimationLength)
        {
            // time since the last frame update
            float animationTime = Time.deltaTime;
            wholeAnimationTime += animationTime;

            // if more time passed than the animation should run -> substract the extra time
            if (wholeAnimationTime > openingClosingAnimationLength)
            {
                animationTime -= wholeAnimationTime % openingClosingAnimationLength;
            }

            // Rotate the object around its local z-axis
            openerLever.Rotate(0.0f, 0.0f, rotationPerSecond * animationTime, Space.Self);

            // yield return null coroutines should be executed right after the normal update functions of MonoBehaviours
            yield return null;
        }

        // when the animation is finished, set the flag accordingly
        isOpeningOrClosing = false;
    }
}
