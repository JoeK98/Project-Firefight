using UnityEngine;

/// <summary>
/// Controls the connections of a hose
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HoseConnectionController : ConnectionController
{

    #region Serialized Fields

    /// <summary>
    /// A hose is currently only grabbable at its connections
    /// This means the connection needs a rigidbody
    /// </summary>
    [SerializeField, Tooltip("The Rigidbody of this connection (if not set, it will be retrieved in the Start method)")]
    private Rigidbody rigidBody = null;

    #endregion

    #region Public Attributes

    /// <summary>
    /// For Hoses the input and output connections arent initially defined
    /// which means that this value needs to be changed accordingly
    /// </summary>
    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    #endregion

    #region Private Attributes

    /// <summary>
    /// Target position when Updated in the LateUpdate method
    /// </summary>
    private Vector3 targetPosition;

    /// <summary>
    /// Target rotation when Updated in the LateUpdate method
    /// </summary>
    private Quaternion targetRotation;

    /// <summary>
    /// Flag whether the transform should be updated in the LateUpdate method
    /// </summary>
    private bool setTransform = false;

    private bool isFollowing = false;

    #endregion

    #region MonoBehaviour implementation

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

    private void Update()
    {
        if (isFollowing)
        {
            MoveAccordingly(ConnectedObject.transform);
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
        // if not already connected and the other object is on the same layer -> try to connect
        if (!ConnectedObject && other.gameObject.layer == gameObject.layer)
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.CheckOnTriggerEnter(this))
            {
                ConnectedObject = connection;

                if (ConnectedObject.GetType() == typeof(HoseConnectionController))
                {
                    HoseConnectionController connectedObjectAsHose = (HoseConnectionController)ConnectedObject;

                    if (!connectedObjectAsHose.isFollowing)
                    {
                        isFollowing = true;

                        setTransform = true;
                    }
                }
                else
                {
                    MoveAccordingly(ConnectedObject.transform);

                    setTransform = true;

                    rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                }

                UpdateWaterPressure();
            }
        }
    }

    #endregion

    #region Protected Methods

    protected override void MoveAccordingly(Transform other)
    {
        transform.position = other.position;
        transform.rotation = other.rotation;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    #endregion

    #region Public Methods 

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

    public void OnSelectEntered()
    {
        if (ConnectedObject && ConnectedObject.GetType() == typeof(HoseConnectionController))
        {
            ((HoseConnectionController)ConnectedObject).StartFollowing();
        }
    }

    private void StartFollowing()
    {
        if (ConnectedObject.GetType() == typeof(HoseConnectionController))
        {

            if (!isFollowing)
            {
                ((HoseConnectionController)ConnectedObject).StopFollowing();
            }
            isFollowing = true;

            setTransform = true;
        }
    }

    private void StopFollowing()
    {
        if (ConnectedObject.GetType() == typeof(HoseConnectionController))
        {

            isFollowing = false;

            setTransform = false;
        }
    }

    #endregion

}
