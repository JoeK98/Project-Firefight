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
    
    #endregion

    #region Public Attributes

    /// <summary>
    /// Flag whether the connection is open
    /// </summary>
    public bool IsOpen { get; protected set; } = true;

    #endregion

    #region Protected Attributes

    /// <summary>
    /// The connected connection
    /// </summary>
    protected ConnectionController connectedObject = null;

    /// <summary>
    /// Flag whether the connection is being cleared (TODO: not implemented yet)
    /// </summary>
    protected bool isClearing = false;

    #endregion

    #region MonoBehaviour implementation

    //TODO: REMOVE DEBUG
    private void Start()
    {
        //DEBUG("Not Connected");
    }

    //TODO: REMOVE DEBUG
    protected override void Update()
    {
        UpdateWaterPressure();
        //DEBUG(transform.parent.name + ", " + gameObject.name + ": " + InputWaterPressure);//(connectedObject ? connectedObject.transform.parent.name + ", " + connectedObject.gameObject.name : "null"));
    }

    /// <summary>
    /// Check if a connection can be build to a colliding trigger
    /// </summary>
    /// <param name="other"> the other collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (connectedObject == null && other.CompareTag("Connection"))
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if ((connection.connectedObject == null || connection.connectedObject == this) && connection.connectionSize == connectionSize)
            {
                connectedObject = connection;
                if (isMovable)
                {    
                    transform.position = connectedObject.transform.position;
                }
                //DEBUG("Connected");
            }
        }
    }

    #endregion

    #region Abstract Class implementation

    protected override void UpdateWaterPressure()
    {
        if (waterPressureViaConnection && connectedObject != null)
        {
            InputWaterPressure = connectedObject.OutputWaterPressure;
            OutputWaterPressure = InputWaterPressure;
        }
    }

    #endregion

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
            //DEBUG(transform.parent.name + ", " + gameObject.name + ": " + waterPressure);
        }
    }

    /// <summary>
    /// Disconnect two ConnectionControllers
    /// </summary>
    public virtual void ClearConnection()
    {
        if (!isClearing && connectedObject != null)
        {
            isClearing = true;
            connectedObject.ClearConnection();
            connectedObject = null;
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
        return connection.connectionSize == connectionSize && (connectedObject == null || connectedObject == connection);
    }

    #endregion

}
