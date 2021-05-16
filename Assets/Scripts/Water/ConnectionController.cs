using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConnectionController : WaterObjectController
{

    #region DEBUG

    [SerializeField]
    private TextMeshProUGUI debugText = null;

    #endregion

    #region Serialized Fields

    [SerializeField]
    private bool waterPressureViaConnection = true;

    [SerializeField]
    private HoseTypes connectionSize = HoseTypes.C;

    #endregion

    #region Public Attributes

    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    public bool IsOpen { get; private set; } = true;

    #endregion

    #region Private Attributes

    private ConnectionController connectedObject = null;

    private bool isClearing = false;

    private Rigidbody rigidBody;

    #endregion

    #region MonoBehaviour implementation

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateWaterPressure();
        if (debugText)
        {
            debugText.text = "In: " + InputWaterPressure.ToString() + ", Out: " + OutputWaterPressure.ToString();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (connectedObject == null && collision.collider.CompareTag("Connection"))
        {
            ConnectionController connection = collision.collider.GetComponent<ConnectionController>();
            if (connection.connectedObject == null || connection.connectedObject == this)
            {
                connectedObject = connection.connectionSize == connectionSize ? connection : null;
                transform.position = connectedObject.transform.position;
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
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
        }
    }

    public void ClearConnection()
    {
        if (!isClearing && connectedObject != null)
        {
            isClearing = true;
            connectedObject.ClearConnection();
            connectedObject = null;
            rigidBody.constraints = RigidbodyConstraints.None;
        }
        isClearing = false;
    }

    #endregion

}
