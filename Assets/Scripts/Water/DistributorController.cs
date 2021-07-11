using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls a distributor
/// <author> Joe Koelbel </author>
/// </summary>
public class DistributorController : MovableParentWaterObject
{
    [Header("Connections")]

    /// <summary>
    /// The input connection
    /// </summary>
    [SerializeField, Tooltip("The input connection")]
    private ConnectionController inputConnection = null;

    /// <summary>
    /// Array of the output connections
    /// </summary>
    [SerializeField, Tooltip("Array of the output connections")]
    private ConnectionController[] outputConnections = new ConnectionController[3];

    [Header("Animation")]

    /// <summary>
    /// Array of the objects that are animated when opening or closing a connection
    /// </summary>
    [SerializeField, Tooltip("Array of the objects that are animated when opening or closing a connection")]
    private Transform[] outputOpener = new Transform[3];

    /// <summary>
    /// How long the animation takes
    /// </summary>
    [SerializeField, Tooltip("How long the animation takes")]
    private float openingClosingAnimationLength = 1.0f;

    /// <summary>
    /// array that shows which connections are open or closed
    /// </summary>
    private bool[] isOpenOutputConnection;

    /// <summary>
    /// array that shows which connections are currently opening or closing
    /// </summary>
    private bool[] isOpeningOrClosing;

    protected override void Start()
    {
        base.Start();

        // initialize the arrays
        int length = outputConnections.Length;
        isOpenOutputConnection = new bool[length];
        isOpeningOrClosing = new bool[length];
    }

    public override void UpdateWaterPressure()
    {
        // The input water pressure is the output water pressure of the input connection
        InputWaterPressure = inputConnection.OutputWaterPressure;
        OutputWaterPressure = InputWaterPressure;

        // The output water pressure is the input water pressure divided by the amount of open output connections
        List<int> openOutputConnectionIndices = new List<int>();
        for (int i = 0; i < isOpenOutputConnection.Length; i++)
        {
            if (isOpenOutputConnection[i])
            {
                openOutputConnectionIndices.Add(i);
            }
        }


        float outputPressurePerOpenedConnection = OutputWaterPressure / openOutputConnectionIndices.Count;

        // Update the water pressures of the output connections manually
        for (int i = 0; i < isOpenOutputConnection.Length; i++)
        {
            outputConnections[i].UpdateWaterPressure(isOpenOutputConnection[i] ? outputPressurePerOpenedConnection : 0.0f);
        }
    }

    public override void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        if (!isHose)
        {
            base.AdjustTransformOnConnection(currentConnectionTransform, targetTransform, fixedConnection, isHose);

            if (fixedConnection)
            {
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                foreach (ConnectionController outputConnection in outputConnections)
                {
                    outputConnection.Fixate();
                }

                inputConnection.Fixate();
            }
        }
    }

    public override void OnClearConnection(bool wasFixedConnection)
    {
        // Assert that only one connection can be connected to fixated connection
        if (wasFixedConnection)
        {
            setTransform = false;

            rigidBody.constraints = RigidbodyConstraints.None;

            foreach (ConnectionController outputConnection in outputConnections)
            {
                outputConnection.UnFixate();
            }

            inputConnection.UnFixate();
        }
    }

    /// <summary>
    /// Opens or closes the connection with the given index
    /// </summary>
    /// <param name="index"> index of the connection to be opened or closed </param>
    public void OnToggleConnection(int index)
    {
        // Only do something when that connection is not already opening or closing
        if (!isOpeningOrClosing[index])
        {
            isOpeningOrClosing[index] = true;
            isOpenOutputConnection[index] = !isOpenOutputConnection[index];

            UpdateWaterPressure();

            // Start the animation in a coroutine
            StartCoroutine(RotateOpener(index, isOpenOutputConnection[index]));
        }
    }

    /// <summary>
    /// Coroutine that handels the animation of opening and closing of connections
    /// </summary>
    /// <param name="index"> the index of the opening or closing connection </param>
    /// <param name="isOpening"> whether the connection is opening or closing </param>
    /// <returns></returns>
    private IEnumerator RotateOpener(int index, bool isOpening)
    {
        // Calculate how much the object should rotate per second
        float rotationPerSecond = (isOpening ? 90.0f : -90.0f) / openingClosingAnimationLength;

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

            // Rotate the object around its local y-axis
            outputOpener[index].Rotate(0.0f, rotationPerSecond * animationTime, 0.0f, Space.Self);

            // yield return null coroutines should be executed right after the normal update functions of MonoBehaviours
            yield return null;
        }

        // when the animation is finished, set the flag accordingly
        isOpeningOrClosing[index] = false;
    }
}
