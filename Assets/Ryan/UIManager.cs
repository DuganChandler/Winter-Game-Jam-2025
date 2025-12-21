using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject victoryScreen;
    private void Awake()
    { 
        Player.OnPlayerDeath += OnPlayerDeath;
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape) 
            && (GameManager.Instance.GameState == GameState.Gameplay || GameManager.Instance.GameState == GameState.Pause))
        {
            TogglePause();
        }
    }
    public void RestartGame()
    {
        // sets the restart falg to treu to immediatly start the game in MainMenuController
        UnPauseGame();
        GameManager.Instance.Restart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        // in case it becomes true accidently idk..
        UnPauseGame();
        GameManager.Instance.Restart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win)
        {
            StartCoroutine(VictoryCoroutine());
        }
    }
    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(2f);
        victoryScreen.SetActive(true);
    }
}
