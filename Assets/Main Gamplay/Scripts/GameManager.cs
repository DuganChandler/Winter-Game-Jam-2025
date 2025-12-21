using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    Pause,
    Gameover,
    Win
}

public class GameManager : MonoBehaviour {
    private static GameManager _Instance;
    public static GameManager Instance { 
        get { 
            if (!_Instance) {
                _Instance = new GameObject().AddComponent<GameManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        } 
    }

    public event System.Action<GameState> OnGameStateChanged;

    private GameState m_gameState = GameState.MainMenu;
    public GameState GameState
    {
        get
        {
            return m_gameState;
        }
        set
        {
            if (m_gameState == value) return; 
            m_gameState = value;
            OnGameStateChanged?.Invoke(m_gameState);
        }
    }

    public bool Restart { get; set; } = false;
}