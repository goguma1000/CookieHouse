using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PuzzleBoard : NetworkBehaviour
{
    [SerializeField] GameObject[] Masks;
    void Update()
    {
        for (int i = 0; i < Masks.Length; i++)
        {
            if (Masks[i].transform.childCount == 0) return;
        }
        Debug.Log("solved Puzzle");
    }
}
