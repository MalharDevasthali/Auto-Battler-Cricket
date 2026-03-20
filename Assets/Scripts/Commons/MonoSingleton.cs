using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple instances of {typeof(T)} detected!");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (Instance == null)
        {
            Debug.LogError($"{typeof(T)} is not correctly inherited!");
        }
    }
}