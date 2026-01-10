using UnityEngine;

[RequireComponent(typeof(MainMenuView))]
public class MainMenuController : MonoBehaviour
{
    private MainMenuView mainMenuView;

    void Start()
    {
        mainMenuView = GetComponent<MainMenuView>();
        Init();
    }

    private void Init()
    {
        mainMenuView.AddStartGameListener(OnStartGameClicked);
    }

    private void OnStartGameClicked()
    {
        SceneService.LoadScene("GameScene");
    }
}
