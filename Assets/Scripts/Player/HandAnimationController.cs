using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Animator))]
public class HandAnimationController : MonoBehaviour
{
    [SerializeField]
    private HandTypes handType;

    [SerializeField]
    private bool useAsFist = false;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float thumbMoveSpeed = 0.25f;

    private Animator animator;
    private InputDevice inputDevice;

    private float thumbValue;
    private float indexValue;
    private float threeFingersValue;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        inputDevice = GetInputDevice();
    }

    // Update is called once per frame
    private void Update()
    {
        AnimateHand();
    }

    private InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HeldInHand 
                                            | InputDeviceCharacteristics.Controller
                                            | (handType == HandTypes.Left ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);

        //TODO: Error when no input device was found

        return inputDevices[0];
    }

    private void AnimateHand()
    {

        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryTouched);

        thumbValue += (primaryTouched || secondaryTouched ? thumbMoveSpeed : -thumbMoveSpeed) * Time.deltaTime;
        thumbValue = Mathf.Clamp01(thumbValue);

        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out indexValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out threeFingersValue);

        if (useAsFist)
        {
            float maxValue = Mathf.Max(thumbValue, indexValue, threeFingersValue);

            animator.SetFloat("Thumb", maxValue);
            animator.SetFloat("Index", maxValue);
            animator.SetFloat("ThreeFingers", maxValue);
        }
        else
        {
            animator.SetFloat("Thumb", thumbValue);
            animator.SetFloat("Index", indexValue);
            animator.SetFloat("ThreeFingers", threeFingersValue);
        }
    }
}
