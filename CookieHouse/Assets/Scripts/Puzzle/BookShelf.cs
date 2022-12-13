using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{

    [SerializeField] GameObject[] Masks;
    [SerializeField] int[] answerChildCount;
    [SerializeField] GameObject[] eventItems;
    private bool isEventOn = false;
    void Update()
    {
        for (int i = 0; i < Masks.Length; i++)
        {
            if (Masks[i].transform.childCount != answerChildCount[i]) return;
        }
        isEventOn = true;
        if (isEventOn)
        {
            isEventOn = false;
            eventItems[0].SetActive(false);
            eventItems[1].SetActive(true);
        }

    }
}
