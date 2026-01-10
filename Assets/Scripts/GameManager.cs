using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static List<IService> Services;

    void Awake()
    {
        // Initialize the services list if it hasn't been initialized yet
        if (Services == null)
        {
            Services = new List<IService>();
        }

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void AddService<T>(T service) where T : IService
    {
        if (Services == null)
        {
            Services = new List<IService>();
        }
        Services.Add(service);
        service.Initialize();
    }

    public static void RemoveService<T>(T service) where T : IService
    {
        if (Services != null)
        {
            Services.Remove(service);
            service.Dispose();
        }
    }
}
