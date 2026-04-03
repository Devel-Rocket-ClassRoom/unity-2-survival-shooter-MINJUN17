using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle soundToggle;
    public AudioSource bgmSource;
    public AudioSource[] sfxSources; 

    private bool isPaused = false;
    private float savedBGMVolume = 1f;
    private float savedSFXVolume = 1f;

    private void Start()
    {
        bgmSlider.value = 1f;
        sfxSlider.value = 1f;
        soundToggle.isOn = true;
        pauseMenuUI.SetActive(false);

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        soundToggle.onValueChanged.AddListener(OnToggleSound);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void SetBGMVolume(float volume)
    {
        savedBGMVolume = volume;
        if (soundToggle.isOn)
            bgmSource.volume = volume;
    }

    private void SetSFXVolume(float volume)
    {
        savedSFXVolume = volume;
        if (soundToggle.isOn)
        {
            foreach (var source in sfxSources)
                source.volume = volume;
        }
    }

    private void OnToggleSound(bool isOn)
    {
        bgmSource.volume = isOn ? savedBGMVolume : 0f;
        foreach (var source in sfxSources)
            source.volume = isOn ? savedSFXVolume : 0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}