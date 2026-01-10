using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
public static class EditorBootstrapper
{
    // This runs automatically before the first scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Debug.Log("EditorBootstrapper: Checking for Persistent Managers...");

        // Check if our persistent scene is already loaded
        // (Replace "PersistentManagers" with the exact name of your manager scene)
        bool isManagerLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "PersistentManagers")
            {
                isManagerLoaded = true;
                break;
            }
        }

        // If it's not loaded, load it additively
        if (!isManagerLoaded)
        {
            SceneManager.LoadScene("PersistentManagers", LoadSceneMode.Additive);
        }
    }
}
#endif