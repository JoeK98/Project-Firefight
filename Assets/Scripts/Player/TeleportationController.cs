using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{

    [Range(0.1f, 0.95f)]
    [SerializeField]
    private float deadzone = 0.5f;

    [SerializeField]
    private HandTypes handType = HandTypes.Left;

    private InputDevice inputDevice;

    private XRInteractorLineVisual lineVisual;

    private bool previouslyEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HeldInHand
                                            | InputDeviceCharacteristics.Controller
                                            | (handType == HandTypes.Left ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

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

    // Update is called once per frame
    void Update()
    {
        Vector2 axisInput;
        inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);

        lineVisual.enabled = axisInput.y > deadzone;

        if (axisInput.y <= deadzone && previouslyEnabled)
        {
            lineVisual.enabled = false;
        }

        previouslyEnabled = lineVisual.enabled;
    }
}
