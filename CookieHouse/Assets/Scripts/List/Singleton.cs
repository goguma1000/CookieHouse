using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;
    private static readonly object _lock = new object();
   public static T Instance
    {
        get
        {
            if (applicationsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }
            lock (_lock)
            {
                if(instance == null)
                {
                    var all = FindObjectsOfType<T>();
                    instance = all != null && all.Length > 0 ? all[0] : null;

                    if(all != null && all.Length > 1){
                        Debug.LogWarning("[Singleton] There are " + all.Length + " instances of " + typeof(T) + "... This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton)" + typeof(T).ToString();

                        Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");

                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " + instance.gameObject.name);
                    }

                    if (Application.isPlaying)
                        DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }
    }

    private static bool applicationsQuitting = false;
}
