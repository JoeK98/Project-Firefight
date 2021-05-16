using UnityEngine;

public class HydrantController : WaterObjectController
{

    #region Constants

    private const float WATER_PRESSURE = 3.0f;

    #endregion

    #region Serialized Fields

    [SerializeField]
    private ConnectionController connection = null;

    #endregion

    #region Private Attributes

    private bool isOpen = true;//false;

    #endregion

    #region MonoBehaviour implementation

    private void Start()
    {
        InputWaterPressure = WATER_PRESSURE;
    }

    private void Update()
    {
        UpdateWaterPressure();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("HydrantKey"))
        {
            isOpen = !isOpen;
        }
    }

    #endregion

    #region Abstract Class implementation

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = WATER_PRESSURE;
        if (isOpen)
        {
            OutputWaterPressure = InputWaterPressure;
        }
        else
        {
            OutputWaterPressure = 0.0f;
        }

        connection.UpdateWaterPressure(OutputWaterPressure);
    }

    #endregion

}
