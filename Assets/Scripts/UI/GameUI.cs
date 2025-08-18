using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject pauseScreen;
        private bool _paused;
        private Logs _logs;
        private FPController _fpController;

        public void Start()
        {
            _logs = FindFirstObjectByType<Logs>();
            _fpController = FindFirstObjectByType<FPController>();
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
                if (_fpController) _fpController.CanMove = true;
                if (_fpController) _fpController.CanLook = true;
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
                if (_fpController) _fpController.CanMove = false;
                if (_fpController) _fpController.CanLook = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true; 
                Time.timeScale = 0;
                _paused = true;
            }
        }
    }
}
