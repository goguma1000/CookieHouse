using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : PooledObject
{
    public bool endRow { get; set; }

    public RectTransform rectTransform
    {
        get { return (RectTransform)transform; }
    }
    public float width { 
        set { ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value); }
        get { return ((RectTransform)transform).rect.width; }
    }

    public float height
    {
        set { ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value); }
        get { return ((RectTransform)transform).rect.height; }
    }

    public Vector2 offset
    {
        get { return new Vector2(rectTransform.anchoredPosition.x, -rectTransform.anchoredPosition.y); }
    }

}
