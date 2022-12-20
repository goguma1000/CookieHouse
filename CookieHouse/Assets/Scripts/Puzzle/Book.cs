using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Book : NetworkBehaviour
{
    [Networked(OnChanged = nameof(setMatch))]
    public NetworkBool doMatch { get; set; }
    public bool xshift = false;
    public bool yshift = false;    
    public bool zshift = false;    
    [SerializeField] private Vector3 shiftValsue;
    [SerializeField] private Vector3 rotateValsue;
    [SerializeField] private Transform pivotPosition;
    [SerializeField] private string colTag;
    [SerializeField] private Collider interCol;
    private bool isTakingAuthority = false;
    public AudioSource audio;
    private static void setMatch(Changed<Book> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.doMatch)
        {
            Vector3 temp = Vector3.zero;
            changed.Behaviour.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            changed.Behaviour.interCol.enabled = false;
            changed.Behaviour.gameObject.transform.SetParent(changed.Behaviour.pivotPosition);
            if (changed.Behaviour.xshift) {
               temp = new Vector3(changed.Behaviour.shiftValsue.x *(changed.Behaviour.pivotPosition.childCount -1), changed.Behaviour.shiftValsue.y, changed.Behaviour.shiftValsue.z);
            }
            else if (changed.Behaviour.yshift)
            {
                temp = new Vector3(changed.Behaviour.shiftValsue.x, changed.Behaviour.shiftValsue.y * (changed.Behaviour.pivotPosition.childCount - 1), changed.Behaviour.shiftValsue.z);
            }
            else if (changed.Behaviour.zshift)
            {
                temp = new Vector3(changed.Behaviour.shiftValsue.x, changed.Behaviour.shiftValsue.y, changed.Behaviour.shiftValsue.z * (changed.Behaviour.pivotPosition.childCount - 1));
            }
            changed.Behaviour.gameObject.transform.localPosition = Vector3.zero + temp;
            changed.Behaviour.gameObject.transform.localRotation = Quaternion.Euler(changed.Behaviour.rotateValsue);
            changed.Behaviour.gameObject.transform.GetComponent<XrOffsetGrabInteractable>().enabled = false;
        }
    }

   /* private async void OnTriggerStay(Collider other)
    {
        Debug.Log("stay");
        if (other.gameObject.CompareTag(colTag))
        {
            isTakingAuthority = true;
            bool auth = await Object.WaitForStateAuthority();
            isTakingAuthority = false;
            doMatch = true;
        }
    }*/
    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(colTag))
        {
            isTakingAuthority = true;
            bool auth = await Object.WaitForStateAuthority();
            isTakingAuthority = false;
            doMatch = true;
            GetComponent<Rigidbody>().isKinematic = true;
            audio.Play();
        }
    }
}
