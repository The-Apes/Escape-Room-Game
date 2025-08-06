using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    private bool _paused;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject pauseScreen;
    private Logs _logs;

    public void Start()
    {
        _logs = FindFirstObjectByType<Logs>();
    }
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (_paused)
        {
            gameScreen.SetActive(true);
            pauseScreen.SetActive(false);
            if (_logs) _logs.ClearLogs();
            Time.timeScale = 1;
            _paused = false;
        }
        else
        {
            gameScreen.SetActive(false);
            pauseScreen.SetActive(true);
            if (_logs) _logs.ShowLogs();
            Time.timeScale = 0;
            _paused = true;
        }
    }
}
