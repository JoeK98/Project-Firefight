using TMPro;
using UnityEngine;

/// <summary>
/// Controls a connection
/// <author> Joe Koelbel </author>
/// </summary>
public class ConnectionController : WaterObjectController
{

    //TODO: REMOVE DEBUG

    #region DEBUG

    [SerializeField]
    protected TextMeshProUGUI debugText = null;

    protected void DEBUG(string message)
    {
        if (debugText)
        {
            debugText.text = message;
        }
    }

    #endregion

    #region Serialized Fields

    /// <summary>
    /// Flag whether the water pressure is updated via the connected ConnectionController
    /// </summary>
    [SerializeField]
    protected bool waterPressureViaConnection = true;

    /// <summary>
    /// The size of the connection
    /// </summary>
    [SerializeField]
    protected HoseTypes connectionSize = HoseTypes.C;

    /// <summary>
    /// Flag whether the connection is movable (TODO: not implemented yet)
    /// </summary>
    [SerializeField]
    protected bool isMovable = false;

    [SerializeField]
    protected WaterObjectController parentObject = null;
    
    #endregion

    #region Protected Attributes

    /// <summary>
    /// The connected connection
    /// </summary>
    protected ConnectionController connectedObject = null;

    /// <summary>
    /// Flag whether the connection is being cleared
    /// Used to stop Recursive calls
    /// </summary>
    protected bool isClearing = false;

    /// <summary>
    /// Flag whether the connection is fixating
    /// Used to stop Recursive calls
    /// </summary>
    protected bool isFixating = false;

    /// <summary>
    /// Flag whether the connection is unfixating
    /// Used to stop Recursive calls
    /// </summary>
    protected bool isUnfixating = false;

    protected bool isConnectedToFixed = false;

    protected bool isFixated = false;

    protected Vector3 lastPosition;

    protected Quaternion lastRotation;

    protected int layer;

    protected bool isParentMovable = false;

    #endregion

    #region MonoBehaviour implementation

    private void Start()
    {
        layer = LayerMask.NameToLayer("Connection");

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        isParentMovable = parentObject && parentObject.GetType().IsSubclassOf(typeof(MovableParentWaterObject));
    }

    private void Update()
    {
        // Wenn nicht fixiert, verbunden mit anderer Connection und bewegt, dann bewege die andere Verbindung entsprechend
        if (!isFixated && connectedObject && (!lastPosition.Equals(transform.position) || !lastRotation.Equals(transform.rotation)))
        {
            connectedObject.MoveAccordingly(transform);
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        DEBUG(parentObject.name + ": " + name + ": " + InputWaterPressure.ToString());
    }

    /// <summary>
    /// Check if a connection can be build to a colliding trigger
    /// </summary>
    /// <param name="other"> the other collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (!connectedObject && other.gameObject.layer == layer)
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if ((!connection.connectedObject || connection.connectedObject == this) && connection.connectionSize == connectionSize)
            {
                connectedObject = connection;
                if (isMovable && !isFixated)
                {
                    isConnectedToFixed = !connection.isMovable || connection.isFixated;

                    if (isParentMovable)
                    {
                        ((MovableParentWaterObject)parentObject).AdjustTransformOnConnection(transform, other.transform, isConnectedToFixed, connectedObject.GetType() == typeof(HoseConnectionController));
                    }
                }
            }
        }
    }

    #endregion

    #region Abstract Class implementation

    public override void UpdateWaterPressure()
    {
        if (waterPressureViaConnection)
        {
            InputWaterPressure = connectedObject ? connectedObject.OutputWaterPressure : 0.0f;
            OutputWaterPressure = InputWaterPressure;

            parentObject.UpdateWaterPressure();
        }
    }

    #endregion

    protected virtual void MoveAccordingly(Transform other)
    {

    }

    #region Public Methods

    /// <summary>
    /// If the water pressure is not updated via the connection
    /// then it can be Updated manually
    /// </summary>
    /// <param name="waterPressure"></param>
    public void UpdateWaterPressure(float waterPressure)
    {
        if (!waterPressureViaConnection)
        {
            InputWaterPressure = waterPressure;
            OutputWaterPressure = waterPressure;

            if (connectedObject)
            {
                connectedObject.UpdateWaterPressure();
            }
        }
    }

    /// <summary>
    /// Disconnect two ConnectionControllers
    /// </summary>
    public virtual void OnClearConnection()
    {
        if (!isClearing && connectedObject)
        {
            isClearing = true;
            connectedObject.OnClearConnection();
            connectedObject = null;
            if (isParentMovable)
            {
                ((MovableParentWaterObject)parentObject).OnClearConnection(isConnectedToFixed);
            }
            isConnectedToFixed = false;

            UpdateWaterPressure();
        }
        isClearing = false;
    }

    /// <summary>
    /// Check whether two connections are compatible
    /// Needed for HoseConnectionControllers since C# does not allow access 
    /// to protected Members of an Instance of the base class (e.g. connectionSize) 
    /// </summary>
    /// <param name="connection"> the other ConnectionController </param>
    /// <returns> true if the connections are compatible else false </returns>
    public bool CheckOnTriggerEnter(ConnectionController connection)
    {
        return connection.connectionSize == connectionSize && (!connectedObject || connectedObject == connection);
    }

    public void Fixate()
    {
        if (!isFixating)
        {
            isFixating = true;
            if (connectedObject) // ? (No null propagation) should not be used for unity objects
            {
                connectedObject.Fixate();
            }
            isFixated = true;
        }
        isFixating = false;
    }

    public void UnFixate()
    {
        if (!isUnfixating)
        {
            isUnfixating = true;
            if (connectedObject) // ? (No null propagation) should not be used for unity objects
            {
                connectedObject.UnFixate();
            }
            isFixated = false;
        }
        isUnfixating = false;
    }

    #endregion

}
