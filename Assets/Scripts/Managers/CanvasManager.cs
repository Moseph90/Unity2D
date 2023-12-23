using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button returnGame;
    public Button returnMenu;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject winText;

    [Header("Text")]
    public Text masterVolSliderText;
    public Text musicVolSliderText;
    public Text sfxVolSliderText;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Image")]
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    [Header("Variable")]
    public int health;
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = GameManager.Instance.maxLives;
        health = GameManager.Instance.lives;

        if (winText) winText.SetActive(false);
        if (masterVolSlider) masterVolSlider.value = 100;
        if (musicVolSlider) musicVolSlider.value = 100;
        if (sfxVolSlider) sfxVolSlider.value = 100;

        if (playButton != null)
        {
            playButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));
            Time.timeScale = 1f;
        }
        if (returnMenu) returnMenu.onClick.AddListener(() => GameManager.Instance.ChangeScene(0));
        if (returnGame) returnGame.onClick.AddListener(UnpauseGame);
        if (settingsButton) settingsButton.onClick.AddListener(ShowSettingsMenu);
        if (backButton) backButton.onClick.AddListener(ShowMainMenu);
        if (quitButton) quitButton.onClick.AddListener(Quit);
        GameManager.Instance.OnLivesValueChanged.AddListener((value) => UpdateLivesSprites(value));
        //for (int i = 0; i < 3; i++)
        //{
        //    if (i < health) hearts[i].sprite = fullHeart;
        //    else hearts[i].sprite = emptyHeart;
        //    if (i < maxHealth) hearts[i].enabled = true;
        //    else hearts[i].enabled = false;
        //}
        if (masterVolSlider)
        {
            masterVolSlider.onValueChanged.AddListener((value) => OnMasterSliderValueChanged(value));

            float newValue;
            audioMixer.GetFloat("MasterVol", out newValue);
            masterVolSlider.value = newValue + 80;
            masterVolSliderText.text = (Mathf.Ceil(newValue + 80).ToString());
            if (masterVolSliderText)
                masterVolSliderText.text = masterVolSlider.value.ToString();
        }
        if (musicVolSlider)
        {
            musicVolSlider.onValueChanged.AddListener((value) => OnMusicSliderValueChanged(value));

            float newValue;
            audioMixer.GetFloat("MusicVol", out newValue);
            musicVolSlider.value = newValue + 80;
            musicVolSliderText.text = (Mathf.Ceil(newValue + 80).ToString());
            if (musicVolSliderText)
                musicVolSliderText.text = musicVolSlider.value.ToString();
        }
        if (sfxVolSlider)
        {
            sfxVolSlider.onValueChanged.AddListener((value) => OnSFXSliderValueChanged(value));

            float newValue;
            audioMixer.GetFloat("SFXVol", out newValue);
            sfxVolSlider.value = newValue + 80;
            sfxVolSliderText.text = (Mathf.Ceil(newValue + 80).ToString());
            if (sfxVolSliderText)
                sfxVolSliderText.text = sfxVolSlider.value.ToString();
        }
    }
    public void ShowText()
    {
        winText.SetActive(true);
    }
    void OnMasterSliderValueChanged(float value)
    {
        masterVolSliderText.text = value.ToString();
        audioMixer.SetFloat("MasterVol", value - 80);
    }
    void OnMusicSliderValueChanged(float value)
    {
        musicVolSliderText.text = value.ToString();
        audioMixer.SetFloat("MusicVol", value - 80);
    }
    void OnSFXSliderValueChanged(float value)
    {
        sfxVolSliderText.text = value.ToString();
        audioMixer.SetFloat("SFXVol", value - 80);
    }
    void UpdateLivesSprites(int value)
    {
        // if (livesText) livesText.text = "Lives: " + value.ToString();
        for (int i = 0; i < value; i++)
        {
            if (i < health) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;
            if (i < maxHealth) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }

    void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;
        if (Input.GetKeyDown(KeyCode.P) && !PlayerController.win)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf )
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        maxHealth = GameManager.Instance.maxLives;
        health = GameManager.Instance.lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = fullHeart;
            else hearts[i].sprite = emptyHeart;
            if (i < maxHealth) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }
}
