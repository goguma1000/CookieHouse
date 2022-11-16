using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;
public class SessionListPanel : MonoBehaviour
{
    [SerializeField] private GameObject itemParent;
    [SerializeField] private GameObject sessionListPrefab;
    [SerializeField] private NewSessionTab newSessionTab;
    [SerializeField] private TextMeshProUGUI _error;

    private NetworkManager manager;
    [SerializeField] private float space = 0;
    private float y = 0;

    public async void Show()
    {
        gameObject.SetActive(true);
       // _error.text = "";
        manager = NetworkManager.FindInstance();
        UpdateSessionList(new List<SessionInfo>());
        await manager.EnterLobby($"CookieHouse", UpdateSessionList);
    }

    public void Hide()
    {
        newSessionTab.HidePannel();
        manager?.Disconnect();
        gameObject.SetActive(false);
    }

    public void UpdateSessionList(List<SessionInfo> sessions)
    {
        Recycle();
        foreach(SessionInfo session in sessions)
        {
            AddRow(session);
        }
        ResizeContent();
    }

    private void AddRow( SessionInfo session)
    {
        GameObject go = Instantiate(sessionListPrefab, Vector3.zero, Quaternion.identity, itemParent.transform);
        go.GetComponent<SessionListItem>().Setup(session, sellectedSession =>
        {
            manager.JoinSession(sellectedSession);
        });
       
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
        rt.anchoredPosition = new Vector2(-space, y);
        
       

        y -= rt.sizeDelta.y;
    }

    private void Recycle()
    {   
        if(itemParent != null)
        {
           SessionListItem[] items = itemParent.GetComponentsInChildren<SessionListItem>();
            foreach(SessionListItem item in items)
            {
                item.transform.SetParent(null);
                Destroy(item.gameObject);
            }
        }
        y = -space;
    }

    private void ResizeContent()
    {
        RectTransform rt = itemParent.GetComponent<RectTransform>();
        int count = itemParent.transform.childCount;
        float height = ((sessionListPrefab.GetComponent<RectTransform>().sizeDelta.y * count) + (space * (count - 1)));
        Debug.Log($"Count: {count} Space: {space} height:{height}");
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }

    public void Start()
    {
        Show();
    }

}
