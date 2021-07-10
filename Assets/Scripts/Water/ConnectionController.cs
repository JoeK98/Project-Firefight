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

    /// <summary>
    /// The parent object of the connection
    /// </summary>
    [SerializeField]
    protected WaterObjectController parentObject = null;
    


    /// <summary>
    /// The connected connection
    /// </summary>
    public ConnectionController ConnectedObject { get; protected set; } = null;

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

    /// <summary>
    /// Flag whether this connection is connected to a fixated object
    /// </summary>
    protected bool isConnectedToFixated = false;

    /// <summary>
    /// Flag whether this connection is fixated
    /// </summary>
    protected bool isFixated = false;

    /// <summary>
    /// The position of this connection on the last frame
    /// </summary>
    protected Vector3 lastPosition;

    /// <summary>
    /// The rotation of this connection on the last frame
    /// </summary>
    protected Quaternion lastRotation;

    /// <summary>
    /// Flag whether the parent object is movable
    /// </summary>
    protected bool isParentMovable = false;

    /// <summary>
    /// Start is called before the first frame update
    /// Sets initial values to some variables
    /// </summary>
    private void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;

        isParentMovable = parentObject && parentObject.GetType().IsSubclassOf(typeof(MovableParentWaterObject));
    }

    /// <summary>
    /// Update is called once per frame
    /// Updates the position of the connection and the connected objects
    /// </summary>
    private void Update()
    {
        // If not fixated, connected to another connection and is at a different location than last frame -> Move the connected object accordingly
        if (!isFixated && ConnectedObject && (!lastPosition.Equals(transform.position) || !lastRotation.Equals(transform.rotation)))
        {
            ConnectedObject.MoveAccordingly(transform);
        }

        // Save position and rotation for next frame
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    /// <summary>
    /// Check if a connection can be build to a colliding trigger
    /// </summary>
    /// <param name="other"> the other collider </param>
    private void OnTriggerEnter(Collider other)
    {
        // If not already connected and if the other object is on the same layer
        if (!ConnectedObject && other.gameObject.layer == gameObject.layer)
        {
            // Try to get the other ConnectionController
            ConnectionController connection = other.GetComponent<ConnectionController>();

            // The other object must have no connection or must be connected to this (since the execution of one OnTriggerEnter happens before the other) and the connections must be of the same size
            if ((!connection.ConnectedObject || connection.ConnectedObject == this) && connection.connectionSize == connectionSize)
            {
                // then the other object can be assigned to the connectedObject variable
                ConnectedObject = connection;

                // if this connection is movable and not fixated
                if (isMovable && !isFixated)
                {
                    // check if the other connection is a fixated connection
                    isConnectedToFixated = !connection.isMovable || connection.isFixated;

                    // if the parent object is a movable object -> adjust its transform to have the connections fit to another
                    if (isParentMovable)
                    {
                        ((MovableParentWaterObject)parentObject).AdjustTransformOnConnection(transform, other.transform, isConnectedToFixated, ConnectedObject.GetType() == typeof(HoseConnectionController));
                    }
                }
            }
        }
    }

    protected virtual void MoveAccordingly(Transform other)
    {
        // TODO
    }

    public override void UpdateWaterPressure()
    {
        // the water pressure can be assigned via the connectedObject or the parent object
        // this method is used when the pressure is updated via the connectedObject
        if (waterPressureViaConnection)
        {
            // the InputWaterPressure is either the OutputWaterPressure of the connectedObject or 0
            InputWaterPressure = ConnectedObject ? ConnectedObject.OutputWaterPressure : 0.0f;
            OutputWaterPressure = InputWaterPressure;

            // Update the water pressure of the parent object
            parentObject.UpdateWaterPressure();
        }
    }

    /// <summary>
    /// If the water pressure is not updated via the connection
    /// then it can be Updated manually
    /// </summary>
    /// <param name="waterPressure"> the water pressure given by the parent object </param>
    public void UpdateWaterPressure(float waterPressure)
    {
        // the water pressure can be assigned via the connectedObject or the parent object
        // this method is used when the pressure is updated via the parent object
        if (!waterPressureViaConnection)
        {
            // the water pressure is given by the parent object in the method call
            InputWaterPressure = waterPressure;
            OutputWaterPressure = waterPressure;

            // if this connection is connected to another object -> update its water pressure
            if (ConnectedObject)
            {
                ConnectedObject.UpdateWaterPressure();
            }
        }
    }

    /// <summary>
    /// Disconnect two ConnectionControllers
    /// </summary>
    public virtual void OnClearConnection()
    {
        // Only do something when not already clearing and still being conected to an object
        if (!isClearing && ConnectedObject)
        {
            // set the flag to stop the recursion to true
            isClearing = true;

            // call the method on the connected object
            ConnectedObject.OnClearConnection();

            // remove the connected object
            ConnectedObject = null;

            // if the parent object is movable -> call its OnClearConnection method to maybe unfixate the object
            if (isParentMovable)
            {
                ((MovableParentWaterObject)parentObject).OnClearConnection(isConnectedToFixated);
            }
            isConnectedToFixated = false;

            UpdateWaterPressure();
        }
        // set the flag to stop the recursion to false after everything is done
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
        return connection.connectionSize == connectionSize && (!ConnectedObject || ConnectedObject == connection);
    }

    /// <summary>
    /// fixate this object and the connected object
    /// </summary>
    public void Fixate()
    {
        if (!isFixating)
        {
            isFixating = true;
            if (ConnectedObject) // ? (No null propagation) should not be used for unity objects
            {
                ConnectedObject.Fixate();
            }
            isFixated = true;
        }
        isFixating = false;
    }

    /// <summary>
    /// unfixate this object and the connected objet
    /// </summary>
    public void UnFixate()
    {
        if (!isUnfixating)
        {
            isUnfixating = true;
            if (ConnectedObject) // ? (No null propagation) should not be used for unity objects
            {
                ConnectedObject.UnFixate();
            }
            isFixated = false;
        }
        isUnfixating = false;
    }
}
