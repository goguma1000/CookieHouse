using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
public class Piece : NetworkBehaviour
{
    [Networked(OnChanged = nameof(setMatch))]
    public NetworkBool doMatch { get; set; }

    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distOffset;
    private bool isTakingAuthority = false;
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
        }
    }
    public async void CheckRightPosition()
    {
        float distance = Vector3.Distance(targetPosition.position, this.transform.position);
        if (distance < distOffset)
        {
            isTakingAuthority = true;
            bool auth = await Object.WaitForStateAuthority();
            isTakingAuthority = false;
            if (auth)
            {
                transform.SetParent(null);
                doMatch = true;
            }
        }
        else if (distOffset < distance && distance < 3 * distOffset)
        {
            this.gameObject.transform.position += new Vector3(-0.2f, 0, 0);
        }
    }
}


