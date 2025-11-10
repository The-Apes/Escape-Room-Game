using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject disclaimerMenu;
    
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI continueButton;

        public AudioMixer audioMixer;
        public Slider musicSlider;
        public Slider sfxSlider;

        private const float DefaultVolume = 0f;    // 0 dB (max volume)
        private const float MinVolume = -80f;      // -80 dB (min volume)

        private int _continueStage = 0;

          private void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }
    
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
        
        public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

        public void UpdateSoundVolume(float volume)
        {
            audioMixer.SetFloat("SFXVolume", volume);
        }

        public void SaveVolume()
        {
            audioMixer.GetFloat("MusicVolume", out float musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);

            audioMixer.GetFloat("SFXVolume", out float sfxVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }
    
    public void LoadVolume()
        {
            // Music
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
                if (musicSlider != null)
                    musicSlider.value = musicVolume;
                audioMixer.SetFloat("MusicVolume", musicVolume);
            }
            else
            {
                if (musicSlider != null)
                    musicSlider.value = DefaultVolume;
                audioMixer.SetFloat("MusicVolume", DefaultVolume);
            }

            // SFX
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
                if (sfxSlider != null)
                    sfxSlider.value = sfxVolume;
                audioMixer.SetFloat("SFXVolume", sfxVolume);
            }
            else
            {
                if (sfxSlider != null)
                    sfxSlider.value = DefaultVolume;
                audioMixer.SetFloat("SFXVolume", DefaultVolume);
            }
        }

        public void ResetToDefaults()
        {
            if (musicSlider != null)
                musicSlider.value = DefaultVolume;
            if (sfxSlider != null)
                sfxSlider.value = DefaultVolume;

            audioMixer.SetFloat("MusicVolume", DefaultVolume);
            audioMixer.SetFloat("SFXVolume", DefaultVolume);

            SaveVolume();
        }
    
    }
}