using UnityEngine;

/// <summary>
/// ScriptableObject used by developers to configure test mode settings for scenes.
/// This holds a reference to PlayerData that will be used when running scenes in test mode.
/// </summary>
[CreateAssetMenu(fileName = "New Dev Player Config", menuName = "Game/Dev Player Config")]
public class DevPlayerConfigSO : ScriptableObject
{
    [Header("Player Data")]
    [Tooltip("The PlayerData to use when running scenes in test mode")]
    public PlayerData playerData = new PlayerData();
    
    [Header("Test Mode Settings")]
    [Tooltip("Whether to use this configuration when the scene starts")]
    public bool useInTestMode = true;
    
    /// <summary>
    /// Applies the player data to the DevPlayerSO component if it exists in the scene.
    /// Call this method when entering test mode.
    /// </summary>
    public void ApplyToDevPlayer(DevPlayerSO devPlayer)
    {
        if (devPlayer == null || playerData == null)
        {
            Debug.LogWarning("DevPlayerConfigSO: Cannot apply - DevPlayerSO or PlayerData is null");
            return;
        }
        
        // Apply player data to DevPlayerSO
        // This is where you would set up the dev player with the test data
        Debug.Log($"DevPlayerConfigSO: Applying PlayerData to DevPlayerSO - Level: {playerData.level}, Health: {playerData.health}");
        
        // You can extend this method to apply specific properties to DevPlayerSO
        // For example: devPlayer.InitializeWithData(playerData);
    }
    
    /// <summary>
    /// Validates that the configuration is set up correctly.
    /// </summary>
    public bool IsValid()
    {
        return playerData != null;
    }
}
