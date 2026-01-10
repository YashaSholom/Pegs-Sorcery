using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneService
{
    public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneService: Cannot load scene - scene name is null or empty");
            return;
        }

        Debug.Log($"SceneService: Loading scene '{sceneName}' with mode {loadSceneMode}");
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }

    public static void LoadScene(int sceneBuildIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"SceneService: Cannot load scene - invalid build index {sceneBuildIndex}");
            return;
        }

        Debug.Log($"SceneService: Loading scene at build index {sceneBuildIndex} with mode {loadSceneMode}");
        SceneManager.LoadScene(sceneBuildIndex, loadSceneMode);
    }

    public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneService: Cannot load scene asynchronously - scene name is null or empty");
            return null;
        }

        Debug.Log($"SceneService: Loading scene '{sceneName}' asynchronously with mode {loadSceneMode}");
        return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
    }

    public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (sceneBuildIndex < 0 || sceneBuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"SceneService: Cannot load scene asynchronously - invalid build index {sceneBuildIndex}");
            return null;
        }

        Debug.Log($"SceneService: Loading scene at build index {sceneBuildIndex} asynchronously with mode {loadSceneMode}");
        return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);
    }

    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static int GetActiveSceneBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
