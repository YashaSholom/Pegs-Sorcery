using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Player Stats")]
    public int level = 1;
    public float health = 100f;
    public float maxHealth = 100f;
    
    [Header("Player Position (for testing)")]
    public Vector3 testPosition = Vector3.zero;
    
}
