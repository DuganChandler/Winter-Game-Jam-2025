using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPage;
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject logoImage;
    [SerializeField] private GameObject gameplayPanel;

    void OnEnable()
    {
        gameplayPanel.SetActive(false);
        SoundManager.Instance.PlayMusic("music");
        GameManager.Instance.GameState = GameState.MainMenu;
        if (GameManager.Instance.Restart)
        {
            StartGame();
            GameManager.Instance.Restart = false;
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameplayPanel.SetActive(true);
        // trigger the game start
        // zoom camera out
        GameManager.Instance.GameState = GameState.Gameplay;
    }

    public void Credits()
    {
        menuButtons.SetActive(false);
        creditsPage.SetActive(true);
        logoImage.SetActive(false);
    }

    public void OnCreditsBack()
    {
        creditsPage.SetActive(false);
        menuButtons.SetActive(true);
        logoImage.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
