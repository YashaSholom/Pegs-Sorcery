using UnityEngine;

/// <summary>
/// MonoBehaviour component that can be attached to a GameObject in scenes for developer testing.
/// This component can be configured with a DevPlayerConfigSO to initialize player data in test mode.
/// </summary>
public class DevPlayerSO : MonoBehaviour
{
    [Header("Test Mode Configuration")]
    [Tooltip("The DevPlayerConfigSO to use when initializing in test mode")]
    public DevPlayerConfigSO devPlayerConfig;
    
    [Header("Debug Info")]
    [SerializeField] private PlayerData currentPlayerData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Auto-apply config if available and enabled
        if (devPlayerConfig != null && devPlayerConfig.useInTestMode && devPlayerConfig.IsValid())
        {
            devPlayerConfig.ApplyToDevPlayer(this);
            currentPlayerData = devPlayerConfig.playerData;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Gets the current player data being used by this dev player.
    /// </summary>
    public PlayerData GetPlayerData()
    {
        return currentPlayerData;
    }
}
