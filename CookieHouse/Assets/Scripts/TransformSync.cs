using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit;

[OrderAfter(typeof(Piece))]
public class TransformSync : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnGrab))] 
    public int currentGrabber { get; set; }

    [Networked]
    private NetworkHand hand { get; set; }
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    public Collider intercol;
    public Rigidbody rb;
    public bool useGravity;
    private bool isTakingAuthority = false;

    private void Awake()
    {
        defaultPosition = this.gameObject.transform.position;
        defaultRotation = this.gameObject.transform.rotation;
    }
    private static void OnGrab(Changed<TransformSync> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.currentGrabber != 0)
        {
            changed.Behaviour.intercol.enabled = false;
            if (changed.Behaviour.useGravity)
            {
                changed.Behaviour.rb.useGravity = false;
            }
            
        }
        else
        {
            changed.Behaviour.intercol.enabled = true;
            if (changed.Behaviour.useGravity)
            {
                changed.Behaviour.rb.useGravity = true;
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
            currentGrabber = obj.gameObject.GetComponent<HardwareHand>().GetInstanceID();
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

    public async void releaseGrabber(SelectExitEventArgs args)
    {
        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        hand = null;
        currentGrabber = 0;
        
    }
    
    public async void getStateAuth()
    {
        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
    }

    public void ResetPosion()
    {
        this.gameObject.transform.position = defaultPosition;
        this.gameObject.transform.rotation = defaultRotation;
    }
    /*public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if(currentGrabber!= 0 && useGravity)
        {
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }*/

    /*public override void Render()
    {
        base.Render();
        if (currentGrabber != 0 && useGravity)
        {
            transform.position = hand.gameObject.transform.position;
            transform.rotation = hand.gameObject.transform.rotation;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayableArea") && intercol.enabled)
        {
            this.gameObject.transform.position = defaultPosition;
            this.gameObject.transform.rotation = defaultRotation;
        }
    }
   
}
