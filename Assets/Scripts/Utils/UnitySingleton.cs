using Unity.VisualScripting;
using UnityEngine;

public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                SetupInstance();
            }
            return _instance;
        }
    }

    public static bool IsInitialized
    {
        get { return _instance != null; }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private static void SetupInstance()
    {
        _instance = FindObjectOfType(typeof(T)) as T;

        if (_instance == null)
        {
            GameObject obj = new GameObject();
            _instance = (T)obj.AddComponent(typeof(T));
            //obj.hideFlags = HideFlags.DontSave;
            obj.name = typeof(T).Name;
            DontDestroyOnLoad(obj);
        }
    }
}
