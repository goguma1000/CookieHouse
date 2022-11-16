using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PooledObject _prefab;
    private List<PooledObject> _free = new List<PooledObject>();
    private static ObjectPoolRoot poolRoot;

    public PooledObject prefab
    {
        get { return _prefab; }
    }

    public PooledObject Instantiate()
    {
        return Instantiate(_prefab.transform.position, prefab.transform.rotation);
    }

    public PooledObject Instantiate(Vector3 p, Quaternion q, Transform parent = null)
    {
        PooledObject newt = null;
        if(_free.Count > 0)
        {
            var t = _free[0];
            if (t)
            {
                Transform xform = t.transform;
                xform.SetParent(parent, false);
                xform.position = p;
                xform.rotation = q;
                newt = t;
            }
            else
            {
                Debug.LogWarning("Recycled object of type <" + _prefab + "> was destroyed - not re-using!");
            }

            _free.RemoveAt(0);
        }

        if(newt == null)
        {
            newt = Object.Instantiate(_prefab, p, q, parent);
            newt.name = "Instance(" + newt.name + ")";
            newt.pool = this;
        }

        newt.OnRecycled();
        newt.gameObject.SetActive(true);
        return newt;
    }
    public static void Recycle(PooledObject po)
    {
        if(po != null)
        {
            if(po.pool == null)
            {
                po.gameObject.SetActive(false);
                po.transform.SetParent(null, false);
                Object.Destroy(po.gameObject);
            }
            else
            {
                po.pool._free.Add(po);
                if(poolRoot == null)
                {
                    poolRoot = Singleton<ObjectPoolRoot>.Instance;
                    poolRoot.name = "ObjectPoolRoot";
                }

                po.gameObject.SetActive(false);
                po.transform.SetParent(poolRoot.transform, false);
            }
            
        }
    }
}
