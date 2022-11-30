using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualizeLoadingText : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    
    public void SetTMPtext(string s)
    {
        loadingText.text = s;
    }
    
}
