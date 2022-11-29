using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeLoadingPanel : MonoBehaviour
{
    public GameObject doughnut;
    [SerializeField]private float angle = 30;
    // Update is called once per frame
    void Update()
    {
        doughnut.transform.Rotate(Vector3.forward,angle * Time.deltaTime);  
    }
}
