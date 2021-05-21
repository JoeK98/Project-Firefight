using TMPro;
using UnityEngine;

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

    [SerializeField]
    protected bool waterPressureViaConnection = true;

    [SerializeField]
    protected HoseTypes connectionSize = HoseTypes.C;

    [SerializeField]
    protected bool isMovable = false;
    
    #endregion

    #region Public Attributes

    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    public bool IsOpen { get; protected set; } = true;

    #endregion

    #region Protected Attributes

    protected ConnectionController connectedObject = null;

    protected bool isClearing = false;

    #endregion

    #region MonoBehaviour implementation

    private void Start()
    {
        //DEBUG("Not Connected");
    }


    private void Update()
    {
        UpdateWaterPressure();
        DEBUG(transform.parent.name + ", " + gameObject.name + ": " + InputWaterPressure);//(connectedObject ? connectedObject.transform.parent.name + ", " + connectedObject.gameObject.name : "null"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (connectedObject == null && other.CompareTag("Connection"))
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.connectedObject == null || connection.connectedObject == this)
            {
                connectedObject = connection.connectionSize == connectionSize ? connection : null;
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

    public void UpdateWaterPressure(float waterPressure)
    {
        if (!waterPressureViaConnection)
        {
            InputWaterPressure = waterPressure;
            OutputWaterPressure = waterPressure;
            //DEBUG(transform.parent.name + ", " + gameObject.name + ": " + waterPressure);
        }
    }

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

    public bool CheckOnTriggerEnter(ConnectionController connection, HoseTypes connectionType)
    {
        return connectionSize == connectionType && (connectedObject == null || connectedObject == connection);
    }

    #endregion

}
