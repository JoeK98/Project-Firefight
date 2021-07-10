using UnityEngine;

/// <summary>
/// Controls the connections of a hose
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HoseConnectionController : ConnectionController
{
    /// <summary>
    /// A hose is currently only grabbable at its connections
    /// This means the connection needs a rigidbody
    /// </summary>
    [SerializeField]
    private Rigidbody rigidBody = null;

    /// <summary>
    /// For Hoses the input and output connections arent initially defined
    /// which means that this value needs to be changed accordingly
    /// </summary>
    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    private Vector3 targetPosition;

    private Quaternion targetRotation;

    private bool setTransform = false;

    /// <summary>
    /// Start is called before the first frame update
    /// Search for the Rigidbody when it is not assigned in the editor
    /// </summary>
    private void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Update the position of the connection when connected to an object
    /// </summary>
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
        if (!ConnectedObject && other.gameObject.layer == gameObject.layer)
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.CheckOnTriggerEnter(this))
            {
                ConnectedObject = connection;

                setTransform = true;

                MoveAccordingly(ConnectedObject.transform);

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
        if (!isClearing && ConnectedObject)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
            isClearing = true;
            ConnectedObject.OnClearConnection();
            ConnectedObject = null;
            setTransform = false;
            UpdateWaterPressure();
            parentObject.UpdateWaterPressure();
        }
        isClearing = false;
    }

}
