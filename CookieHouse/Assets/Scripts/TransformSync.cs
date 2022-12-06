using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;

public class TransformSync : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnGrab))] 
    public int currentGrabber { get; set; }

    [Networked]
    private NetworkHand hand { get; set; }
    private bool isTakingAuthority = false;

    private static void OnGrab(Changed<TransformSync> changed)
    {
        changed.LoadNew();
        Debug.Log("change: " + changed.Behaviour.hand.name);

    }

    public async void setGrabber(SelectEnterEventArgs args)
    {
        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        if (auth)
        {
            GameObject obj = args.interactorObject.transform.gameObject;
            currentGrabber = obj.gameObject.GetInstanceID();
            RigPart part = obj.GetComponent<HardwareHand>().side;
            if(part == RigPart.LeftController)
            {
                hand = obj.transform.parent.GetComponent<HardwareRig>().transformBridge.lefthand;
            }
            else if(part == RigPart.RightController)
            {
                hand = obj.transform.parent.GetComponent<HardwareRig>().transformBridge.righthand;
            }
        }
    }

    public void releaseGrabber(SelectExitEventArgs args)
    { 
        currentGrabber = 0;
        hand = null;
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(currentGrabber!= 0)
        {
            Debug.Log($"fixed: {hand.transform.position}");
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }

    public override void Render()
    {
        base.Render();
        if (currentGrabber != 0)
        {
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }
}
