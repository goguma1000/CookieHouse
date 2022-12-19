using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class EndingLocker : NetworkBehaviour
{
    public string colTag;
    [SerializeField] GameObject socket;
    public GameObject leftDoor;
    public GameObject rightDoor;
    [Networked(OnChanged = nameof(EventOn))]
    private NetworkBool isEventOn { get; set; }

    private void Update()
    {
        if (isEventOn)
        {
            if (leftDoor.transform.localRotation.eulerAngles.y >= 90)
            {
                this.enabled = false;
                return;
            }
            leftDoor.transform.Rotate(Vector3.up, 90 * Time.deltaTime);
            rightDoor.transform.Rotate(Vector3.up, -90 * Time.deltaTime);
        }
    }
    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(colTag))
        {
            other.gameObject.transform.parent.GetComponent<TransformSync>().useGravity = false;
            other.gameObject.transform.parent.GetComponent<XrOffsetGrabInteractable>().enabled = false;
            other.gameObject.transform.parent.SetParent(socket.transform);
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.localRotation = Quaternion.identity;
            bool auth = await Object.WaitForStateAuthority();
            isEventOn = true;
        }
    }

    private static void EventOn(Changed<EndingLocker> changed)
    {
        changed.LoadNew();
    }
}
