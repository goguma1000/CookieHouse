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
    public AudioSource audio;
    public AudioSource bottleAudio;
    private bool oneTimePlay = false;
    // Update is called once per frame
    void Update()
    {
        if(answer == 3 && !eventOn)
        {
            if (!oneTimePlay)
            {
                audio.Play();
                oneTimePlay = true;
            }

            if (eventItem.transform.localRotation.eulerAngles.y >= 30)
            {
                eventOn = true;
                eventItem.GetComponent<Door>().isOpen = true;
                this.enabled = false;
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
            if(!bottleAudio.isPlaying)
                bottleAudio.Play();
            Destroy(collision.gameObject);
        }
    }
}
