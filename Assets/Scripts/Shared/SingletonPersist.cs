using UnityEngine;

public class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar en la escena una instancia existente
                _instance = FindObjectOfType<T>();

                // Si no se encontró ninguna, crear una nueva instancia
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();

                    // Hacer que el objeto persista a través de las escenas
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Asegurarse de que solo haya una instancia
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
