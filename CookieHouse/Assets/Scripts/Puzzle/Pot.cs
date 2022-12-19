using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Pot : NetworkBehaviour
{
    [Networked(OnChanged = nameof(UpdateValue))]
    private int answer { get; set; }
    private bool eventOn =false;
    [SerializeField] GameObject eventItem;
    private bool isTakingAuthority = false;
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
    private static void UpdateValue(Changed<Pot> changed)
    {
        changed.LoadNew();
    }
    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            isTakingAuthority = true;
            bool auth = await Object.WaitForStateAuthority();
            isTakingAuthority = false;
            answer++;
            Destroy(collision.gameObject);
        }
    }
}
