using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;

public class TransformSync : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnGrab))] 
    public HardwareHand currentGrabber { get; set; }

    [Networked]
    private NetworkHand hand { get; set; }

    public Collider intercol;
    public Rigidbody rb;
    public bool useGravity;
    private bool isTakingAuthority = false;

    private static void OnGrab(Changed<TransformSync> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.currentGrabber != null)
        {
            changed.Behaviour.intercol.enabled = true;
            
        }
        else
        {
            changed.Behaviour.intercol.enabled = true;
            if (changed.Behaviour.useGravity)
            {
                changed.Behaviour.rb.isKinematic = false;
            }
        }
    }

    public async void setGrabber(SelectEnterEventArgs args)
    {

        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        if (auth)
        {
            GameObject obj = args.interactorObject.transform.gameObject;
            currentGrabber = obj.gameObject.GetComponent<HardwareHand>();
            Debug.Log(currentGrabber);
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
        currentGrabber = null;
        hand = null;
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(currentGrabber!= null)
        {
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }

    public override void Render()
    {
        base.Render();
        if (currentGrabber != null)
        {
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }
}
