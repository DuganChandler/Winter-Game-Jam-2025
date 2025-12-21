using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    private void Awake()
    {
        UnPauseGame();
        Player.OnPlayerDeath += OnPlayerDeath;
    }
    private void OnDestroy()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
    }
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.GameState == GameState.Gameplay)
        {
            TogglePause();
        }
    }
    public void RestartGame()
    {
        // sets the restart falg to treu to immediatly start the game in MainMenuController
        GameManager.Instance.Restart = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        // in case it becomes true accidently idk..
        GameManager.Instance.Restart = false;
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void PauseGame()
    {
        GameManager.Instance.GameState = GameState.Pause;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void UnPauseGame()
    {
        GameManager.Instance.GameState = GameState.Gameplay;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            UnPauseGame();
        }
        else
        {
            PauseGame();
        }
    }
    private void OnPlayerDeath()
    {
        GameManager.Instance.GameState = GameState.Gameover;
        StartCoroutine(DeathCoroutine());
    }
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        deathMenu.SetActive(true);
    }
}
