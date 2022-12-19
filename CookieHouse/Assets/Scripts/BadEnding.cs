using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Fusion;

public class BadEnding : NetworkBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject rawImage;
    public GameObject leftDoor;
    public GameObject rightDoor;
   [Networked(OnChanged = nameof(PlayVideo))]
    private int playerCount { get; set; }

    private static void PlayVideo(Changed<BadEnding> changed)
    {
        changed.LoadNew();
        if(changed.Behaviour.playerCount == 1)
        {
            changed.Behaviour.videoPlayer.Play();
            changed.Behaviour.leftDoor.transform.localRotation = Quaternion.identity;
            changed.Behaviour.rightDoor.transform.localRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        if (!videoPlayer.isPlaying && rawImage.activeSelf)
        {
            rawImage.SetActive(false);
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool auth = await Object.WaitForStateAuthority();
            playerCount++;
        }
    }

    private async void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool auth = await Object.WaitForStateAuthority();
            playerCount--;
        }
    }
}
