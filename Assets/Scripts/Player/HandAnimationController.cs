using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Class that controls the animations of the hands
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandAnimationController : MonoBehaviour
{
    /// <summary>
    /// Is it a Left or a Right hand
    /// </summary>
    [SerializeField]
    private HandTypes handType;

    /// <summary>
    /// Flag whether every button should curl all fingers
    /// </summary>
    [SerializeField]
    private bool useAsFist = false;

    /// <summary>
    /// How fast the Thumb curls when A/B/X/Y is pressed since those are no triggers
    /// </summary>
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float thumbMoveSpeed = 0.25f;

    /// <summary>
    /// The animator of the hand
    /// </summary>
    private Animator animator;

    /// <summary>
    /// The device that controls the hand
    /// </summary>
    private InputDevice inputDevice;

    /// <summary>
    /// Current curl value of the thumb (0.0 - 1.0)
    /// </summary>
    private float thumbValue;

    /// <summary>
    /// Current curl value of the index finger (0.0 - 1.0)
    /// </summary>
    private float indexValue;

    /// <summary>
    /// Current curl value of the other fingers (0.0 - 1.0)
    /// </summary>
    private float threeFingersValue;

    /// <summary>
    /// Start is called before the first frame update
    /// Searches for the animator and checks if an input device is connected
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        inputDevice = GetInputDevice();
    }

    /// <summary>
    /// Update is called once per frame
    /// Animating the hand
    /// </summary>
    private void Update()
    {
        AnimateHand();
    }

    /// <summary>
    /// Get the desired input device based on the hand type
    /// </summary>
    /// <returns> The desired input device based on the hand type </returns>
    private InputDevice GetInputDevice()
    {
        // Define the kind of input device
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HeldInHand 
                                            | InputDeviceCharacteristics.Controller
                                            | (handType == HandTypes.LEFT ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

        // Get a List of input devices that fit the characteristics
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);

        //TODO: Error when no input device was found

        // Return the first fitting input device
        return inputDevices[0];
    }

    /// <summary>
    /// Animating the hand
    /// </summary>
    private void AnimateHand()
    {
        // Check if A/B/X/Y is pressed
        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryTouched);

        // If any of A/B/X/Y is pressed then increase the thumbValue else decrease it
        thumbValue += (primaryTouched || secondaryTouched ? thumbMoveSpeed : -thumbMoveSpeed) * Time.deltaTime;
        thumbValue = Mathf.Clamp01(thumbValue);

        // Get the values of the triggers for the other fingers
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out indexValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out threeFingersValue);

        // if the hand should be handled like a fist then the max value is applied to all fingers
        if (useAsFist)
        {
            float maxValue = Mathf.Max(thumbValue, indexValue, threeFingersValue);

            animator.SetFloat("Thumb", maxValue);
            animator.SetFloat("Index", maxValue);
            animator.SetFloat("ThreeFingers", maxValue);
        }
        // if the hand should not be handled like a fist then apply the values accordingly
        else
        {
            animator.SetFloat("Thumb", thumbValue);
            animator.SetFloat("Index", indexValue);
            animator.SetFloat("ThreeFingers", threeFingersValue);
        }
    }
}
