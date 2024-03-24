using UnityEngine;

public class SingletonBehaviourDontDestroy<T> : SingletonBehaviour<T> where T : MonoBehaviour{
    protected override bool DontDestroy => true;
}

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour{
    private static T _instance;

    public static T Instance {
        get {
            if ((Object)SingletonBehaviour<T>._instance != (Object)null)
                return SingletonBehaviour<T>._instance;
            SingletonBehaviour<T>._instance = Object.FindObjectOfType<T>();
            if ((Object)SingletonBehaviour<T>._instance == (Object)null)
                SingletonBehaviour<T>._instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return SingletonBehaviour<T>._instance;
        }
    }

    public static bool Initialized => (Object)SingletonBehaviour<T>._instance != (Object)null;

    protected virtual bool DontDestroy => false;

    protected virtual void OnAwake() { }

    private void Awake() {
        if ((Object)SingletonBehaviour<T>._instance == (Object)null)
            SingletonBehaviour<T>._instance = this as T;
        else if (SingletonBehaviour<T>._instance.GetInstanceID() != this.GetInstanceID()) {
            Object.Destroy((Object)this.gameObject);
            return;
        }

        if (this.DontDestroy)
            Object.DontDestroyOnLoad((Object)this.gameObject);
        this.OnAwake();
    }

    public virtual void Preload() { }
    protected virtual void OnDestroy() {
        if (!((Object)SingletonBehaviour<T>._instance != (Object)null) ||
            SingletonBehaviour<T>._instance.GetInstanceID() != this.GetInstanceID())
            return;
        SingletonBehaviour<T>._instance = default(T);
    }
}