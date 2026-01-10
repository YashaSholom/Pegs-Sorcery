using UnityEngine;

public abstract class Service : IService
{
    public void Dispose()
    {
        Debug.LogWarning("Service: Dispose method not implemented.");
    }

    public void Initialize()
    {
        Debug.LogWarning("Service: Initialize method not implemented.");
    }
}
