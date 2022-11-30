using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Piece : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distOffset;
    public void CheckRightPosition()
    {
        transform.SetParent(null);
      
        if (Vector3.Distance(targetPosition.position, this.transform.position) < distOffset)
        {
            transform.SetParent(targetPosition);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1, 1, 1);
            transform.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
}
