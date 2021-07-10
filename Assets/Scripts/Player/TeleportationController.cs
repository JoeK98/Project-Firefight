using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class that controls the line of the teleportation process
/// <author> Joe Koelbel </author>
/// </summary>
public class TeleportationController : MonoBehaviour
{
    /// <summary>
    /// How much the stick must be moved to activate the line
    /// </summary>
    [Range(0.1f, 0.95f)]
    [SerializeField]
    private float deadzone = 0.5f;

    /// <summary>
    /// What kind of hand is it
    /// </summary>
    [SerializeField]
    private HandTypes handType = HandTypes.LEFT;

    /// <summary>
    /// The input device that controls that hand
    /// </summary>
    private InputDevice inputDevice;

    /// <summary>
    /// The Line
    /// </summary>
    private XRInteractorLineVisual lineVisual;

    /// <summary>
    /// Start is called before the first frame update
    /// Search for an fitting input device and the line visual
    /// </summary>
    private void Start()
    {
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HeldInHand
                                            | InputDeviceCharacteristics.Controller
                                            | (handType == HandTypes.LEFT ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);

        if (inputDevices.Count > 0)
        {
            inputDevice = inputDevices[0];
        }
        else
        {
            //TODO: Error when no input device was found
        }

        lineVisual = GetComponent<XRInteractorLineVisual>();
    }

    /// <summary>
    /// Update is called once per frame
    /// Checks whether the input is higher than the deadzone and shows the line accordingly
    /// </summary>
    private void Update()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisInput);

        lineVisual.enabled = axisInput.y > deadzone;
    }
}
