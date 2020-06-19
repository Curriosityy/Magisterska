using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    private static bool _ShuttingDown = false;
    private static object _Lock = new object();
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (_Lock)
            {
                if (_Instance == null)
                {
                    // Search for existing instance.
                    _Instance = FindObjectOfType<T>();

                    // Create new instance if one doesn't already exist.
                    if (_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        GameObject singletonObject = new GameObject();
                        _Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        _ShuttingDown = true;
    }


    private void OnDestroy()
    {
        _ShuttingDown = true;
    }

}
