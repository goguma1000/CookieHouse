using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Fire :NetworkBehaviour
{
    [Networked(OnChanged = nameof(EventOn))]
    private NetworkBool isEventOn { get; set; }

    public GameObject[] eventItems;
    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            bool auth = await Object.WaitForStateAuthority();
            isEventOn = true;
        }
    }

    private static void EventOn(Changed<Fire> changed) 
    {
        changed.LoadNew();
        if (changed.Behaviour.isEventOn)
        {
            for (int i = 0; i < changed.Behaviour.eventItems.Length; i++)
            {
                if (i < changed.Behaviour.eventItems.Length - 2) {
                    changed.Behaviour.eventItems[i].SetActive(true);
                }
                else
                {
                    changed.Behaviour.eventItems[i].GetComponent<XrOffsetGrabInteractable>().enabled = true;
                }
            }
            
        }
    }
}
