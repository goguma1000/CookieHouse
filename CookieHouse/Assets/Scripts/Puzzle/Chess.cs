using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Chess : NetworkBehaviour
{
    [Networked(OnChanged = nameof(EventOn))]
    private bool isEventOn { get; set; }
    public GameObject[] eventitems;
    public GameObject otherChess;
    public Transform targetPosition;
    public GameObject hintObject;
    public float distOffset;
    private bool isTakingAuthority = false;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private bool timerON = false;
    private float timer = 0;
    private void Awake()
    {
        defaultPosition = this.gameObject.transform.position;
        defaultRotation = this.gameObject.transform.rotation;
    }
    private void Update()
    {
        if (timerON)
        {
            timer += Time.deltaTime;
        }
        else if (!timerON && timer != 0) timer = 0;
        
        if(timer > 60f && !hintObject.activeSelf)
        {
            hintObject.SetActive(true);
        }
    }
    public void TimerOn()
    {
        timerON = true;
    }

    public void TimerOff()
    {
        timerON = false;
        hintObject.SetActive(false);
        timer = 0;
    }
    public async void CheckRightPosition()
    {
        float distance = Vector3.Distance(targetPosition.position, this.transform.position);
        if (distance < distOffset)
        {
            isTakingAuthority = true;
            bool auth = await Object.WaitForStateAuthority();
            isTakingAuthority = false;
            if (auth)
            {
                isEventOn = true;
            }
            return;
        }
        else
        {
            this.gameObject.transform.position = defaultPosition;
            this.gameObject.transform.rotation = defaultRotation;
        }
    }

    private static void EventOn(Changed<Chess> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.isEventOn)
        {
            changed.Behaviour.gameObject.transform.SetParent(changed.Behaviour.targetPosition);
            changed.Behaviour.gameObject.transform.localPosition = Vector3.zero;
            changed.Behaviour.gameObject.transform.localRotation = Quaternion.identity;
            changed.Behaviour.gameObject.transform.localScale = new Vector3(1, 1, 1);
            changed.Behaviour.gameObject.transform.GetComponent<XrOffsetGrabInteractable>().enabled = false;
            changed.Behaviour.eventitems[0].SetActive(false);
            changed.Behaviour.eventitems[1].SetActive(true);
            changed.Behaviour.otherChess.GetComponent<XrOffsetGrabInteractable>().enabled = false;
            changed.Behaviour.enabled = false;
        }
    }
}
