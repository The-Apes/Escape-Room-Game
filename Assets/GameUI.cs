using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject pauseScreen;
    private bool _paused;
    private Logs _logs;
    private FPController _FPController;

    public void Start()
    {
        _logs = FindFirstObjectByType<Logs>();
        _FPController = FindFirstObjectByType<FPController>();
    }
    public void PauseInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        PauseGame();

    }

    public void PauseGame()
    {
        if (_paused)
        {
            gameScreen.SetActive(true);
            pauseScreen.SetActive(false);
            if (_logs) _logs.ClearLogs();
            if (_FPController) _FPController.canMove = true;
            if (_FPController) _FPController.canLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
            Time.timeScale = 1;
            _paused = false;
        }
        else
        {
            gameScreen.SetActive(false);
            pauseScreen.SetActive(true);
            if (_logs) _logs.ShowLogs();
            if (_FPController) _FPController.canMove = false;
            if (_FPController) _FPController.canLook = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true; 
            Time.timeScale = 0;
            _paused = true;
        }
    }
}
