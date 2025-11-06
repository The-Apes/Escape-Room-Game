using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject pauseScreen;
        private bool _paused;
        private Logs _logs;
        private FpController _fpController;

        private void Start()
        {
            _logs = FindAnyObjectByType<Logs>();
            _fpController = FindAnyObjectByType<FpController>();
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
                if (gameScreen) gameScreen.SetActive(true);
                if (pauseScreen) pauseScreen.SetActive(false);

                _logs?.ClearLogs();

                if (_fpController != null)
                {
                    _fpController.CanMove = true;
                    _fpController.CanLook = true;
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                _paused = false;
            }
            else
            {
                if (gameScreen) gameScreen.SetActive(false);
                if (pauseScreen) pauseScreen.SetActive(true);

                _logs?.ShowLogs();

                if (_fpController != null)
                {
                    _fpController.CanMove = false;
                    _fpController.CanLook = false;
                }

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0f;
                _paused = true;
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}