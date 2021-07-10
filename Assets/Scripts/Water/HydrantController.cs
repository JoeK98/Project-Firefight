using UnityEngine;

/// <summary>
/// Class that controls a hydrant
/// <author> Joe Koelbel </author>
/// </summary>
public class HydrantController : WaterObjectController
{

    #region Constants

    /// <summary>
    /// The water pressure that the hydrant provides
    /// </summary>
    private const float WATER_PRESSURE = 3.0f;

    #endregion

    #region Serialized Fields

    /// <summary>
    /// The output connection of the hydrant
    /// </summary>
    [SerializeField]
    private ConnectionController connection = null;

    #endregion

    #region Private Attributes

    /// <summary>
    /// Flag whether the hydrant is open or not
    /// </summary>
    private bool isOpen = false;

    #endregion

    #region MonoBehaviour implementation

    /// <summary>
    /// Start is called before the first frame Update
    /// Set the InputWaterPressure to a constant value
    /// since we are not simulating a citys water system
    /// </summary>
    private void Start()
    {
        InputWaterPressure = WATER_PRESSURE;
    }

    /// <summary>
    /// Close / Open the hydrant when touched with a hydrant key
    /// </summary>
    /// <param name="other"> collider that entered the trigger </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HydrantKey"))
        {
            isOpen = !isOpen;
            UpdateWaterPressure();
        }
    }

    #endregion

    #region Abstract Class implementation

    public override void UpdateWaterPressure()
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
