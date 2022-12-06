using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMatch : MonoBehaviour
{
    public GameObject matchTarget;
    // Update is called once per frame
    void Update()
    {
        Debug.Log($"match y: {matchTarget.transform.up}, obj y: {transform.up}, Dot: {Vector3.Dot(matchTarget.transform.up, transform.up)}");
        Debug.Log($"match y: {matchTarget.transform.right}, obj y: {transform.right}, Dot: {Vector3.Dot(matchTarget.transform.right, transform.right)}");
        if (Vector3.Dot(matchTarget.transform.up, transform.up) > 0.99 && Vector3.Dot(matchTarget.transform.right, transform.right) > 0.99)
        {
            Debug.Log($"match");
        }
    }
}
