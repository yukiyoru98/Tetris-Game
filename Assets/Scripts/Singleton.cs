using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this as T)
        {
            Debug.LogError($"Duplicated singleton instance of {typeof(T)}.");
            Destroy(this);
            return;
        }
        instance = this as T;
    }
}
