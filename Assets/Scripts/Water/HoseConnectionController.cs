using UnityEngine;

/// <summary>
/// Controls the connections of a hose
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HoseConnectionController : ConnectionController
{
    /// <summary>
    /// For Hoses the input and output connections arent initially defined
    /// which means that this value needs to be changed accordingly
    /// </summary>
    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    /// <summary>
    /// A hose is currently only grabbable at its connections (TODO: check later if this is still true)
    /// This means the connection needs a rigidbody
    /// </summary>
    private Rigidbody rigidBody = null;

    private Vector3 targetPosition;

    private Quaternion targetRotation;

    private bool setTransform = false;

    /// <summary>
    /// Start is called before the first frame update
    /// Search for the Rigidbody
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        layer = LayerMask.NameToLayer("Connection");
    }

    private void Update()
    {
        DEBUG(parentObject.name + ": " + name + ": " + InputWaterPressure.ToString());
    }

    private void LateUpdate()
    {
        if (setTransform)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    /// <summary>
    /// Check if a connection can be build with the colliding object
    /// </summary>
    /// <param name="other"> the other collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (!connectedObject && other.gameObject.layer == layer)
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.CheckOnTriggerEnter(this))
            {
                connectedObject = connection;

                setTransform = true;

                MoveAccordingly(connectedObject.transform);

                rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                UpdateWaterPressure();
            }
        }
    }

    protected override void MoveAccordingly(Transform other)
    {
        transform.position = other.position;
        transform.rotation = other.rotation;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    public override void OnClearConnection()
    {
        if (!isClearing && connectedObject)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
            isClearing = true;
            connectedObject.OnClearConnection();
            connectedObject = null;
            setTransform = false;
            UpdateWaterPressure();
            parentObject.UpdateWaterPressure();
        }
        isClearing = false;
    }

}
