using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
public class FirePlaceDoor : NetworkBehaviour
{
    [Networked(OnChanged = nameof(LoadData))]
    private int handCount { get; set; }
    public Collider eventCollider;
    public XRGrabInteractable grabinteractable;
    private void Update()
    {
       
    }

    private static void LoadData(Changed<FirePlaceDoor> changed)
    {
        changed.LoadNew();
        Debug.Log("Hand Count" + changed.Behaviour.handCount);
        if (changed.Behaviour.handCount >= 1)
        {
            changed.Behaviour.grabinteractable.enabled = true;
            changed.Behaviour.eventCollider.enabled = true;
        }
        else
        {
            changed.Behaviour.grabinteractable.enabled = false;
            changed.Behaviour.eventCollider.enabled = false;
        }
    }

    /*private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            bool auth = await Object.WaitForStateAuthority();
            handCount++;
        }
    }*/

    /*private async void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            bool auth = await Object.WaitForStateAuthority();
            handCount--;
        }
    }*/
    public async void  AddPlayerHandCount(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.CompareTag("Hand"))
        {
            bool auth = await Object.WaitForStateAuthority();
            handCount++;
        }
    }

    public async void MinusPlayerHandCount(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.CompareTag("Hand"))
        {
            bool auth = await Object.WaitForStateAuthority();
            handCount--;
        }
    }
}
