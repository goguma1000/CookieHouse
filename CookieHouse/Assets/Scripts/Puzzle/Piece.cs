using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            transform.eulerAngles = new Vector3(90, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
