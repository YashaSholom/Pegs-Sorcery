using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] Button startGameButton;

    public void AddStartGameListener(UnityEngine.Events.UnityAction action)
    {
        startGameButton.onClick.AddListener(action);
    }
}
