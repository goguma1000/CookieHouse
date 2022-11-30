using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NewSessionTab : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputName;
    
    public void OnEnable()
    {
        NetworkManager manger = NetworkManager.FindInstance();
        InputName.text = "Session" + manger.GetInstanceID();
    }
    public void ShowPannel()
    {
        gameObject.SetActive(true);
    }

    public void HidePannel()
    {
        gameObject.SetActive(false);
    }

    public void OnCreateSession()
    {
        if (InputName.text != "")
        {
            SessionProps props = new SessionProps();
            props.RoomName = InputName.text;
            NetworkManager.FindInstance().CreateSessoin(props);
        }
    }
}

