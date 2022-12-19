using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject otherDoor;
    private float timer = 0;
    private void Update()
    {
        if (isOpen)
        {
            timer += Time.deltaTime;
        
        }

        if(timer >= 180)
        {
            if (otherDoor.transform.localRotation.eulerAngles.y >= 30)
            {
                otherDoor.GetComponent<Door>().enabled = false;
                this.enabled = false;
                return;
            }
            otherDoor.transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }
    }
}
