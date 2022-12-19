using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class HardwareHand : MonoBehaviour
{

    public RigPart side;
    private InputDevice targetDevice;
    [SerializeField]
    private InputDeviceCharacteristics controllerCharacter;
    private float grabBtnValue;

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacter, devices);
        if(devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    private void Update()
    {
        if(targetDevice ==null || !targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            targetDevice.TryGetFeatureValue(CommonUsages.grip, out grabBtnValue);
        }
    }

    public float getGripValue()
    {
        return grabBtnValue;
    }
}
