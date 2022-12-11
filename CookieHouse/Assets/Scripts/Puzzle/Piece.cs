using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
public class Piece : NetworkBehaviour
{
    [Networked(OnChanged = nameof(setMatch))]
    private bool doMatch { get; set; }

    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distOffset;
    private static void setMatch(Changed<Piece> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.doMatch)
        {
            changed.Behaviour.gameObject.transform.SetParent(changed.Behaviour.targetPosition);
            changed.Behaviour.gameObject.transform.localPosition = Vector3.zero;
            changed.Behaviour.gameObject.transform.localRotation = Quaternion.identity;
            changed.Behaviour.gameObject.transform.localScale = new Vector3(1, 1, 1);
            changed.Behaviour.gameObject.transform.GetComponent<XrOffsetGrabInteractable>().enabled = false;
            changed.Behaviour.doMatch = false;
        }
    }
    public async void CheckRightPosition()
    {
        float distance = Vector3.Distance(targetPosition.position, this.transform.position);
        if (distance < distOffset)
        {
            bool auth = await Object.WaitForStateAuthority();
            transform.SetParent(null);
            doMatch = true;
        }
        else if (distOffset < distance && distance < 3 * distOffset)
        {
            bool auth = await Object.WaitForStateAuthority();
            this.transform.position += new Vector3(0.2f, 0, 0);
        }
    }
}


