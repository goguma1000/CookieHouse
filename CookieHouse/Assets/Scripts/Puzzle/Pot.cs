using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private int answer = 0;
    private bool eventOn =false;
    [SerializeField] GameObject eventItem;
    
    // Update is called once per frame
    void Update()
    {
        if(answer == 3 && !eventOn)
        {
            if(eventItem.transform.localRotation.eulerAngles.y >= 30)
            {
                eventOn = true;
                return;
            }
            eventItem.transform.Rotate(Vector3.up, 30*Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            answer++;
            Destroy(collision.gameObject);
        }
    }
}
