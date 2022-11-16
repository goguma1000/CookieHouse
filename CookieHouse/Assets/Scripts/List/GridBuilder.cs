using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridBuilder : GridCell
{
    public bool autoResize = true;
    public bool resizeWidth = true;
    public bool resizeHeight = true;

    public float maxLayoutheight { get; set; }
    public float maxLayoutwidth { get; set; }
    public float minLayoutheight { get; set; }
    public float minLayoutwidth { get; set; }

    private List<GridCell> currentChildren = new List<GridCell>();
    private float rowWidth = 0;
    private float rowHeight = 0;
    private float nextX = 0;
    private float nextY = 0;
    private int childCounter;
    private void Awake()
    {
        OnRecycled();
    }

    public override void OnRecycled()
    {
        base.OnRecycled();
        maxLayoutheight = float.MaxValue;
        maxLayoutwidth = float.MaxValue;

        float startW = 0;
        float startH = 0;

        if (transform.parent)
        {
            RectTransform rt = rectTransform;

            if (!resizeWidth)
                startW = (rt.anchorMax.x - rt.anchorMin.x) * ((RectTransform)transform.parent).rect.width;
            if(!resizeHeight)
                startH = (rt.anchorMax.y - rt.anchorMin.y) * ((RectTransform)transform.parent).rect.height;
        }

        minLayoutwidth = startW;
        minLayoutheight = startH;
    }

    public void BeginUpdate()
    {
        if(currentChildren.Count != transform.childCount)
        {
            currentChildren.Clear();
            for(int c = 0; c < transform.childCount;)
            {
                GameObject go = transform.GetChild(c).gameObject;
                var po = go.GetComponent<PooledObject>();
                if (po != null)
                    ObjectPool.Recycle(po);
                else
                {
                    c++;
                }
            }
        }

        childCounter = 0;
        nextX = 0;
        nextY = 0;
        rowWidth = 0;
        rowHeight = 0;
    }

    public void EndRow()
    {
        if(childCounter > 0 && !currentChildren[childCounter - 1].endRow)
        {
            currentChildren[childCounter - 1].endRow = true;
            MoveNextRow();
        }
    }
    public void EndUpdate()
    {
        EndRow();
        for(int i = childCounter; i < currentChildren.Count;)
        {
            ObjectPool.Recycle(currentChildren[i]);
            currentChildren.RemoveAt(i);
        }
        UpdateRect(rowWidth, -nextY);
    }

    private void UpdateRect(float w, float h)
    {
        if (resizeWidth) width = Mathf.Max(minLayoutwidth, Mathf.Min(maxLayoutwidth, w));
        if (resizeHeight) height  = Mathf.Max(minLayoutheight, Mathf.Min(maxLayoutheight, h));
    }
    public enum Alignment
    {
        Default,
        Left,
        Center,
        Right
    }

    public T AddRow<T>(T prefab, Action<T> setup = null, Alignment align = Alignment.Default) where T: GridCell
    {
        return AddCell(prefab, setup, true);
    }

    public T AddCell<T>(T prefab, Action<T> setup = null, bool endRow = false) where T: GridCell
    {
        T panel = Add(prefab, nextX, nextY);
        if (setup != null)
            setup(panel);
        panel.endRow = endRow;
        LayoutCell(panel);

        return panel;
    }

    private T Add<T>(T prefab, float x, float y) where T: GridCell
    {
        GridCell panel;
        if(childCounter < currentChildren.Count && currentChildren[childCounter].pool.prefab == prefab)
        {
            panel = currentChildren[childCounter];
        }
        else
        {
            panel = ObjectPool.Instantiate(prefab);
            RectTransform rt = (RectTransform)panel.transform;
            rt.localScale = Vector3.one;

            if (childCounter < currentChildren.Count)
            {
                ObjectPool.Recycle(currentChildren[childCounter]);
                currentChildren[childCounter] = panel;
            }
            else
                currentChildren.Add(panel);
        }

        childCounter++;
        panel.transform.localPosition = new Vector2(x, y);
        return (T)panel;
    }

    private void LayoutCell(GridCell cell)
    {
        cell.rectTransform.anchoredPosition = new Vector2(nextX, nextY);
        nextX += cell.width;
        if (nextX > rowWidth)
            rowWidth = nextX;
        if (cell.height > rowHeight)
            rowHeight = cell.height;
        if (cell.endRow)
            MoveNextRow();
    }

    void MoveNextRow()
    {
        nextY -= rowHeight;
        nextX = 0;
        rowHeight = 0;
    }
}
