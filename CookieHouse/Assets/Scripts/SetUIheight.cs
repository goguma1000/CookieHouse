using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
public class SetUIheight : MonoBehaviour
{
    [SerializeField] private GameObject eye;
    private InputDevice targetDevice;
    void Start()
    {
        
    }

    void Update()
    {
        if(targetDevice == null || !targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, eye.transform.position.y, transform.position.z);
        }
    }

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, devices);
        if(devices.Count > 0)
        {
            targetDevice = devices[0];
            transform.position = new Vector3(transform.position.x, eye.transform.position.y, transform.position.z);
        }
    }
}
