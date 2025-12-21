using System.Collections;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        SceneManager.LoadSceneAsync("Title");
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void UnPauseGame()
    {
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
        StartCoroutine(DeathCoroutine());
    }
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        deathMenu.SetActive(true);
    }
}
