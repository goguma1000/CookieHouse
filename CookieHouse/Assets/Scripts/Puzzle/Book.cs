using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Book : NetworkBehaviour
{
    [Networked(OnChanged = nameof(setMatch))]
    public NetworkBool doMatch { get; set; }
    [SerializeField] private Vector3 shiftValsue;
    [SerializeField] private Transform pivotPosition;
    [SerializeField] private string colTag;

    private static void setMatch(Changed<Book> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.doMatch)
        {
            changed.Behaviour.gameObject.transform.SetParent(changed.Behaviour.pivotPosition);
            Vector3 temp = changed.Behaviour.shiftValsue * changed.Behaviour.pivotPosition.childCount;
            changed.Behaviour.gameObject.transform.localPosition = Vector3.zero + temp;
            changed.Behaviour.gameObject.transform.localRotation = Quaternion.identity;
            changed.Behaviour.gameObject.transform.GetComponent<XrOffsetGrabInteractable>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(colTag))
        {
            doMatch = true;
        }
    }
}
