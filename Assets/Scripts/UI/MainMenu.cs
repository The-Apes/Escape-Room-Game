using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject disclaimerMenu;
    
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI continueButton;

        private int _continueStage = 0;
    
        public void StartButton()
        {
            if (mainMenu != null)
            {
                mainMenu.SetActive(false);
            }
            if (disclaimerMenu != null)
            {
                disclaimerMenu.SetActive(true);
            }
        }

        public void NextButton()
        {
            switch (_continueStage)
            {
                case 0:
                    if (description != null)
                    {
                        description.SetText(
                            "<size=50>Controls:</size>\n" +
                            "<b>WASD / Left Stick</b> - Move\n" +
                            "<b>Mouse / Right Stick</b> - Look Around\n" +
                            "<b>Space / South Button</b> - Jump\n" +
                            "<b>C / Left Stick Press</b> - Crouch\n" +
                            "<b>Left Mouse / West Button</b> - Interact\n" +
                            "<b>G / East Button</b> - Drop\n" +
                            "<b>F / North Button</b> - Inspect\n" +
                            "<b>E / West Button</b> - Select Choice\n" +
                            "<b>Scroll / D-Pad</b> - Navigate Choice\n" +
                            "<b>Q / Left Shoulder</b> - Call NPC\n" +
                            "<b>Escape / Start</b> - Pause, <color=yellow><b>Dialogue Logs</b></color>"
                        );
                    }

                    if (continueButton != null)
                        continueButton.SetText("Start");

                    _continueStage++;
                    break;

                case 1:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("TestNPCRoom");
                    break;
            }
        }
        public void ExitButton()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
}