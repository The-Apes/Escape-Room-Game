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
                    description.SetText("<size=50>Controls:</size> \n <b>WASD</b> - Movement \n <b>Scroll</b> - Cycle Choice \n <b>F</b> - Select Choice\n <b>P</b> - Pause");
                    continueButton.SetText("Start");
                    _continueStage++;
                    break;
                case 1:
                    // Loads the next level, but i forgot how to do that, coPilot help me in the next line
                    UnityEngine.SceneManagement.SceneManager.LoadScene("TestNPCRoom");
                    break;
            }
        }
    }
}
