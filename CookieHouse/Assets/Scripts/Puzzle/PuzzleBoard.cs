using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PuzzleBoard : NetworkBehaviour
{
    [SerializeField] GameObject[] Masks;
    [SerializeField] GameObject[] eventItems;
    public bool isMuffin = false;
    public bool isCandy = false;
    public bool isSkeleton = false;
    private bool isEventOn = false;
    void Update()
    {
        for (int i = 0; i < Masks.Length; i++)
        {
            if (Masks[i].transform.childCount == 0) return;
        }
        isEventOn = true;
        if (isEventOn)
        {
            isEventOn = false;
            if (isMuffin)
            {
                eventItems[0].gameObject.SetActive(false);
                eventItems[1].gameObject.SetActive(true);
            }
            else if (isCandy || isSkeleton)
            {
                if(!eventItems[0].activeSelf) eventItems[0].SetActive(true);
            }
        }
        this.enabled = false;
    }
   
    public void GetStateAuth(GameObject obj)
    {
        obj.GetComponent<TransformSync>().getStateAuth();
    }
}
