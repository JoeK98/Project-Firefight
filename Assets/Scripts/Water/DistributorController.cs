using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls a distributor
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DistributorController : MovableParentWaterObject
{
    /// <summary>
    /// The input connection
    /// </summary>
    [SerializeField]
    private ConnectionController inputConnection = null;

    /// <summary>
    /// Array of the output connections
    /// </summary>
    [SerializeField]
    private ConnectionController[] outputConnections = new ConnectionController[3];

    [SerializeField]
    private Transform[] outputOpener = new Transform[3];

    [SerializeField]
    private float openingClosingAnimationLength = 1.0f;

    [SerializeField]
    private Rigidbody rigidBody = null;

    private bool[] isOpenOutputConnection;

    private bool[] isOpeningOrClosing;

    private void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        int length = outputConnections.Length;
        isOpenOutputConnection = new bool[length];
        isOpeningOrClosing = new bool[length];
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
        foreach (int outputIndex in openOutputConnectionIndices)
        {
            outputConnections[outputIndex].UpdateWaterPressure(outputPressurePerOpenedConnection);
        }
        /*foreach (ConnectionController outputConnection in outputConnections)
        {
            outputConnection.UpdateWaterPressure(OutputWaterPressure);
        }*/
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

    public void OnToggleConnection(int index)
    {
        if (!isOpeningOrClosing[index])
        {
            isOpeningOrClosing[index] = true;
            isOpenOutputConnection[index] = !isOpenOutputConnection[index];
            StartCoroutine(RotateOpener(index, isOpenOutputConnection[index]));
        }
    }

    /*private IEnumerator RotateOpener(int index, bool isOpen, Vector3 upVector)
    {
        float time = 0.0f;

        Quaternion start = outputOpener[index].rotation;
        Quaternion target = Quaternion.AngleAxis(isOpen ? 90.0f : -90.0f, upVector) * start;

        do
        {
            time += Time.deltaTime / openingClosingAnimationLength;

            outputOpener[index].rotation = Quaternion.Lerp(start, target, time);

            yield return null;
        }
        while (time < 1.0f);

        isOpeningOrClosing[index] = false;
    }*/

    private IEnumerator RotateOpener(int index, bool isOpen)
    {
        float rotationPerSecond = (isOpen ? 90.0f : -90.0f) / openingClosingAnimationLength;

        float wholeAnimationTime = 0.0f;

        while (wholeAnimationTime < openingClosingAnimationLength)
        {
            float animationTime = Time.deltaTime;
            wholeAnimationTime += animationTime;

            if (wholeAnimationTime > openingClosingAnimationLength)
            {
                animationTime -= wholeAnimationTime % openingClosingAnimationLength;
            }
            outputOpener[index].Rotate(0.0f, rotationPerSecond * animationTime, 0.0f, Space.Self);

            yield return null;
        }

        isOpeningOrClosing[index] = false;
    }
}
